using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour {

	public static Cluster Instance;

	public GameObject clusterPrefab;
	public GameObject centroidPrefab;
	public GameObject AverageCentroid;
	public List<ClusterPoint> redPoints;
	public List<ClusterPoint> bluePoints;
	public List<Centroid> centroids = new List<Centroid>();
	public List<GameObject> pins = new List<GameObject>();

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
            CalculateAverageCentroid();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < pins.Count; i++)
            {
                if (pins[i] == null) { continue; }
                float mag = pins[i].transform.position.magnitude;
                if (mag > high)
                {
		 			Destroy(pins[i].gameObject);
		 		}
		 	}
		 }
	}

	public void CalculateAverageCentroid()
	{
		for (int i = 0 ; i < centroids.Count; i++)
		{
			CreatePinCentroid(centroids[i], i);
		}
		FindOutlier();
	}

    Centroid other;
    float distanceTo, x, z;
    Vector3 position;
    GameObject pinClone;
    private void CreatePinCentroid(Centroid c, int ia)
	{
        
		for (int i = 0; i < centroids.Count; i++)
		{
			if (ia == i) { break; }
			if (centroids[i].ID != c.ID)
			{
				other = centroids[i];
				distanceTo = Vector3.SqrMagnitude(other.position - c.position);
				if (distanceTo > 25f) { continue; }
				x = c.position.x + other.position.x;
				z = c.position.z + other.position.z;
				x /= 2;
				z /= 2;
				position = new Vector3(x, 0, z);
				pinClone = Instantiate(AverageCentroid) as GameObject;
				pinClone.transform.SetParent(transform);
				pinClone.transform.position = position;
				pins.Add(pinClone);
			}
		}
	}

	private float high = 0;
	private void FindOutlier()
	{
		List<float> magnitudes = new List<float>();
		for (int i = 0; i < pins.Count; i++)
		{
			if (pins[i] == null) { continue; }
			magnitudes.Add((float)(pins[i].transform.position.magnitude));
		}




		magnitudes.Sort();

		float q1 = 0;
		float q3 = 0;
		float iqr = 0;

		float highBound = 0;
		float lowBound = 0;

		for (int i = 0; i < magnitudes.Count / 2; i++)
		{
			//Debug.Log(magnitudes[i]);
			q1 += magnitudes[i];
		}

		//	Debug.Log(" ");


		for (int i = magnitudes.Count / 2; i < magnitudes.Count; i++)
		{	//Debug.Log(magnitudes[i]);

			q3 += magnitudes[i];
		}
		q1 /= (magnitudes.Count / 2);
		q3 /= (magnitudes.Count / 2);

		iqr = q3 - q1;

		highBound = (q3 + ( .5f * iqr));
		lowBound = (q1 - (1.5f * iqr));

		// Debug.Log("Q1: " + q1);
		// Debug.Log("Q3: " + q3);
		// Debug.Log("IQR: " + iqr);
		//	Debug.Log(highBound);
		// Debug.Log(lowBound + "\n");


		high = highBound;



	}

	public void Merge()
	{
        Centroid c;
        float distanceToNext;
		for (int i = 0 ; i < centroids.Count; i++)
		{
			c = centroids[i];

			if (i > centroids.Count - 2) { break; }

			distanceToNext = Vector3.SqrMagnitude(c.position - centroids[i + 1].position);

			if (distanceToNext < 16f)
			{
				c.MergeWith(centroids[i + 1]);
			}
		}
	}

	public Centroid GetClosest(Centroid c)
	{
		Centroid closest = centroids[0];
        float dist;
		for (int i = 0; i < centroids.Count; i++)
		{
			if (centroids[i] != c)
			{
				dist = Vector3.SqrMagnitude(c.position - centroids[i].position);

				if (dist < Vector3.SqrMagnitude(c.position - closest.position))
				{
					closest = centroids[i];
				}

			}
		}
		return closest;
	}

	public void BuildPoints(List<string> clusterPoints)
	{
		//StopCoroutine("CreatePoints");
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
        float x, z;
        GameObject clone;
        Vector3 position;
        int ID;
        ClusterPoint point;
		for (int i = 0 ; i < clusterPoints.Count; i++)
		{
			x = float.Parse(clusterPoints[i].Split(',')[1]);
			z = float.Parse(clusterPoints[i].Split(',')[2]);

			clone = Instantiate(clusterPrefab) as GameObject;
			clone.transform.SetParent(transform);
			position = new Vector3(x, 0, z);
			clone.transform.localScale = new Vector3(pointScale, .02f, pointScale);
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

        Debug.Log("childs" + transform.childCount);

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
