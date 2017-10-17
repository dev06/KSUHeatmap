using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatmapCell
{

	public int x, y;
	public Image image;
	public float heatValue;
	public HeatmapCell(int x, int y, Image image)
	{
		this.x = x;
		this.y = y;
		this.image = image;
	}
	public HeatmapCell(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void SetColor(Color c) {
		image.color = c;
	}

	public void SetHeatValue(float v)
	{
		heatValue = v;
	}

}

