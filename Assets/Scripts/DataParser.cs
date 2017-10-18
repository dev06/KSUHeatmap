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

	public Location activeLocation;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start () {


		Init();

		// saveLocation =  Application.dataPath + "/Datapoint/datapoint.csv";
		// fileContents = System.IO.File.ReadAllText(saveLocation);

		// string[] data = fileContents.Split('\n');
		// for (int i = 0; i < data.Length; i++)
		// {


		// 	string[] vals = data[0].Split(',');
		// 	width = float.Parse(vals[2]);
		// 	height = float.Parse(vals[3]);

		// 	if (i == 0) { continue; }
		// 	string[] values = data[i].Split(',');
		// 	DataPoint point = null;
		// 	for (int ii = 0; ii < values.Length; ii++)
		// 	{
		// 		float x = float.Parse(values[0]);
		// 		float y = float.Parse(values[1]);
		// 		//	float acc = float.Parse(values[3]);
		// 		//	Orientation o = (Orientation)System.Enum.Parse( typeof( Orientation ), values[2] );
		// 		point = new DataPoint(new Vector2(x / width , y / height ), 0, Orientation.HIGH);
		// 	}
		// 	dataPoints.Add(point);
		// }

	}

	private void Init()
	{
		activeLocation = ParseLocation();
		ParseDatapoints();
	}

	public Location ParseLocation()
	{
		locationFileContents = System.IO.File.ReadAllText(Application.dataPath + "/Datapoint/location.txt");
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

	public void ParseDatapoints()
	{
		beaconFileContents = System.IO.File.ReadAllText(Application.dataPath + "/Datapoint/datapoint.csv");
		Session s = new Session();
		string[] sessionData = beaconFileContents.Split(new string[] {"END"}, System.StringSplitOptions.None);

		for (int i = 0; i < sessionData.Length; i++)
		{
			string[] datapoint = sessionData[i].Split('\n');



			for (int j = 0; j < datapoint.Length; j++)
			{

				string[] datapointContents = datapoint[j].Split(',');

				if (string.IsNullOrEmpty(datapoint[j])) continue;
				if (datapoint[j].Length <= 1) continue;
				Debug.Log(datapoint[j]);



			}
			//Debug.Log(data[i]);
		}
		// string[] data = beaconFileContents.Split("END");
		// int endIndex = 0;
		// List<DataPoint> points = new List<DataPoint>();




		// for (int i = 0; i < data.Length; i++)
		// {
		// 	if (data[i].Contains("END"))
		// 	{
		// 		endIndex = i;
		// 		break;
		// 	}
		// 	string[] datapointContents = data[i].Split(',');


		// 	string localTime = datapointContents[0];
		// 	float x = float.Parse(datapointContents[1]);
		// 	float y = float.Parse(datapointContents[2]);
		// 	string orientation = datapointContents[3];
		// 	List<string> closestBeaconId = new List<string>();

		// 	for (int j = 4; j < datapointContents.Length; j++)
		// 	{
		// 		closestBeaconId.Add(datapointContents[j]);
		// 	}

		// 	DataPoint dp = new DataPoint(localTime, new Vector2(x, y), int.Parse(orientation), closestBeaconId);
		// 	points.Add(dp);
		// }

		// s.SetDatapoints(points);

		// Debug.Log(s.datapoints);
		// for (int i = 0; i < s.datapoints.Count; i++)
		// {
		// 	Debug.Log(s.datapoints[i]);
		// }

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
