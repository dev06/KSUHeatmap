using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centroid : MonoBehaviour {

	public List<ClusterPoint> points;

	public int ID;
	public Vector3 position;
	private Material material;

	public void Init(int ID)
	{
		this.ID = ID;
		SetMaterial(ID == 0 ? SystemResources.red : SystemResources.blue);
		position = transform.position;
	}


	public void SetMaterial(Material mat)
	{
		this.material = mat;
		transform.GetChild(0).GetComponent<Renderer>().material = material;
		transform.GetChild(1).GetComponent<Renderer>().material = material;
	}

	public Material GetMaterial()
	{
		return material;
	}
}
