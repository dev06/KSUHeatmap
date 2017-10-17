using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeatmapGrid : MonoBehaviour {

	public GameObject cellPrefab;
	public Transform parent;
	private float width;
	private float height;
	public HeatmapCell[,] cells;
	int cellSize = 50;

	List<Vector2> positions = new List < Vector2>();

	void Start ()
	{
		parent = GameObject.Find("Heatmap").transform;
		InitGrid();


	}

	private void InitGrid()
	{

		width = parent.GetComponent<RectTransform>().rect.width / cellSize;
		height = parent.GetComponent<RectTransform>().rect.height / cellSize;
		Debug.Log( parent.GetComponent<RectTransform>().rect.height);
		cells = new HeatmapCell[(int)Mathf.Round(width), (int)Mathf.Round(height)];
		for (int y = 0; y < cells.GetLength(1); y++)
		{
			for (int x = 0; x < cells.GetLength(0); x++)
			{
				GameObject cell = Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity);

				cell.transform.SetParent(parent);
				RectTransform rt = cell.GetComponent<RectTransform>();
				rt.sizeDelta = new Vector3(cellSize, cellSize, 1);
				rt.anchoredPosition = new Vector2(x * rt.sizeDelta.x - width / 2 * rt.sizeDelta.x + rt.sizeDelta.x / 2, y * rt.sizeDelta.y - height / 2 * rt.sizeDelta.y + rt.sizeDelta.y / 2);
				rt.localScale = new Vector3(1, 1, 1);
				cells[x, y] = new HeatmapCell(x, y, cell.GetComponent<Image>());
			}
		}



	}
	int xx, yy;
	float xVel;
	float yVel;
	float dirX = 80;
	float dirY = 80;
	float time;
	float acc;
	public RectTransform obj;
	void Update ()
	{
		//acc += Time.deltaTime;
		time += Time.deltaTime;
		xVel += Time.deltaTime * dirX;
		yVel += Time.deltaTime * dirY ;
		obj.anchoredPosition = new Vector3(xVel, yVel, 1);
		if (obj.anchoredPosition.x + obj.sizeDelta.x > parent.GetComponent<RectTransform>().rect.width || obj.anchoredPosition.x < 0)
		{
			dirX *= -1;

		}
		if (obj.anchoredPosition.y + obj.sizeDelta.y > parent.GetComponent<RectTransform>().rect.height || obj.anchoredPosition.y < 0)
		{

			dirY *= -1;
		}

		if (time > 0.5f)
		{

			xx = (int)obj.anchoredPosition.x / cellSize;
			yy = (int)obj.anchoredPosition.y / cellSize;

			for (int y = 0; y < cells.GetLength(1); y++)
			{
				for (int x = 0; x < cells.GetLength(0); x++)
				{
					float distance = GetDistance(x, y, xx, yy);
					cells[x , y].heatValue += (1.0f / (distance * distance));
				}
			}

			time = 0;
		}


		// if (Input.GetKey(KeyCode.W))
		// {
		// 	yy++;
		// }
		// if (Input.GetKey(KeyCode.S))
		// {
		// 	yy--;
		// }
		// if (Input.GetKey(KeyCode.A))
		// {
		// 	xx--;
		// }
		// if (Input.GetKey(KeyCode.D))
		// {
		// 	xx++;
		// }




		for (int y = 0; y < cells.GetLength(1); y++)
		{
			for (int x = 0; x < cells.GetLength(0); x++)
			{
				cells[x, y].SetColor(Color.Lerp(new Color(1, 1, 0, 1), new Color(1, 0, 0, 1),  	cells[x, y].heatValue));

			}
		}
	}

	void AdjustHeatValue()
	{
		for (int i = 0 ; i < positions.Count; i++)
		{
			for (int a = 0; a < 1; a++)
			{
				cells[(int)positions[i].x, (int)positions[i].y].heatValue += GetAverage((int)positions[i].x, (int)positions[i].y);
			}

			cells[0, 0].heatValue = .1f;
		}
	}

	float GetAverage(int xx, int yy)
	{
		float sum = 0;
		float avg = 0;
		int n = 0;
		for (int y = yy - 1; y <= yy + 1; y++)
		{
			for (int x = xx - 1; x <= xx + 1; x++ )
			{
				if (x == xx && y == yy)
				{
					continue;
				}

				if (x < 0 || x > cells.GetLength(0) - 1 || y < 0 || y > cells.GetLength(1) - 1) { continue; }

				sum += cells[x, y].heatValue;
				n++;

			}
		}
		avg = sum / n;

		return avg;
	}


	float GetDistance(float x1, float y1, float x2, float y2)
	{
		float sqX = (x2 - x1) * (x2 - x1);
		float sqY = (y2 - y1) * (y2 - y1);
		float d = Mathf.Sqrt(sqX + sqY);

		return d;

	}


	void SetAvg(int xx, int yy)
	{
		for (int y = yy - 4; y <= yy + 4; y++)
		{
			for (int x = xx - 4; x <= xx + 4; x++ )
			{
				if (x == xx && y == yy)
				{
					continue;
				}

				if (x < 0 || x > cells.GetLength(0) - 4 || y < 0 || y > cells.GetLength(1) - 4) { continue; }

				cells[x, y].heatValue += cells[xx, yy].heatValue / 1000.0f ;

			}
		}
	}


}
