using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint {

	public Vector2 location;
	public Orientation orientation;
	public float accuracy;

	public DataPoint(Vector2 location, float accuracy, Orientation orientation)
	{
		this.location = location;
		this.accuracy = accuracy;
		this.orientation = orientation;
	}
}

public enum Orientation
{
	NONE,
	LOW,
	MEDIUM,
	HIGH,
}
