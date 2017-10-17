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
		Debug.Log(ParseLocation().ToString());
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

		SystemEnum.Orientation orientation = (SystemEnum.Orientation) System.Enum.Parse (typeof (SystemEnum.Orientation), (data[2].Split(',')[0]).ToUpper());
		int endIndex = 0;
		for (int i = 3; i < data.Length; i++)
		{
			if (data[i].Contains("END")) {
				endIndex = i;
				break;
			}

			string[] beaconInfo = data[i].Split(',');

			Beacon beacon = new Beacon(beaconInfo[0], float.Parse(beaconInfo[1]), float.Parse(beaconInfo[2]), SystemEnum.Orientation.NORTH);

			beacons.Add(beacon);

		}

		for (int i = endIndex + 1; i < data.Length; i++)
		{
			float x = float.Parse(data[i].Split(',')[0]);
			float y = float.Parse(data[i].Split(',')[1]);
			Wall wall = new Wall(x, y);
			walls.Add(wall);
		}

		Debug.Log(walls.Count);

		Location location = new Location(locationName, locationID, width, height, orientation, beacons, walls);

		return location;

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
