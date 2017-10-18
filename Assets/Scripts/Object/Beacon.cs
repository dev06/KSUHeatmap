using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon {


	public string id;
	public float x;
	public float y;
	public int orientation;
	private Vector2 position;

	public Beacon(string id, float x, float y, int orientation)
	{
		this.x = x;
		this.y = y;
		this.orientation = orientation;
		this.position = new Vector2(x, y);
	}


	public Vector2 GetPosition()
	{
		return position;
	}

}
