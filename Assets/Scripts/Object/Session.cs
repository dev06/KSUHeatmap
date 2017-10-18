using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Session
{

	public List<DataPoint> datapoints;

	public Session()
	{

	}


	public Session(List<DataPoint> datapoints)
	{
		this.datapoints = datapoints;
	}


	public void SetDatapoints(List<DataPoint> datapoints)
	{
		this.datapoints = datapoints;
	}

}