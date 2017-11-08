using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour {

	public static Cluster Instance;

	public GameObject clusterPrefab;
	public GameObject centroidPrefab;
	public List<ClusterPoint> redPoints;
	public List<ClusterPoint> bluePoints;
	public static List<Centroid> centroids = new List<Centroid>();

	public int clusterSize = 6;
	public float pointScale = .2f;


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

		if (Input.GetKeyDown(KeyCode.M))
		{
			Merge();
		}

	}

	public void Merge()
	{
		for (int i = 0 ; i < centroids.Count; i++)
		{
			Centroid c = centroids[i];

			Centroid closest = GetClosest(c);

			c.MergeWith(closest);
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
		StartCoroutine("CreatePoints", clusterPoints);
		//CreatePoints(clusterPoints);

		StartCoroutine("AdjustPoints");


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


	public IEnumerator CreatePoints(List<string> clusterPoints)
	{
		for (int i = 0 ; i < clusterPoints.Count; i++)
		{
			float x = float.Parse(clusterPoints[i].Split(',')[1]);
			float z = float.Parse(clusterPoints[i].Split(',')[2]);

			GameObject clone = Instantiate(clusterPrefab) as GameObject;
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
			yield return null;
		}

		for (int i = 0 ; i < centroids.Count; i++)
		{
			centroids[i].CalculateCenter();
		}

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
		}

		return null;
	}

}
