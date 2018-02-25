using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCentroid : MonoBehaviour {

	public List<ClusterPoint> points = new List<ClusterPoint>(); 
	private float scale; 
	public Vector3 position; 
	private Centroid mainCentroid; 
	private Vector3 targetPosition; 
	private SpriteRenderer spriteRenderer; 
	public float distance; 
	public Color random; 
	private float alpha = .1f; 
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>(); 
		random = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1f); 
		scale = Random.Range(.5f, 2f); 
		Color c = mainCentroid.Material.GetColor("_Color"); 
		float strength = .3f;
		spriteRenderer.color = new Color(c.r - strength, c.g - strength, c.b - strength, alpha ); 
		//transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));
		//transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f);
		position = transform.position; 
		distance = Vector3.Distance(mainCentroid.position, transform.position); 
	}



	public void AddPoints(List<ClusterPoint> p)
	{
		this.points = p; 
	}

	public void SetMainCentroid(Centroid c)
	{
		this.mainCentroid = c;
		distance = Vector3.Distance(mainCentroid.position, transform.position); 
	}

	public void RemovePoints()
	{
		for(int i = 0; i < points.Count; i++)
		{
			Destroy(points[i].transform.gameObject); 
		}
		points.Clear(); 
	}

	float magnitude, distanceMagnitude;
	MiniCentroid closest;
	public MiniCentroid GetClosestMiniCentroid(ClusterPoint cp)
	{
		MiniCentroid c;
		closest = null;
		if(cp.miniCentroid == null)
		{
			cp.SetMiniCentroid(this); 
		}
		distanceMagnitude = Vector3.SqrMagnitude(cp.position - cp.miniCentroid.position);
		for (int i = 0; i < mainCentroid.miniCentroid.Count; i++)
		{
			c = mainCentroid.miniCentroid[i];
			magnitude = Vector3.SqrMagnitude(cp.position - c.position);
			if (magnitude < distanceMagnitude)
			{
				closest = c;
				distanceMagnitude = magnitude;
			}
		}

		return closest;
	}

	public void CalculatePoint()
	{
		ClusterPoint cp;
		for (int i = 0; i < points.Count; i++)
		{
			cp = points[i];

			closest = GetClosestMiniCentroid(cp);

			if (closest == null) { continue; }

			cp.miniCentroid.points.Remove(cp);

			closest.points.Add(cp);

			cp.SetMiniCentroid(closest);
		}

		CalculateCenter();
	}

	float x, z;
	public void CalculateCenter()
	{
		if (points.Count == 0) { 
			return; 
		}
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

	public void SetColor(Color c)
	{

		float strength = .1f;
		spriteRenderer.color = new Color(c.r - strength, c.g - strength, c.b - strength, alpha );
		//transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));
		//transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r - strength, c.g - strength, c.b - strength, 1 ));

	}
	public List<Vector2> GetActivePoints()
	{
		List<Vector2> activePoints = new List<Vector2>(); 
		for(int i = 0;i < points.Count; i++)
		{

			if(points[i].Active)
			{
				
				Vector2 p = new Vector2(points[i].transform.position.x, points[i].transform.position.z); 
				activePoints.Add(p); 
			}

			points[i].FilterPoint(); 
			

		}

		return activePoints; 
	}

	public void Refresh()
	{
		List<ClusterPoint> temp = points; 
		points.Clear(); 
		foreach(ClusterPoint p in temp)
		{
			if(p.Active)
			{
				points.Add(p); 
			}
		}
	}


	public void Merge(MiniCentroid mini)
	{
		if(this == mini) return; 

		for(int i = 0;i < mini.points.Count; i++)
		{
			points.Add(mini.points[i]); 
			//mini.points[i].Hide(); 
		}

		mainCentroid.miniCentroid.Remove(mini); 
		Destroy(mini.transform.gameObject); 
	}
}
