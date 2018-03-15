using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataParser : MonoBehaviour {

	public static DataParser Instance;
	public List<KSUHeatmap.DataPoint> dataPoints = new List<KSUHeatmap.DataPoint>();
	public string locationfile;
	public string datafile;
	public float width = 0;
	public float height = 0;

	private string dataPath;
	private string locationPath; 

	private string fileContents = "";
	private  string saveLocation;


	private string locationFileContents;
	private string beaconFileContents;

	public Location activeLocation;  // represents one active location
	public List<Session> activeSession; // represents current session of the active location

	private static int displayPointFrequency = 4;

	public float distanceThreshold = .1f;

	public GameObject layout_guitar;
	public GameObject layout_atrium;
	public LayoutOpacity layout_opacity;

	private Cluster cluster; 

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public void Init()
	{

		//activeSession = ParseDatapoints(Application.dataPath + "/Datapoint/" + fileName + ".csv");

		// if (locationPath == null)
		activeLocation = ParseLocation(Application.streamingAssetsPath + "/Location/" + locationPath);
		// else
       // activeLocation = ParseLocation(Application.streamingAssetsPath + "/Location" + locationPath);
		activeSession = ParseDatapoints(Application.streamingAssetsPath + "/Data/" + dataPath);


		cluster = FindObjectOfType<Cluster>(); 

		cluster.location = activeLocation; 

        // if (datafile == null)
        // activeSession = ParseDatapoints(Application.streamingAssetsPath + "/Data" + dataPath);
        // else

		if(locationfile.Contains("GuitarLab"))
		{
			layout_guitar.SetActive(true);
			layout_atrium.SetActive(false);
		}
		else if (locationfile.Contains("Atrium"))
		{
			layout_guitar.SetActive(false);
			layout_atrium.SetActive(true);
		}
		layout_opacity.Reset();
		BuildAll();
	}

	public void Replay(Transform t)
	{
		BuildPath(activeSession);
	}

	public void Replay()
	{
		BuildPath(activeSession);
	}
	public  void BuildAll()
	{

		BuildWalls(activeLocation);
		BuildPath(activeSession);
		BuildBeacons(activeLocation);

        //set up camera to see whole room
		Camera thiscam = GetComponent<Camera>();
		thiscam.orthographicSize = width / 2;
		if (thiscam.orthographicSize < (height/2) + .5f)
		thiscam.orthographicSize = (height/2) + .5f;
		transform.position = new Vector3((thiscam.orthographicSize * 1.4f), transform.position.y, height/2);
	}

	public  void BuildPath(List<Session> activeSession)
	{
		List<string> displayPoints = new List<string>();
		List<Vector2> navigationPoints = new List<Vector2>();

		Cluster.Instance.CreateCentroid();
		float minDist = distanceThreshold * distanceThreshold;
		string prevPoint, currentPoint;
		float distance;
		Vector2 pv = new Vector2(), nv = new Vector2();
		for (int i = 0 ; i < activeSession.Count; i++)
        //for (int i = 0; i < 1; i++)
		{
            //if (i > 0) { break; }
			for (int j = 0; j < activeSession[i].datapoints.Count; j++)
			{
				if (j == 0)
				{

					displayPoints.Add(activeSession[i].datapoints[0]);

					prevPoint = activeSession[i].datapoints[0];

					pv.x = float.Parse(prevPoint.Split(',')[1]);

					pv.y = float.Parse(prevPoint.Split(',')[2]);

     navigationPoints.Add(pv);           //add the vector 2 to keep track for navigation
 } 
 else
 {

                    //***Moved to make it so after you calc distance, set pv to nv***\\
					//prevPoint = activeSession[i].datapoints[j - 1];

					//px = float.Parse(prevPoint.Split(',')[1]);

					//py = float.Parse(prevPoint.Split(',')[2]);

					//pv = new Vector2(px, py);


 	currentPoint = activeSession[i].datapoints[j];

 	nv.x = float.Parse(currentPoint.Split(',')[1]);

 	nv.y = float.Parse(currentPoint.Split(',')[2]);

 	distance = Vector2.SqrMagnitude(nv - pv);

                    //displayPoints.Add(currentPoint);
 	if (distance > minDist)
 	{
 		displayPoints.Add(currentPoint);
 	}

                    //set previous vector to the current vector (previous for next point)
 	pv.x = nv.x;
 	pv.y = nv.y;

                    navigationPoints.Add(pv);       //add the vector 2 to keep track for navigation
                }
            }



        }

      //  Cluster.Instance.BuildPoints(displayPoints, activeLocation);
        FindObjectOfType<Navigation.Grid>().SetData(navigationPoints);
        displayPoints.Clear();
    }

    public  void BuildWalls(Location location)
    {
    	if (location.GetWalls() == null)
    	{
    		Debug.LogWarning("Location's wall is null");
    		return;
    	}

    	List<string> walls = new List<string>();
    	float maxx = 0;
    	float maxy = 0;
    	for (int i = 0 ; i < location.GetWalls().Count; i++)
    	{
            //find the maximum x (aka the width)
    		if(location.GetWalls()[i].x > maxx)
    		{
    			maxx = location.GetWalls()[i].x;
    		}
            //find the maximum y (aka the height)
    		if (location.GetWalls()[i].y > maxy)
    		{
    			maxy = location.GetWalls()[i].y;
    		}
    		string coordinate = location.GetWalls()[i].x + "," + location.GetWalls()[i].y;
    		walls.Add(coordinate);
    	}
    	width = maxx;
    	height = maxy;

    	FindObjectOfType<Navigation.Grid>().roomscale = new Vector2(width, height);
    	FindObjectOfType<Navigation.Grid>().GenerateCells();

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
        //Debug.Log("Session Count" + sessionData.Length);
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

    			if (contents.Length <= 1)
    			{
    				continue;
    			}

    			string localTime = contents[0];

    			float x = float.Parse(contents[1]);

    			float y = float.Parse(contents[2]);

    			int orientation = int.Parse(contents[3]);

    			List<string> closestID = new List<string>();
    			
    			string concat = localTime + "," + x + "," + y + "," + orientation;
    			for (int k = 4; k < contents.Length; k++)
    			{
    				closestID.Add(contents[k]);
    				concat += "," + contents[k];
    			}

    			KSUHeatmap.DataPoint dp = new KSUHeatmap.DataPoint(localTime, new Vector2(x, y), orientation, closestID);

    			points.Add(dp);

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

    public void SetDataPath(string path)
    {
    	this.dataPath = path; 
    }

    public void SetLocationPath(string path)
    {
    	this.locationPath = path; 
    }

    public string LocationPath
    {
    	get
    	{
    		return locationPath; 
    	}
    }


}
