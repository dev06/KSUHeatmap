using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Heat2d : MonoBehaviour {

	// Use this for initialization
	public Text mapWidthText;
	public Text mapHeightText;
	public Text mapPositionText;
	public Text progresText;
	public Image img;

	public Transform bot;
	private Texture2D texture;
	private int xx, yy;
	private HeatmapCell[,] cells;
	private List<KSUHeatmap.DataPoint> allPoints = new List<KSUHeatmap.DataPoint>(); //list of all the points in the file
	private List<KSUHeatmap.DataPoint> viewPoints = new List<KSUHeatmap.DataPoint>(); //list of all points displayed on screen
	private int pointIndex;
	private int viewingDataChunkIndex = 50;
	private int mapWidth;
	private int mapHeight = 320;
	private bool shouldReset;
	private float resetTimer;
	private KeyCode resetHeatMap = KeyCode.R;


	void Start () {
		mapWidth = (int)((mapHeight * DataParser.Instance.width) / DataParser.Instance.height);

		// Init();
		// allPoints = DataParser.Instance.dataPoints;
		// StartCoroutine("Heat");
		// mapWidthText.text = "Width: " + DataParser.Instance.width + " meters (" + mapWidth + "px)";
		// mapHeightText.text = "Height: " + DataParser.Instance.height + " meters (" + mapHeight + "px)";
	}

	// IEnumerator Heat()
	// {
	// 	ResetTexture();
	// 	for (int i = 0; i < allPoints.Count; i++)
	// 	{
	// 		int x = (int)(allPoints[i].location.x * mapWidth);
	// 		int y = (int)(allPoints[i].location.y * mapHeight);
	// 		if (x > mapWidth || y > mapHeight) continue;
	// 		if (x < 0 || y < 0)continue;
	// 		try
	// 		{
	// 			cells[x, y].heatValue += .5f;
	// 			mapPositionText.text = "Position: " + x.ToString() + "," + y.ToString();
	// 			progresText.text = "Progress: " + (Mathf.Round(((float)i / (float)allPoints.Count) * 100) / 100f) * 100f + "%";

	// 		} catch (System.Exception e)
	// 		{

	// 		}
	// 		yield return null;
	// 	}

	// 	shouldReset = true;
	// }
	void Init()
	{
		float width = DataParser.Instance.width;
		float height = DataParser.Instance.height;
		texture = new Texture2D(mapWidth, mapHeight);
		cells = new HeatmapCell[texture.width, texture.height];
		for (int y = 0; y < cells.GetLength(1); y++)
		{
			for (int x = 0; x < cells.GetLength(0); x++)
			{
				cells[x, y] = new HeatmapCell(x, y);
			}

		}
		GetComponent<SpriteRenderer>().sprite =  Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);;
	}


// Update is called once per frame
	void Update () {
		if (texture == null) return;
		if (shouldReset)
		{
			resetTimer += Time.deltaTime;
			if (resetTimer > 4.0f)
			{
				StartCoroutine("Heat");
				resetTimer = 0;
				shouldReset = false;
			}
		}

		if (Input.GetKeyDown(resetHeatMap))
		{
			StopCoroutine("Heat");
			StartCoroutine("Heat");
		}

		// xx = (int)Mathf.Round((bot.localPosition.x * texture.width));
		// yy = (int)Mathf.Round((bot.localPosition.y * texture.height));

		// if (Input.GetKeyDown(KeyCode.RightArrow))
		// {
		// 	NextHeatStep();
		// }

		// if (Input.GetKeyDown(KeyCode.LeftArrow))
		// {
		// 	PreviousHeatStep();
		// }

		ApplyTexture();

	}

	void NextHeatStep()
	{
		for (int i = pointIndex; i < pointIndex + viewingDataChunkIndex; i++)
		{
			if (i < allPoints.Count)
			{
				viewPoints.Add(allPoints[i]);
			}
		}

		HeatStep(pointIndex);
		pointIndex += viewingDataChunkIndex;
	}

	void HeatStep(int s)
	{
		// for (int i = s; i < viewPoints.Count; i++)
		// {
		// 	int x = (int)(viewPoints[i].location.x * mapWidth);
		// 	int y = (int)(viewPoints[i].location.y * mapHeight);
		// 	cells[x, y].heatValue += .5f;
		// }
	}

	void PreviousHeatStep()
	{
		viewPoints.Clear();
		pointIndex -= viewingDataChunkIndex;
		for (int i = 0; i < pointIndex; i++)
		{
			viewPoints.Add(allPoints[i]);
		}
		ResetTexture();
		HeatStep(0);
	}

	void ApplyTexture()
	{
		for (int y = 0; y < texture.height; y++)
		{
			for (int x = 0; x < texture.width; x++)
			{
				Color color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 0, 0, 1), cells[x, y].heatValue);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}

	void ResetTexture()
	{
		for (int y = 0; y < texture.height; y++)
		{
			for (int x = 0; x < texture.width; x++)
			{
				cells[x, y].heatValue = 0;
			}
		}
	}

	void ClearHeat(int x, int y)
	{
		cells[x, y].heatValue = 0;
	}


	void ApplyHeat(int xx, int yy) {
		int radius = 10;

		for (int y = yy - radius; y <= yy + radius; y++)
		{
			for (int x = xx - radius; x <= xx + radius; x++)
			{
				if (x == xx && y == yy)
				{
					continue;
				}

				if (x < 0 || x > cells.GetLength(0) - radius || y < 0 || y > cells.GetLength(1) - radius) { continue; }


				cells[x, y].heatValue += (1f / (GetDistance(x, y, xx, yy) * GetDistance(x, y, xx, yy))) / radius;
			}
		}
	}


	float GetDistance(float x1, float y1, float x2, float y2)
	{
		float sqX = (x2 - x1) * (x2 - x1);
		float sqY = (y2 - y1) * (y2 - y1);
		float d = Mathf.Sqrt(sqX + sqY);
		Vector2 a = new Vector2(x1, y1);
		Vector2 b = new Vector2(x2, y2);

		return Vector2.Distance(a, b);

	}

}
