using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unwalkable : MonoBehaviour {


	private Transform parent; 
	private Color color; 
	private float scale = 1f; 

	private Location location; 
	public List<ClusterPoint> points; 

	public Unwalkable()
	{	
		parent = GameObject.Find("UnwalkableNodes").transform; 
		color = new Color(.5f, 1f, .5f, 1f); 
	}

	public void GenerateUnwalkableMesh(Location location, List<ClusterPoint> points)
	{
		this.location = location; 
		this.points = points; 
		//StopCoroutine("IGenerateMesh"); 
		MeshCreator mesh = FindObjectOfType<MeshCreator>(); 
		mesh.GenerateMesh(location, points); 
	}

	// private IEnumerator IGenerateMesh()
	// {

	// 	if(location == null)
	// 	{
	// 		Debug.LogError("Location is null"); 
	// 		yield return null; 		
	// 	}

	// 	for(int z = 0;z < location.height / scale; z+=1)
	// 	{
	// 		for(int x = 0; x < location.width / scale; x+=1)
	// 		{
	// 			Vector3 pointToCheck = new Vector3(x * scale, 0, z* scale); 

	// 			if(!HasNearByPoints(pointToCheck))
	// 			{
	// 				GameObject clone = (GameObject)Instantiate(AppResources.Unwalkable);
	// 				clone.transform.localScale = new Vector3(scale, scale, 1f); 
	// 				clone.transform.SetParent(parent);  
	// 				clone.transform.position = pointToCheck; 
	// 				clone.transform.rotation = Quaternion.Euler(new Vector3(90, 0,0)); 
	// 			}
	// 			yield return null; 
	// 		}
	// 	}


	// }



	public void Clear()
	{
		foreach(Transform t in parent)
		{
			Destroy(t.gameObject); 
		}
	}
}
