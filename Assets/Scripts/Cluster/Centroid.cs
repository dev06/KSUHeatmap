using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centroid : MonoBehaviour {

	public List<ClusterPoint> points;

	public int ID;
	public Vector3 position;
	private Material material;
	private Vector3 targetPosition;

	public void Init(int ID)
	{
		this.ID = ID;
		SetMaterial(Cluster.GetMaterialByID(ID));
		position = transform.position;
	}


	public void SetMaterial(Material mat)
	{
		this.material = mat;
		transform.GetChild(0).GetComponent<Renderer>().material = material;
		transform.GetChild(1).GetComponent<Renderer>().material = material;
		Color c = material.GetColor("_Color");
		float strength = .1f;
		transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));
		transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));

	}

    float x, z;
	public void CalculateCenter()
	{
		if (points.Count == 0) { return; }
		x = 0;
		z = 0;
		for (int i = 0 ; i < points.Count; i++)
		{
			x += points[i].position.x;
			z += points[i].position.z;
		}

		x /= points.Count;
		z /= points.Count;

		position.x = x;
        position.y = 0;
        position.z = z;

		targetPosition = position;
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f);
	}

    ClusterPoint cp;
    Centroid closest;
	public void CalculatePoint()
	{
		for (int i = 0; i < points.Count; i++)
		{
			cp = points[i];

			closest = GetClosestCentroid(cp);

			if (closest == null) { continue; }

			cp.centroid.points.Remove(cp);

			closest.points.Add(cp);

			cp.SetCentroid(closest);


			// for (int j = 0; j < Cluster.Instance.centroids.Count; j++)
			// {
			// 	Centroid c = Cluster.Instance.centroids[j];

			// 	if (c.ID == cp.centroid.ID) { continue; }

			// 	float distance = Vector3.Distance(cp.position, c.position);

			// 	if (distance < cp.distance)
			// 	{

			// 		cp.centroid.points.Remove(cp);

			// 		c.points.Add(cp);

			// 		cp.SetCentroid(c);
			// 	}
			// }
		}

		CalculateCenter();
	}

    float magnitude, distanceMagnitude;
    Centroid c;
	public Centroid GetClosestCentroid(ClusterPoint cp)
	{
		closest = null;
		distanceMagnitude = Vector3.SqrMagnitude(cp.position - cp.centroid.position);
		for (int i = 0; i < Cluster.Instance.centroids.Count; i++)
		{
			c = Cluster.Instance.centroids[i];
			magnitude = Vector3.SqrMagnitude(cp.position - c.position);
			if (magnitude < distanceMagnitude)
			{
				closest = c;
				distanceMagnitude = magnitude;
			}
		}

		return closest;
	}

	public void MergeWith(Centroid c)
	{
		c.points.AddRange(this.points);
		for (int i = 0; i < c.points.Count; i++)
		{
			c.points[i].SetCentroid(c);
		}
		c.CalculateCenter();
		c.CalculatePoint();
	}

	public Material GetMaterial()
	{
		return material;
	}
}
