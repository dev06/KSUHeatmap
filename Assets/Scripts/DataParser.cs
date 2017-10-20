using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataParser : MonoBehaviour {

	public static DataParser Instance;
	public List<DataPoint> dataPoints = new List<DataPoint>();
	public float width = 0;
	public float height = 0;

	private string fileContents = "";
	private  string saveLocation;


	private string locationFileContents;
	private string beaconFileContents;

	public Location activeLocation;  // represents one active location
	public List<Session> activeSession; // represents current session of the active location



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

		activeSession = ParseDatapoints(Application.dataPath + "/Datapoint/datapoint.csv");
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

			List<DataPoint> points = new List<DataPoint>();

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

				DataPoint dp = new DataPoint(localTime, new Vector2(x, y), orientation, closestID);

				points.Add(dp);

			}

			session = new Session(points);

			sessions.Add(session);

		}
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
