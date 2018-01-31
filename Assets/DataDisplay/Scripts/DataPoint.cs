using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint {
    
    public readonly Vector2 position;       //holds where the point is
    public readonly Vector2 movement;       //holds where the point moved to next point
    public readonly float angle;            //holds where the point is facing
    public readonly int time;             //holds the local time of the point
    public readonly string filter;          //holds what type of filter it is
    public readonly string[] closestBeacons;    //holds a string of the closest n beacons

    //nothing
    public DataPoint()
    {
        position = new Vector2();
        movement = new Vector2();
        angle = 0;
        time = 0;
        filter = Filter.None;
    }

    public DataPoint(DataPoint other)
    {
        position = other.position;
        movement = other.movement;
        angle = other.angle;
        time = other.time;
        filter = other.filter;
        closestBeacons = new string[other.closestBeacons.Length];
        other.closestBeacons.CopyTo(closestBeacons, 0);
    }

    //no filter, no closest beacons
    public DataPoint(Vector2 pos, Vector2 mov, float ang, int tim)
    {
        position = pos;
        movement = mov;
        angle = ang;
        time = tim;
        filter = Filter.None;
    }

    //no closest beacons
    public DataPoint(Vector2 pos, Vector2 mov, float ang, int tim, string fil)
    {
        position = pos;
        movement = mov;
        angle = ang;
        time = tim;
        filter = fil;
    }

    //no filter
    public DataPoint(Vector2 pos, Vector2 mov, float ang, int tim, List<string> clobea)
    {
        position = pos;
        movement = mov;
        angle = ang;
        time = tim;
        filter = Filter.None;
        closestBeacons = new string[clobea.Count];
        clobea.ToArray().CopyTo(closestBeacons,0);
    }

    //everything
    public DataPoint(Vector2 pos, Vector2 mov, float ang, int tim, string fil, List<string> clobea)
    {
        position = pos;
        movement = mov;
        angle = ang;
        time = tim;
        filter = fil;
        closestBeacons = new string[clobea.Count];
        clobea.ToArray().CopyTo(closestBeacons, 0);
    }
}
