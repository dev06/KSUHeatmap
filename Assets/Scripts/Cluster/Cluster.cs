using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cluster : MonoBehaviour {

	public static Cluster Instance;

	public GameObject miniCentroid; 
	public GameObject clusterPrefab;
	public GameObject centroidPrefab;
	public GameObject AverageCentroid;
	public List<ClusterPoint> allPoints = new List<ClusterPoint>(); 
	public List<ClusterPoint> redPoints;
	public List<ClusterPoint> bluePoints;
	public List<Centroid> centroids = new List<Centroid>();
	public List<GameObject> pins = new List<GameObject>();
	public Location location; 
	public int clusterSize = 6;
	public float pointScale = .5f;
	public float centroidCenterDelay = .4f;
	private Unwalkable unwalkable; 
	private Spawner boxSpawner; 
	private PathGeneration pathGeneration; 
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		unwalkable = new Unwalkable(); 


		//pathGeneration = new PathGeneration(centroids); 
	}

	void Start()
	{
		boxSpawner = FindObjectOfType<Spawner>(); 
	}

	GameObject centroidClone;
	Centroid centroid;
	public void CreateCentroid()
	{
		for (int i = 0; i < clusterSize; i++)
		{
			centroidClone = Instantiate(centroidPrefab) as GameObject;
			centroidClone.transform.SetParent(transform);
			centroid = centroidClone.GetComponent<Centroid>();
			int ID = GenerateID();
			centroid.Init(ID);
			centroids.Add(centroid);
		}

	}





	void Update () {

		for (int i = 0 ; i < centroids.Count; i++)
		{
			centroids[i].CalculatePoint();
		}

		if(Input.GetKey(KeyCode.Space))
		{
			//centroids[0].AssignMiniCentroidPoints();
			
		}



		if (Input.GetKeyDown(KeyCode.I))
		{
			
		}

		if (Input.GetKeyDown(KeyCode.H))
		{

		}




		if (Input.GetKeyDown(KeyCode.G))
		{
			// CalculateAverageCentroid();
			GenerateMesh(); 
				//centroids[i].GeneratePath();
				//centroids[i].FilterMiniCentroids(); 
			//GenerateUnwalkableMesh(); 
		}
	}

	public void SpawnBox()
	{
		boxSpawner.DestroyBoxes(); 
		boxSpawner.SpawnBoxes(location); 
	}

	public void HideBox()
	{
		boxSpawner.DestroyBoxes();  
	}

	public void CalculateAverageCentroid()
	{
		// for (int i = 0 ; i < centroids.Count; i++)
		// {
		// 	CreatePinCentroid(centroids[i], i);
		// }
	}

	public void GenerateUnwalkableMesh()
	{
		for(int i = 0;i < centroids.Count; i++)
		{
			Centroid current = centroids[i]; 
			for(int j = 0; j < current.points.Count; j++)
			{
				if(current.points[j].Active)
				{					
					allPoints.Add(current.points[j]); 
				}
			}
		}

		unwalkable.GenerateUnwalkableMesh(location, allPoints); 

	}

	public void GenerateMesh()
	{
		List<Vector2> points = new List<Vector2>(); 
		for(int i =0;i < centroids.Count; i++)
		{
			Centroid currentCentroid = centroids[i]; 
			for(int j =0; j < currentCentroid.miniCentroid.Count; j++)
			{
				MiniCentroid mini = currentCentroid.miniCentroid[j]; 

				List<Vector2> active = mini.GetActivePoints(); 
				
				for(int k = 0;k < active.Count; k++)
				{
					points.Add(active[k]); 
				}
			}
		}


		FindObjectOfType<Navigation.Grid>().DisplayNavmesh(points);

	}



	public void BuildPoints(List<string> clusterPoints, Location location)
	{
		//StopCoroutine("CreatePoints");
		//StopCoroutine("AdjustPoints");
		this.location = location; 
		//StartCoroutine("CreatePoints", clusterPoints);
		//CreatePoints(clusterPoints);
		//StartCoroutine("AdjustPoints");


	}

	public IEnumerator AdjustPoints()
	{
		while (true)
		{
			for (int ia = 0 ; ia < centroids.Count; ia++)
			{
				centroids[ia].CalculatePoint();
				yield return null;
			}
		}
	}


	public void CreatePoints(List<string> clusterPoints)
	{
		float x, z;
		GameObject clone;
		Vector3 position;
		int ID;
		ClusterPoint point;
		float scaleMultiplier = 5f; 
		for (int i = 0 ; i < clusterPoints.Count; i++)
		{
			x = float.Parse(clusterPoints[i].Split(',')[1]);
			z = float.Parse(clusterPoints[i].Split(',')[2]);

			clone = Instantiate(clusterPrefab) as GameObject;
			clone.transform.SetParent(transform);
			position = new Vector3(x, 0, z);
			clone.transform.localScale = new Vector3(pointScale, pointScale, pointScale) * scaleMultiplier;
			clone.transform.position = position;
			clone.name = "point" + i;
			ID = Random.Range(0, clusterSize);
			point = clone.GetComponent<ClusterPoint>();
			point.Init(ID);
			for (int j = 0; j < centroids.Count; j++)
			{
				if (centroids[j].ID == ID)
				{
					centroids[j].points.Add(point);
					point.SetCentroid(centroids[j]);
				}
			}
			//yield return null;
		}

		for (int i = 0 ; i < centroids.Count; i++)
		{
			centroids[i].CalculateCenter();
		}

		//Debug.Log("childs" + transform.childCount);

		// yield return new WaitForSeconds(4);

		// ClearArea(true);
	}

	public void CreateVectorPoints(List<Vector2> points)
	{
		StopCoroutine("ICreateVectorPoints"); 
		StartCoroutine("ICreateVectorPoints",points); 
	}

	public IEnumerator ICreateVectorPoints(List<Vector2> points)
	{
		float scaleMultiplier = 5f; 

		for(int i = 0; i < points.Count; i++)
		{
			GameObject clone = (GameObject)Instantiate(clusterPrefab); 
			clone.transform.SetParent(transform); 
			clone.transform.position = new Vector3(points[i].x, 0, points[i].y); 
			clone.transform.localScale = new Vector3(pointScale, pointScale, pointScale) * scaleMultiplier; 
			clone.name = "point" + i; 
			int ID = Random.Range(0, clusterSize); 
			ClusterPoint point = clone.GetComponent<ClusterPoint>(); 
			point.Init(ID); 

			for(int j = 0;j < centroids.Count; j++)
			{
				if(centroids[j].ID == ID)
				{
					centroids[j].points.Add(point); 
					point.SetCentroid(centroids[j]); 
				}
			}

			if(i %  200 ==0)
			{
				yield return null; 
			}
		}

		for (int i = 0 ; i < centroids.Count; i++)
		{
			centroids[i].CalculateCenter();
		}

		yield return null; 
	}




	public static Material GetMaterialByID(int id)
	{
		switch (id)
		{
			case 0: return SystemResources.red;
			case 1: return SystemResources.blue;
			case 2: return SystemResources.green;
			case 3: return SystemResources.yellow;
			case 4: return SystemResources.orange;
			case 5: return SystemResources.pink;
			case 6: return SystemResources.color_1;
			case 7: return SystemResources.color_2;
			case 8: return SystemResources.color_3;
			case 9: return SystemResources.color_4;
			case 10: return SystemResources.color_5;
			case 11: return SystemResources.color_6;


		}

		return null;
	}

	public void ClearArea(bool replay = true)
	{

		StopCoroutine("CreatePoints");
		StopCoroutine("AdjustPoints");


		foreach (Transform tt in transform)
		{
			Destroy(tt.gameObject);
		}
		centroids.Clear();
		
		pins.Clear();

		allPoints.Clear(); 

		unwalkable.Clear(); 	


		if (replay)
		{
			DataParser.Instance.Replay();
		}

	}


	int GenerateID()
	{
		int ID = Random.Range(0, clusterSize);
		do
		{
			ID = Random.Range(0, clusterSize);

		} while (IDExists(ID));

		return ID;
	}

	bool IDExists(int ID)
	{
		for (int i = 0; i < centroids.Count; i++)
		{
			if (centroids[i].ID == ID)
			{
				return true;
			}
		}

		return false;
	}


	public void AssignMiniCentroid()
	{

		for(int i =0 ;i < centroids.Count; i++)
		{
			centroids[i].AssignMinicentroidPoints();
		}		
	}

	public void FilterMinicentroidPoints()
	{
		for(int i =0 ;i < centroids.Count; i++)
		{
			centroids[i].FilterMiniCentroids(); 
		}	
	}



}
