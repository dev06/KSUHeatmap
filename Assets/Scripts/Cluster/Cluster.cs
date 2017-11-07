using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour {

	public GameObject clusterPrefab;
	public GameObject centroidPrefab;
	public List<ClusterPoint> redPoints;
	public List<ClusterPoint> bluePoints;
	public List<Centroid> centroids;
	void Start () {

		// for (int i = 0; i < 10; i++)
		// {
		// 	GameObject clone = Instantiate(clusterPrefab) as GameObject;
		// 	float x = Random.Range(-4, 5);
		// 	float z = Random.Range(-4, 5);
		// 	Vector3 position = new Vector3(x, 0, z);
		// 	clone.transform.position = position;
		// 	int ID = Random.Range(0, 2);
		// 	ClusterPoint point = clone.GetComponent<ClusterPoint>();
		// 	point.ID = ID;
		// 	point.SetMaterial(ID == 0 ? SystemResources.red : SystemResources.blue);
		// 	point.Init();
		// 	if (ID == 0)
		// 	{
		// 		redPoints.Add(point);
		// 	}
		// 	if (ID == 1)
		// 	{
		// 		bluePoints.Add(point);
		// 	}
		// }


		CreateCentroid();
		CreateCentroid();


	}

	public void CreateCentroid()
	{
		GameObject centroidClone = Instantiate(centroidPrefab) as GameObject;
		Centroid centroid = centroidClone.GetComponent<Centroid>();
		int ID = GenerateID();
		centroid.Init(ID);
		centroids.Add(centroid);

	}

	int GenerateID()
	{
		int ID = Random.Range(0, 2);
		do
		{
			ID = Random.Range(0, 2);

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


	public void CalculateCentroid(List<ClusterPoint> points, int ID)
	{
		if (points.Count < 1) return;
		float x = 0;
		float y = 0;
		for (int i = 0; i < points.Count; i++)
		{
			ClusterPoint point = points[i];
			x += point.position.x;
			y += point.position.z;
		}

		x /= points.Count;
		y /= points.Count;



		GameObject centroidClone = Instantiate(centroidPrefab) as GameObject;
		centroidClone.transform.position = new Vector3(x, 0, y);
		Centroid centroid = centroidClone.GetComponent<Centroid>();
		centroid.ID = ID;
		centroid.SetMaterial(ID == 0 ? SystemResources.red : SystemResources.blue);
		//centroid.Init();
		centroids.Add(centroid);

		for (int i = 0; i < points.Count; i++)
		{
			ClusterPoint point = points[i];
			point.SetCentroid(centroid);
		}
	}


	public void ReassignCluster()
	{
		for (int i = 0; i < redPoints.Count; i++)
		{
			ClusterPoint currentPoint = redPoints[i];

			for (int j = 0; j < centroids.Count; j++)
			{
				Centroid currentCentroid = centroids[j];

				if (currentPoint.ID != currentCentroid.ID)
				{
					float distance = Vector3.Distance(currentPoint.position, currentCentroid.position);

					if (distance < currentPoint.distance)
					{
						currentPoint.SetCentroid(currentCentroid);

						currentPoint.SetMaterial(currentCentroid.GetMaterial());

						redPoints.Remove(currentPoint);


					}
				}
			}
		}
	}


	void Update () {

		if (Input.GetKeyDown(KeyCode.Space))
		{
			ReassignCluster();
			Debug.Log("1");
		}
	}
}
