using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrewTesting : MonoBehaviour {

	// Use this for initialization
	void Start()
	{

		List<string> walls = new List<string>();
		walls.Add("0.432, 0.000");
		walls.Add("0.432, 1.618");
		walls.Add("0.000, 1.618");
		walls.Add("0.000, 9.179");
		walls.Add("0.432, 9.179");
		walls.Add("0.432, 9.462");
		walls.Add("8.989, 9.462");
		walls.Add("8.989, 0.000");
		LocationBuilder.BuildWalls(walls);

		List<string> beacons = new List<string>();
		beacons.Add("beacon_1,  3.137,  0.000,  000");
		beacons.Add("beacon_2,  1.091,  9.462,  180");
		beacons.Add("beacon_3,  8.989,  1.946,  270");
		beacons.Add("beacon_4,  8.989,  7.290,  270");
		beacons.Add("beacon_5,  3.037,  9.462,  180");
		beacons.Add("beacon_6,  0.000,  7.477,  090");
		beacons.Add("beacon_7,  0.000,  5.632,  090");
		beacons.Add("beacon_8,  0.000,  1.838,  090");
		LocationBuilder.BuildBeacons(beacons);

		List<string> datapoints = new List<string>();
		datapoints.Add("0,0,0,0,clo1");
		datapoints.Add("5,1,0,90,clo1,clo2,clo3,clo4");
		datapoints.Add("10,2,5,15,clo1,clo3,clo5");
		datapoints.Add("20,5,5,90,clo1,clo2,clo4");
		datapoints.Add("30,6,2,270,clo3,clo5,clo7");
		datapoints.Add("40,4,3,15,clo5,clo7,clo9");
		datapoints.Add("50,7,5,15,clo0");
		DataBuilder.INSTANCE.BuildData(datapoints);


	}
}
