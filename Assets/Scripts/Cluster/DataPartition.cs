using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPartition : MonoBehaviour 
{

	private List<Vector2> datapoints; 

	private Cluster cluster; 

	public DataPartition(List<Vector2> datapoints)
	{
		cluster = FindObjectOfType<Cluster>(); 
		this.datapoints = datapoints; 

		cluster.CreateVectorPoints(datapoints); 
	}


	public List<Vector2> FilteredPoints()
	{
		List<Vector2> newPoints = new List<Vector2>(); 
		return newPoints; 
	}






	

}
