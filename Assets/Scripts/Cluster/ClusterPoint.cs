using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterPoint : MonoBehaviour {



	public int ID;
	public Vector3 position;
	public Centroid centroid;
	public float distance;
	private Material material;
	private LineRenderer lineRenderer;
	private Vector3 targetLinePosition;

	void Start ()
	{

	}

	public void Init(int ID)
	{
		this.ID = ID;
		SetMaterial(Cluster.GetMaterialByID(ID));
		position = transform.position;
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update()
	{
		if (centroid == null) { return; }
		targetLinePosition = Vector3.Lerp(lineRenderer.GetPosition(1), centroid.position, Time.deltaTime * 5.0f);
		lineRenderer.SetPosition(1, targetLinePosition);
		lineRenderer.material = material;
	}




	public void SetMaterial(Material mat)
	{
		this.material = mat;
		GetComponent<Renderer>().material = material;
	}

	public void SetCentroid(Centroid c)
	{
		this.centroid = c;
		lineRenderer.SetPosition(0, position);
		distance = Vector3.Distance(position, centroid.position);
		GetComponent<LineRenderer>().material = material;
		lineRenderer.SetPosition(1,  centroid.position);
		SetMaterial(c.GetMaterial());

	}

}
