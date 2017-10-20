using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint {

	public string time;
	public Vector2 position;
	public int orientation;
	public List<string> closestBeaconID;

	public DataPoint(string time, Vector2 position, int orientation, List<string> closestBeaconID)
	{
		this.time = time;
		this.position = position;
		this.orientation = orientation;
		this.closestBeaconID = closestBeaconID;
	}
}
