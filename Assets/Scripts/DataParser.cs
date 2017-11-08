using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataParser : MonoBehaviour {

	public static DataParser Instance;
	public List<KSUHeatmap.DataPoint> dataPoints = new List<KSUHeatmap.DataPoint>();
	public float width = 0;
	public float height = 0;

	private string fileContents = "";
	private  string saveLocation;


	private string locationFileContents;
	private string beaconFileContents;

	public Location activeLocation;  // represents one active location
	public List<Session> activeSession; // represents current session of the active location

	private static int displayPointFrequency = 4;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start () {

		Init();
	}

	private void Init()
	{
		activeLocation = ParseLocation(Application.dataPath + "/Datapoint/location.txt");

		activeSession = ParseDatapoints(Application.dataPath + "/Datapoint/updated_data.csv");

		BuildAll();
	}

	public void Replay(Transform t)
	{
		foreach (Transform tt in t)
		{
			Destroy(tt.gameObject);
		}

		BuildPath(activeSession);
	}

	public  void BuildAll()
	{
		BuildPath(activeSession);
		BuildWalls(activeLocation);
		BuildBeacons(activeLocation);
	}

	public  void BuildPath(List<Session> activeSession, int session = -1)
	{
		if (session != -1)
		{
			DataBuilder.INSTANCE.BuildData(activeSession[session].datapoints);
			return;
		}

		List<string> displayPoints = new List<string>();


		Cluster.Instance.CreateCentroid();
		for (int i = 0 ; i < activeSession.Count; i++)
		{
			if (i > 0) { break; }
			for (int j = 0; j < activeSession[i].datapoints.Count; j++)
			{
				if (displayPoints.Count == 0)
				{
					displayPoints.Add(activeSession[i].datapoints[j]);
				} else
				{
					string prevPoint = activeSession[i].datapoints[j - 1];
					float px = float.Parse(prevPoint.Split(',')[1]);
					float py = float.Parse(prevPoint.Split(',')[2]);
					Vector2 pv = new Vector2(px, py);

					string currentPoint = activeSession[i].datapoints[j];
					float nx = float.Parse(currentPoint.Split(',')[1]);
					float ny = float.Parse(currentPoint.Split(',')[2]);
					Vector2 nv = new Vector2(nx, ny);

					float distance = Mathf.Abs(Vector2.Distance(nv, pv));

					if (distance > .01f)
					{
						displayPoints.Add(currentPoint);
					}
				}
			}

			Cluster.Instance.BuildPoints(displayPoints);

		}
	}

	public  void BuildWalls(Location location)
	{
		if (location.GetWalls() == null)
		{
			Debug.LogWarning("Location's wall is null");
			return;
		}

		List<string> walls = new List<string>();

		for (int i = 0 ; i < location.GetWalls().Count; i++)
		{
			string coordinate = location.GetWalls()[i].x + "," + location.GetWalls()[i].y;
			walls.Add(coordinate);
		}

		LocationBuilder.BuildWalls(walls);
	}


	public  void BuildBeacons(Location location)
	{
		if (location.GetBeacons() == null)
		{
			Debug.Log("Locations's Beacons are null.");
			return;
		}

		List<string> beacons = new List<string>();

		for (int i = 0 ; i < location.GetBeacons().Count; i++)
		{
			Beacon beacon = location.GetBeacons()[i];
			string format = beacon.id + "," + beacon.x + "," + beacon.y + "," + beacon.orientation;
			beacons.Add(format);
		}

		LocationBuilder.BuildBeacons(beacons);

	}

	public Location ParseLocation(string path)
	{
		/// <summary>
		///	Parses the location to create a room
		/// +path -> path of the file
		/// </summary>

		locationFileContents = System.IO.File.ReadAllText(path);

		string[] data = locationFileContents.Split('\n');

		List<Beacon> beacons = new List<Beacon>();

		List<Wall> walls = new List<Wall>();

		string locationName = data[0].Split(',')[0];

		string locationID = data[0].Split(',')[1];

		float width = float.Parse(data[1].Split(',')[2]);

		float height = float.Parse(data[1].Split(',')[3]);

		int orientation = int.Parse((data[2].Split(',')[0]));

		int endIndex = 0;

		for (int i = 3; i < data.Length; i++)
		{
			if (data[i].Contains("END")) {

				endIndex = i;

				break;
			}

			string[] beaconInfo = data[i].Split(',');

			Beacon beacon = new Beacon(beaconInfo[0], float.Parse(beaconInfo[1]), float.Parse(beaconInfo[2]), int.Parse(beaconInfo[3]));

			beacons.Add(beacon);

		}

		for (int i = endIndex + 1; i < data.Length; i++)
		{
			float x = float.Parse(data[i].Split(',')[0]);

			float y = float.Parse(data[i].Split(',')[1]);

			Wall wall = new Wall(x, y);

			walls.Add(wall);
		}



		Location location = new Location(locationName, locationID, width, height, orientation, beacons, walls);

		return location;

	}


	public List<Session> ParseDatapoints(string path)
	{
		/// <summary>
		/// Reads datapoints and creates session. Each session is extracted between "END" keyword found in the file
		/// x,y,z,0,1,2,3		--> represents one session
		/// x,y,z,0,1,2,3
		///	END
		/// x,y,z,0,1,2,3		--> represents second session
		/// x,y,z,0,1,2,3
		/// </summary>
		beaconFileContents = System.IO.File.ReadAllText(path);

		Session s = new Session();

		string[] sessionData = beaconFileContents.Split(new string[] {"END"}, System.StringSplitOptions.None);

		List<Session> sessions = new List<Session>();

		for (int i = 0; i < sessionData.Length; i++)
		{
			string[] currentSessionData = sessionData[i].Split('\n');

			Session session;

			List<KSUHeatmap.DataPoint> points = new List<KSUHeatmap.DataPoint>();

			List<string> datapoints = new List<string>();

			for (int j = 0 ; j < currentSessionData.Length; j++)
			{
				string[] contents = currentSessionData[j].Split(',');

				if (contents.Length <= 1) { continue; }

				string localTime = contents[0];

				float x = float.Parse(contents[1]);

				float y = float.Parse(contents[2]);

				int orientation = int.Parse(contents[3]);

				List<string> closestID = new List<string>();

				for (int k = 4; k < contents.Length; k++)
				{
					closestID.Add(contents[k]);
				}

				KSUHeatmap.DataPoint dp = new KSUHeatmap.DataPoint(localTime, new Vector2(x, y), orientation, closestID);

				points.Add(dp);

				string concat = localTime + "," + x + "," + y + "," + orientation  + "," + closestID[0] + "," + closestID[1] + "," + closestID[2];

				datapoints.Add(concat);

			}

			session = new Session(datapoints);

			sessions.Add(session);

		}


		sessions.RemoveAt(sessions.Count - 1);
		return sessions;
	}


	public string GetFileContents(string path)
	{
		string text = "";
		StreamReader reader = new StreamReader(path);

		while (!reader.EndOfStream)
		{
			text += reader.ReadLine() + "\n";
		}

		reader.Close();

		return text;
	}


}
