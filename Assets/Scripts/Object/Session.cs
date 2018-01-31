using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Session
{

	//	public List<KSUHeatmap.DataPoint> datapoints;
	public List<string> datapoints;

	public Session()
	{

	}


	// public Session(List<KSUHeatmap.DataPoint> datapoints)
	// {
	// 	this.datapoints = datapoints;
	// }


	public Session(List<string> datapoints)
	{
		this.datapoints = datapoints;
	}


	public void SetDatapoints(List<string>  datapoints)
	{
		this.datapoints = datapoints;
	}

}