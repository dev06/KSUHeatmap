using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour {

	public static Cluster Instance;

	public GameObject clusterPrefab;
	public GameObject centroidPrefab;
	public GameObject AverageCentroid;
	public GameObject Outer; 
	public List<ClusterPoint> redPoints;
	public List<ClusterPoint> bluePoints;
	public List<Centroid> centroids = new List<Centroid>();
	public List<GameObject> pins = new List<GameObject>();
	public List<GameObject> outerPins = new List<GameObject>(); 

	public int clusterSize = 6;
	public float pointScale = .2f;
	public float centroidCenterDelay = .4f;


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

	}


	public void CreateCentroid()
	{
		for (int i = 0; i < clusterSize; i++)
		{
			GameObject centroidClone = Instantiate(centroidPrefab) as GameObject;
			centroidClone.transform.SetParent(transform);
			Centroid centroid = centroidClone.GetComponent<Centroid>();
			int ID = GenerateID();
			centroid.Init(ID);
			centroids.Add(centroid);
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


	void Update () {

		if (Input.GetKeyDown(KeyCode.Space))
		{
			for (int i = 0 ; i < centroids.Count; i++)
			{
				centroids[i].CalculatePoint();
			}
		}

<<<<<<< HEAD
		// if (Input.GetKeyDown(KeyCode.M))
		// {
		// 	CalculateAverageCentroid();
		// }
		// if (Input.GetKeyDown(KeyCode.R))
		// {
		// 	for (int i = 0 ; i < pins.Count; i++)
		// 	{
		// 		if (pins[i] == null) { continue;  }
		// 		float mag = pins[i].transform.position.magnitude;
		// 		if (mag > high)
		// 		{
		// 			Destroy(pins[i].gameObject);
		// 		}
		// 	}
		// }
=======
		if (Input.GetKeyDown(KeyCode.M))
		{
			CalculateAverageCentroid();
		}

		if(Input.GetKeyDown(KeyCode.K))
		{
			CreateOuterPins(); 
		}
	}

	private void CreateOuterPins()
	{
		for(int i = 0; i < pins.Count; i++)
		{
			CreateEdges(pins[i]); 
		}
>>>>>>> b9b5d553d7df2fac20726a7ebe1efeb17736aa43
	}

	public void CalculateAverageCentroid()
	{
		for (int i = 0 ; i < centroids.Count; i++)
		{
			CreatePinCentroid(centroids[i], i);
		}
	}

	private void CreatePinCentroid(Centroid c, int ia)
	{
		for (int i = 0; i < centroids.Count; i++)
		{
			if (ia == i) { break; }
			if (centroids[i].ID != c.ID)
			{
				Centroid other = centroids[i];
				float distanceTo = Vector3.Distance(other.position, c.position);
				if (distanceTo > 5f) { continue; }
				float x = c.position.x + other.position.x;
				float z = c.position.z + other.position.z;
				x /= 2;
				z /= 2;
				Vector3 position = new Vector3(x, 0, z);
				GameObject pinClone = Instantiate(AverageCentroid) as GameObject;
				pinClone.transform.SetParent(transform);
				pinClone.transform.position = position;
				pins.Add(pinClone);
			}
		}
	}

	private void CreateEdges(GameObject current)
	{
		float threshold = .001f; 
		for(int i = 0;i < pins.Count; i++)
		{
			if(pins[i] == current) continue; 
			bool hasLeft = false; 
			bool hasRight = false; 

			bool isEdge = false; 

			if(pins[i].transform.position.x < current.transform.position.x)
			{
				float distance = Vector3.Distance(current.transform.position, pins[i].transform.position); 


				if(distance < threshold)
				{
					hasLeft = true; 
					continue; 

				}
			}

			if(pins[i].transform.position.x > current.transform.position.x)
			{
				float distance = Vector3.Distance(current.transform.position, pins[i].transform.position); 

				if(distance < threshold)
				{
					hasRight = true; 
				}
			}

			if( hasLeft)
			{
				isEdge = false; 
			}
			else
			{
				isEdge = true; 
			}


			if(isEdge)
			{


				outerPins.Add(current); 

			}
		}


		for(int i = 0;i < outerPins.Count; i++)
		{
			GameObject pinClone = Instantiate(Outer) as GameObject;
			pinClone.transform.SetParent(transform);
			pinClone.transform.position = outerPins[i].transform.position + new Vector3(0, 3, 0); 
		}

	}
	public Centroid GetClosest(Centroid c)
	{
		Centroid closest = centroids[0];
		for (int i = 0; i < centroids.Count; i++)
		{
			if (centroids[i] != c)
			{
				float dist = Vector3.Distance(c.position, centroids[i].position);

				if (dist < Vector3.Distance(c.position, closest.position))
				{
					closest = centroids[i];
				}

			}
		}
		return closest;
	}

	public void BuildPoints(List<string> clusterPoints)
	{
		StopCoroutine("CreatePoints");
		StopCoroutine("AdjustPoints");

		//StartCoroutine("CreatePoints", clusterPoints);
		CreatePoints(clusterPoints);
		StartCoroutine("AdjustPoints");


	}

	public IEnumerator AdjustPoints()
	{
		while (true)
		{
			for (int ia = 0 ; ia < centroids.Count; ia++)
			{
				centroids[ia].CalculatePoint();
				yield return new WaitForSeconds(centroidCenterDelay);
			}
		}
	}


	public void CreatePoints(List<string> clusterPoints)
	{

		for (int i = 0 ; i < clusterPoints.Count; i++)
		{
			float x = float.Parse(clusterPoints[i].Split(',')[1]);
			float z = float.Parse(clusterPoints[i].Split(',')[2]);

			GameObject clone = Instantiate(clusterPrefab) as GameObject;
			clone.transform.SetParent(transform);
			Vector3 position = new Vector3(x, 0, z);
			clone.transform.localScale = new Vector3(pointScale, .02f, pointScale);
			clone.transform.position = position;
			int ID = Random.Range(0, clusterSize);
			ClusterPoint point = clone.GetComponent<ClusterPoint>();
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

		// yield return new WaitForSeconds(4);

		// ClearArea(true);
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


		if (replay)
		{
			DataParser.Instance.Replay();
		}

	}

}
