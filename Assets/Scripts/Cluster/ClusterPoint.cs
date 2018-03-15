using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterPoint : MonoBehaviour {



	public int ID;
	public Vector3 position;
	public Centroid centroid;
	public MiniCentroid miniCentroid; 
	public float distance;
	public float distanceFromMini; 
	private Material material;
	private LineRenderer lineRenderer;
	private Vector3 targetLinePosition;
	private SpriteRenderer sr; 
	private bool active = true; 
	private float alpha = .014f; 

	void Start ()
	{

	}

	public void Init(int ID)
	{
		this.ID = ID;
		SetMaterial(Cluster.GetMaterialByID(ID));
		position = transform.position;
		lineRenderer = GetComponent<LineRenderer>();
		sr = GetComponent<SpriteRenderer>(); 
		transform.rotation = Quaternion.Euler(new Vector3(90,0,0)); 
		lineRenderer.enabled = false; 
		//transform.localScale = new Vector3(1,1,1); 
	}

	void Update()
	{
		if (centroid == null) { return; }
		//targetLinePosition = Vector3.Lerp(lineRenderer.GetPosition(1), centroid.position, Time.deltaTime * 5.0f);
		//lineRenderer.SetPosition(1, targetLinePosition);
		//lineRenderer.material = material;

		
		distance = Vector3.Distance(position, centroid.position);			
		
	}

	public void Hide()
	{
		if(sr == null) sr = GetComponent<SpriteRenderer>(); 
		sr.enabled = false; 
		lineRenderer.enabled = false; 
		active = false; 
	}

	public void Show()
	{
		if(sr == null) sr = GetComponent<SpriteRenderer>(); 
		sr.enabled = true; 
		//lineRenderer.enabled = true;
		active = true; 
	}


	public void SetMaterial(Material mat)
	{
		this.material = mat;
		GetComponent<Renderer>().material = material;
		GetComponent<SpriteRenderer>().color = new Color(mat.color.r, mat.color.g,mat.color.b, alpha); 
	}

	public void SetCentroid(Centroid c)
	{
		this.centroid = c;
		lineRenderer.SetPosition(0, position);
		distance = Vector3.Distance(position, centroid.position);
		GetComponent<LineRenderer>().material = material;
		lineRenderer.SetPosition(1,  centroid.position);
		lineRenderer.material = material;

		SetMaterial(c.GetMaterial());

	}

	public void SetMiniCentroid(MiniCentroid c)
	{
		this.miniCentroid = c; 
		//sr.color = c.random; 
		distanceFromMini = Vector3.Distance(position, miniCentroid.position); 

	}

	public void SetPosition(Vector3 p)
	{
		transform.position = p; 
		position = p; 

		//lineRenderer.SetPosition(0, p); 
	}

	public void Filter()
	{

		if(Vector3.Distance(transform.position, miniCentroid.position) > 1f)
		{
			miniCentroid.points.Remove(this); 
			Hide(); 
		}
	//	return null; 
	}

	public bool Active
	{
		set{
			active = value; 

			if(active == false)
			{
				Hide(); 
			}
		}
		get
		{
			return active; 
		}
	}

	public void FilterPoint()
	{
		if(miniCentroid == null)
		{
			//Debug.Log("MiniCentroid is null"); 
			Hide(); 
			return; 
		}


		for(int i = 0;i < miniCentroid.points.Count; i++)
		{
			ClusterPoint p = miniCentroid.points[i]; 
			if(!p.Active || !Active) continue; 
			if(p == this) continue; 


			float dist = Vector3.Distance(p.transform.position, transform.position); 
			if(dist > 2f)
			{
				miniCentroid.points.Remove(this); 
				Hide(); 
			}

		}
	}

}
