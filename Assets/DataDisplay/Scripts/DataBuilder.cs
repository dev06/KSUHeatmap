using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBuilder : MonoBehaviour {

    public static DataBuilder instance;

    public static DataBuilder INSTANCE
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load("Prefabs/DataBuilder") as GameObject).GetComponent<DataBuilder>();
                instance.name = "DataBuilder";
            }
            return instance;
        }
    }

    public static Material mat_none;
    public static Material mat_kalman;
    public static Material mat_gaussian;

    public static GameObject pref_datavisual;

    [Header("Filter Materials")]
    public Material m_none;
    public Material m_kalman;
    public Material m_gaussian;

    [Header("Prefabs")]
    public GameObject p_datavisual;

    void Awake()
    {
        //set this to instance if there is not one already, otherwise destroy
        if (instance != null)
            DestroyImmediate(this.gameObject);
        instance = this;
        //allows you to set new material if you want, or use default
        if (m_none == null)
            m_none = Resources.Load("Materials/None") as Material;
        if (m_kalman == null)
            m_kalman = Resources.Load("Materials/Kalman") as Material;
        if (m_gaussian == null)
            m_gaussian = Resources.Load("Materials/Gaussian") as Material;

        if (p_datavisual == null)
            p_datavisual = Resources.Load("Prefabs/DataVisual") as GameObject;

        //set the static variables for this class
        mat_none = m_none;
        mat_kalman = m_kalman;
        mat_gaussian = m_gaussian;
        pref_datavisual = p_datavisual;
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    //instantiate the data points and set their data
    public static void BuildData(List<DataPoint> datapoints)
    {
        instance = INSTANCE;
        GameObject nextDataPoint;
        for(int i = 0; i < datapoints.Count; i++)
        {
            nextDataPoint = Instantiate(pref_datavisual, instance.transform);
            nextDataPoint.transform.position = new Vector3(datapoints[i].position.x, 0, datapoints[i].position.y);
            nextDataPoint.GetComponent<DataVisual>().SetDataPointInfo(datapoints[i]);
            nextDataPoint.name = "DataPoint (" + i + ")";
        }
    }

    //parse the strings to make datapoints then give to BuildData(List<DataPoint>) above
    public static void BuildData(List<string> datarows)
    {
        if(datarows.Count < 2)
        {
            Debug.Log("YOU NEED AT LEAST 2 DATA POINTS...");
            return;
        }

        List<DataPoint> datapoints = new List<DataPoint>();

        string[] curRow = datarows[1].Split(',');
        string[] prevRow = datarows[0].Split(',');
        Vector2 prevPos = new Vector2(float.Parse(prevRow[1]), float.Parse(prevRow[2]));

        //temp variables
        int localTime = int.Parse(curRow[0]);
        Vector2 playerPos = new Vector2(float.Parse(curRow[1]), float.Parse(curRow[2]));
        int orientation = int.Parse(curRow[3]);
        List<string> closestBeacons = new List<string>();
        for (int j = 4; j < curRow.Length; j++)
        {
            if (curRow[j] != "")
                closestBeacons.Add(curRow[j]);
        }

        Vector2 movementVector =  playerPos - prevPos;
        prevPos = playerPos;

        datapoints.Add(new DataPoint(playerPos, movementVector, orientation, localTime, closestBeacons));



        for(int i = 2; i < datarows.Count; i++)
        {
            curRow = datarows[i].Split(',');

            localTime = int.Parse(curRow[0]);
            playerPos = new Vector2(float.Parse(curRow[1]), float.Parse(curRow[2]));
            orientation = int.Parse(curRow[3]);
            closestBeacons.Clear();
            for (int j = 4; j < curRow.Length; j++)
            {
                if (curRow[j] != "")
                    closestBeacons.Add(curRow[j]);
            }
            movementVector = playerPos - prevPos;
            prevPos = playerPos;

            datapoints.Add(new DataPoint(playerPos, movementVector, orientation, localTime, closestBeacons));
        }

        BuildData(datapoints);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
