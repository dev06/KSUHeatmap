using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centroid : MonoBehaviour {


	public List<ClusterPoint> points;
	public List<MiniCentroid> miniCentroid = new List<MiniCentroid>(); 
	public int ID;
	public Vector3 position;
	public float STD;
	private Material material;
	private Vector3 targetPosition;
	private Cluster cluster; 
	private int miniCentroidCount; 
	private int pointsInMiniCentroid = 5; // each mini centroid will atleast have that x many points. 
	PathGeneration path;



	public void Init(int ID)
	{
		this.ID = ID;
		SetMaterial(Cluster.GetMaterialByID(ID));
		position = transform.position;
		cluster = FindObjectOfType<Cluster>(); 
	}

	private void CreateMiniCentroid()
	{

		for(int i =0 ; i< miniCentroidCount; i++)
		{
			GameObject clone = Instantiate(cluster.miniCentroid) as GameObject; 
			clone.transform.SetParent(transform); 
			clone.transform.position = transform.position; 
			MiniCentroid mini = clone.GetComponent<MiniCentroid>(); 
			mini.SetMainCentroid(this); 
			miniCentroid.Add(mini); 
		}
		path = new PathGeneration(miniCentroid); 

	}


	public void SetMaterial(Material mat)
	{
		this.material = mat;
		transform.GetChild(0).GetComponent<Renderer>().material = material;
		transform.GetChild(1).GetComponent<Renderer>().material = material;
		Color c = material.GetColor("_Color");
		float strength = .1f;
		//transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));
	//	transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));

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
	bool b; 


	public void AssignMinicentroidPoints()
	{
		StopCoroutine("IAssignMiniCentroidPoints"); 
		StartCoroutine("IAssignMiniCentroidPoints"); 
	}
	private IEnumerator IAssignMiniCentroidPoints()
	{
		int counter = 0; 

		while(counter < 100)
		{

			if(!b)
			{
				miniCentroidCount = points.Count / pointsInMiniCentroid; 
				CreateMiniCentroid(); 

				int startIndex = 0; 
				int endIndex = 0; 
				if(miniCentroid.Count > points.Count)
				{
					miniCentroidCount = points.Count; 
				}

				for(int i = 0; i < miniCentroidCount; i++)
				{
					List<ClusterPoint> toAdd= new List<ClusterPoint>(); 
					endIndex = startIndex + points.Count  / miniCentroidCount; 
					for(int ia = startIndex; ia < endIndex; ia++)
					{
						toAdd.Add(points[ia]); 

					}

					miniCentroid[i].AddPoints(toAdd); 
					startIndex = endIndex; 
				}


				b = true; 
			}

			for(int i =0;i < miniCentroid.Count; i++)
			{
				miniCentroid[i].CalculatePoint(); 
			}

			counter++; 

			yield return null; 
		}
	}

	void Update()
	{
		position = transform.position; 
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f);
	}

	Centroid closest = null;
	public void CalculatePoint()
	{
		ClusterPoint cp;
		for (int i = 0; i < points.Count; i++)
		{
			cp = points[i];

			closest = GetClosestCentroid(closest, cp);

			if (closest == null) { continue; }

			cp.centroid.points.Remove(cp);

			closest.points.Add(cp);

			cp.SetCentroid(closest);
		}

		CalculateCenter();
	}

	float magnitude, distanceMagnitude;
	public Centroid GetClosestCentroid(Centroid closest, ClusterPoint cp)
	{
		Centroid c;
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

	public Material GetMaterial()
	{
		return material;
	}


	private List<float> pointDistances = new List<float>(); 
	public void FilterPoints()
	{	
		pointDistances.Clear(); 
		for(int i = 0;i < miniCentroid.Count; i++)
		{

			pointDistances.Add(miniCentroid[i].distance); 
		}

		pointDistances.Sort(); 

		float q1 = 0; 
		float q3 = 0; 
		float q1Sum = 0, q1Avg = 0; 
		float q3Sum = 0, q3Avg = 0; 
		for(int i = 0; i < pointDistances.Count / 2; i++)
		{
			q1Sum+=pointDistances[i]; 
		}

		q1Avg = q1Sum / (pointDistances.Count / 2); 

		for(int i = pointDistances.Count / 2; i < pointDistances.Count; i++)
		{
			q3Sum+=pointDistances[i]; 
		}

		q3Avg = q3Sum / (pointDistances.Count / 2); 

		float diff = (q3Avg - q1Avg) * 1.9f;

		float max = q3Avg + diff; 

		float min = q1Avg - diff; 

		// Debug.Log("Max " + max + " Min " + min); 
		// for(int i =0 ;i < pointDistances.Count; i++)
		// {
		// 	Debug.Log(pointDistances[i]); 
		// } 


		for(int i = 0;i < miniCentroid.Count; i++)
		{

			// if(Vector3.Distance(transform.position, points[i].position) > 4F)
			// {
			// 	GameObject obj = points[i].transform.gameObject; 
			// 	points[i].SetPosition(transform.position); 
			// 	// points.Remove(points[i]); 
			// 	// Destroy(obj); 
			// }
			if(miniCentroid[i].distance < min || miniCentroid[i].distance > max)
			{
				GameObject obj = miniCentroid[i].transform.gameObject; 
				for(int ia = 0;ia < miniCentroid[i].points.Count; ia++)
				{
					miniCentroid[i].points[ia].Hide();
				}
				miniCentroid.Remove(miniCentroid[i]); 
				Destroy(obj); 
			}
		} 
	}

	public void FilterMiniCentroids()
	{
		for(int i = 0;i < points.Count; i++)
		{
			points[i].FilterPoint(); 
		}

		for(int i =0 ;i < miniCentroid.Count; i++)
		{
			MiniCentroid current = miniCentroid[i]; 
			MiniCentroid closest = GetClosestMiniCentroid(current); 
			if(closest == null) continue; 
			float distance = Vector3.Distance(current.position, closest.position); 
			if(distance > .55f)
			{
				GameObject obj = current.transform.gameObject; 

				for(int ia =0 ;ia < current.points.Count; ia++)
				{
					current.points[ia].Hide(); 
				}

				miniCentroid[i].Refresh(); 

				miniCentroid.Remove(current); 
				Destroy(obj); 
			}

		}
	}

	private MiniCentroid GetClosestMiniCentroid(MiniCentroid c)
	{
		float closest = 10000; 
		MiniCentroid closestCentroid = null; 

		for(int i = 0;i < miniCentroid.Count; i++)
		{

			MiniCentroid toCheck = miniCentroid[i]; 
			if(toCheck == c) continue; 
			float distance = Vector3.Distance(c.position, toCheck.position); 
			if(distance < closest)
			{	
				closest = distance; 
				closestCentroid = toCheck; 
			}

		}		

		return closestCentroid; 
	}


	List<float> pointMag = new List<float>(); 
	public static bool test; 
	public void CalculateSTD()
	{
		for(int i =0 ;i < points.Count; i++)
		{
			pointMag.Add(points[i].position.magnitude); 
		}

		float mean = 0; 

		for(int i = 0;i < pointMag.Count; i++)
		{
			mean+=pointMag[i];
		} 

		mean = mean / pointMag.Count; 


		float secondSum = 0; 

		for(int i =0 ;i < pointMag.Count; i++)
		{
			secondSum += Mathf.Pow(pointMag[i] - mean, 2); 
		}

		float mult = secondSum * (1f/ pointMag.Count); 

		STD = Mathf.Sqrt(mult); 

		// if(STD > 5)
		// {
		// 	for(int i = 0;i < points.Count; i++)
		// 	{
		// 		GameObject obj = points[i].transform.gameObject; 
		// 		points.Remove(points[i]); 
		// 		//Destroy(obj); 
		// 	}
		// }
	}

	public void GeneratePath()
	{
		path.GeneratePath(material.GetColor("_Color")); 

	}

	public Material Material
	{
		get{
			return material; 
		}
	}
}
