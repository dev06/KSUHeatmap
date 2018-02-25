using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneration : MonoBehaviour {

	public List<MiniCentroid> miniCentroid; 
	private MiniCentroid first; 
	public PathGeneration(List<MiniCentroid> miniCentroid)
	{
		this.miniCentroid = miniCentroid; 

	}
	MiniCentroid next = null; 
	List<MiniCentroid> mapped = new List<MiniCentroid>(); 

	public void GeneratePath(Color c)
	{
		for(int i =0 ; i < miniCentroid.Count; i++)
		{
			if(next == null)
			{
				next = GetStarterCentroid();
				first = next; 
			}

			MiniCentroid closest = GetClosestCentroid(next); 

			if(!InMapped(closest))
			{
				DrawPath(closest.position,c); 
				mapped.Add(next); 
				next = closest; 			
			}
			else
			{
				DrawPath(first.position, c); 
			}	
		}


	}

	private void DrawPath(Vector3 position, Color c)
	{
		LineRenderer renderer = next.transform.GetComponent<LineRenderer>(); 
		renderer.startColor = c; 
		renderer.endColor = c; 

		renderer.SetPosition(0, next.transform.position + (Vector3.up * 2)); 
		renderer.SetPosition(1, position + (Vector3.up * 2)); 

	}

	private bool InMapped(MiniCentroid c)
	{
		for(int i = 0;i < mapped.Count; i++)
		{
			if(mapped[i] == c)
			{
				return true; 
			}
		}

		return false; 
	}

	private MiniCentroid GetStarterCentroid()
	{
		MiniCentroid c = null; 
		float z = 10000; 
		for(int i = 0;i < miniCentroid.Count; i++)
		{
			MiniCentroid toCheck = miniCentroid[i]; 
			if(toCheck.position.z < z)
			{
				z = toCheck.position.z; 
				c = toCheck; 
			}
		}

		if(c == null)
		{
			c = miniCentroid[Random.Range(0, miniCentroid.Count)]; 
		}

		return c; 
	}

	private MiniCentroid GetClosestCentroid(MiniCentroid c)
	{
		float closest = 10000; 
		MiniCentroid closestCentroid = first; 

		for(int i = 0;i < miniCentroid.Count; i++)
		{

			MiniCentroid toCheck = miniCentroid[i]; 
			if(toCheck == c) continue; 
			if(InMapped(toCheck)) continue;
			float distance = Vector3.Distance(c.position, toCheck.position); 
			if(distance < closest)
			{	
				closest = distance; 
				closestCentroid = toCheck; 
			}

		}		

		return closestCentroid; 
	}

}