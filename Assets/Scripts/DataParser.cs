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

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start () {


		//Debug.Log(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));

		saveLocation =  Application.dataPath + "/Datapoint/datapoint.csv";
		fileContents = System.IO.File.ReadAllText(saveLocation);

		string[] data = fileContents.Split('\n');
		for (int i = 0; i < data.Length; i++)
		{


			string[] vals = data[0].Split(',');
			width = float.Parse(vals[2]);
			height = float.Parse(vals[3]);

			if (i == 0) { continue; }
			string[] values = data[i].Split(',');
			DataPoint point = null;
			for (int ii = 0; ii < values.Length; ii++)
			{
				float x = float.Parse(values[0]);
				float y = float.Parse(values[1]);
				//	float acc = float.Parse(values[3]);
				//	Orientation o = (Orientation)System.Enum.Parse( typeof( Orientation ), values[2] );
				point = new DataPoint(new Vector2(x / width , y / height ), 0, Orientation.HIGH);
			}
			dataPoints.Add(point);
		}

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
