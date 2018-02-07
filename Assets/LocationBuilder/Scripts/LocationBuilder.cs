using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationBuilder : MonoBehaviour {
    public static LocationBuilder INSTANCE
    {
        get
        {
            if(instance == null)
            {
                GameObject newIns = Instantiate(Resources.Load("Prefabs/LocationBuilder") as GameObject);
                instance = newIns.GetComponent<LocationBuilder>();
                instance.name = "LocationBuilder";
                instance.SetStartValues();
            }
            return instance;
        }
    }
    static LocationBuilder instance;
    static GameObject p_wall;
    static GameObject p_beacon;
    static float s_wallHeight;
    static float s_wallWidth;
    static float s_beaconScale;
    static float s_beaconHeight;

    [Header("PREFABS")]
    public GameObject wall;
    public GameObject beacon;

    [Header("STATS")]
    public float wallHeight;
    public float wallWidth;
    public float beaconScale;
    public float beaconHeight;

    void SetStartValues()
    {
        //if variables not set, set to a default
        if (wall == null)
            wall = Resources.Load("Prefabs/Wall") as GameObject;

        if (beacon == null)
            beacon = Resources.Load("Prefabs/Beacon") as GameObject;

        if(wallHeight == 0)
            wallHeight = 3;

        if (wallWidth == 0)
            wallWidth = .05f;

        if (beaconScale == 0)
            beaconScale = .2f;

        if (beaconHeight == 0)
            beaconHeight = 1.5f;

        //set all the static floats
        p_wall = wall;
        p_beacon = beacon;
        s_wallHeight = wallHeight;
        s_wallWidth = wallWidth;
        s_beaconScale = beaconScale;
        s_beaconHeight = beaconHeight;
    }

    void Awake()
    {
        if (instance != null)
            DestroyImmediate(gameObject);
        instance = this;
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public static void BuildWalls(List<Vector2> walls)
    {
        instance = INSTANCE;
        DestroyWalls();
        GameObject next_wall;
        Vector3 nextPosition = new Vector3(walls[0].x, 0, walls[0].y);
        for(int i = 0; i < walls.Count-1; i++)
        {
            next_wall = Instantiate(p_wall, instance.transform.Find("Walls"));
            next_wall.name = "Wall (" + i + ")";
            next_wall.transform.position = nextPosition;
            nextPosition.x = walls[i + 1].x;
            nextPosition.z = walls[i + 1].y;
            next_wall.transform.LookAt(nextPosition);
            next_wall.transform.localScale = new Vector3(s_wallWidth, s_wallHeight, (nextPosition - next_wall.transform.position).magnitude + (s_wallWidth/2));
        }
        next_wall = Instantiate(p_wall, instance.transform.Find("Walls"));
        next_wall.name = "Wall (" + (walls.Count-1) + ")";
        next_wall.transform.position = nextPosition;
        nextPosition.x = walls[0].x;
        nextPosition.z = walls[0].y;
        next_wall.transform.LookAt(nextPosition);
        next_wall.transform.localScale = new Vector3(s_wallWidth, s_wallHeight, (nextPosition - next_wall.transform.position).magnitude + (s_wallWidth / 2));
    }

    //seperate strings into Vector2s and give to BuildWalls(List<Vector2>) above
    public static void BuildWalls(List<string> walls)
    {
        List<Vector2> result = new List<Vector2>();
        string[] nextWall;
        for (int i = 0; i < walls.Count; i++)
        {
            nextWall = walls[i].Split(',');
            result.Add(new Vector2(float.Parse(nextWall[0]), float.Parse(nextWall[1])));
        }
        BuildWalls(result);
    }

    //x = posx, y = posz, z = roty
    public static void BuildBeacons(List<Vector3> beacons)
    {
        instance = INSTANCE;
        DestroyBeacons();
        GameObject nextBeacon;
        for(int i = 0; i < beacons.Count; i++)
        {
            nextBeacon = Instantiate(p_beacon, instance.transform.Find("Beacons"));
            nextBeacon.name = "Beacon (" + i + ")";
            nextBeacon.transform.position = new Vector3(beacons[i].x, s_beaconHeight, beacons[i].y);
            nextBeacon.transform.rotation = Quaternion.Euler(0, beacons[i].z, 0);
            nextBeacon.transform.localScale = new Vector3(s_beaconScale, s_beaconScale, s_beaconScale);
        }
    }

    //[0] = id, [1] = posx, [2] = posz, [3] = roty
    public static void BuildBeacons(List<string[]> beacons)
    {
        instance = INSTANCE;
        DestroyBeacons();
        GameObject nextBeacon;
        for (int i = 0; i < beacons.Count; i++)
        {
            nextBeacon = Instantiate(p_beacon, instance.transform.Find("Beacons"));
            nextBeacon.name = "Beacon_" + beacons[i][0];
            nextBeacon.transform.position = new Vector3(float.Parse(beacons[i][1]), s_beaconHeight, float.Parse(beacons[i][2]));
            nextBeacon.transform.rotation = Quaternion.Euler(0, float.Parse(beacons[i][3]), 0);
            nextBeacon.transform.localScale = new Vector3(s_beaconScale, s_beaconScale, s_beaconScale);
            nextBeacon.GetComponent<BeaconVisual>().SetInfo(beacons[i][0]);
        }
    }

    //seperate strings then give to BuildBeacons(List<string[]>) above
    public static void BuildBeacons(List<string> beacons)
    {
        List<string[]> result = new List<string[]>();
        string[] nextBeacon;
        for (int i = 0; i < beacons.Count; i++)
        {
            nextBeacon = beacons[i].Split(',');
            result.Add(nextBeacon);
        }
        BuildBeacons(result);
    }

    static void DestroyWalls()
    {
        instance = INSTANCE;
        Transform walls = instance.transform.Find("Walls");
        for(int i = walls.childCount-1; i >= 0; i--)
        {
            Destroy(walls.GetChild(i).gameObject);
        }
    }

    static void DestroyBeacons()
    {
        instance = INSTANCE;
        Transform beacons = instance.transform.Find("Beacons");
        for (int i = beacons.childCount - 1; i >= 0; i--)
        {
            Destroy(beacons.GetChild(i).gameObject);
        }
    }
}
