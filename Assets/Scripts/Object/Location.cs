using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{

	public string name;
	public string id;
	public float width;
	public float height;
	public int orientation;

	List<Beacon> beacons;

	List<Wall> walls;

	public Location(string name, string id, float width, float height, int orientation, List<Beacon> beacons, List<Wall> walls)
	{
		this.name = name ;
		this.id = id;
		this.width = width;
		this.height = height;
		this.orientation = orientation;
		this.beacons = beacons;
		this.walls = walls;
	}

	public List<Beacon> GetBeacons()
	{
		return beacons;
	}

	public List<Wall> GetWalls()
	{
		return walls;
	}

	public string ToString()
	{
		return string.Format("{0} + {1} + {2} + {3} + {4} + {5}", name, id, width, height, orientation, beacons, walls);
	}

}
