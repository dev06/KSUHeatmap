using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour {


	private Location location; 
	public List<ClusterPoint> points; 
	private float scale = .2f; 


	public void GenerateMesh(Location location, List<ClusterPoint> points)
	{
		this.location = location; 
		this.points = points; 
		StopCoroutine("IGenerateMesh"); 
		StartCoroutine("IGenerateMesh"); 
	}

	private IEnumerator IGenerateMesh()
	{

		Color c= new Color(.7f, 1f, .7f, 1f); 
		Color c2 = new Color(1f, .7f,.7f, 1f); 
		int index = 0; 
		if(location == null)
		{
			Debug.LogError("Location is null"); 
			yield return null; 		
		}

		for(int z = 0;z < location.height / scale; z+=1)
		{
			for(int x = 0; x < location.width / scale; x+=1)
			{
				Vector3 pointToCheck = new Vector3(x * scale, 0, z* scale); 

				if(!HasNearByPoints(pointToCheck))
				{
					
					GameObject clone = (GameObject)Instantiate(AppResources.Unwalkable);
					clone.transform.localScale = new Vector3(scale, scale, 1f); 
					clone.transform.SetParent(transform);  
					clone.transform.position = pointToCheck; 
					clone.transform.rotation = Quaternion.Euler(new Vector3(90, 0,0)); 
					SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
					sr.color = c2; 
					
				}
				index++; 
			}

			if(index % 5 == 0)
			{
				index = 0; 
				yield return null; 
			}
		}

	//	yield return null; 


	}

	public bool HasNearByPoints(Vector3 position)
	{
		for(int i = 0;i < points.Count; i++)
		{
			float dist = Vector3.Distance(points[i].position, position); 
			if(dist < .7f)
			{
				return true; 
			}
		}

		return false; 
	}
}
