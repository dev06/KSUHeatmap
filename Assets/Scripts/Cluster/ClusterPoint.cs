﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterPoint : MonoBehaviour {



	public int ID;
	public Vector3 position;
	public Centroid centroid;
	public float distance;
	private Material material;


	void Start ()
	{

	}

	public void Init() {
		GetComponent<Renderer>().material = material;
		position = transform.position;
	}


	void Update () {

	}

	public void SetMaterial(Material mat)
	{
		this.material = mat;
		GetComponent<Renderer>().material = material;
	}

	public void SetCentroid(Centroid c)
	{
		this.centroid = c;
		distance = Vector3.Distance(position, centroid.position);
	}

}
