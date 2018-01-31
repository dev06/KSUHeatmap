using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataVisual : MonoBehaviour {
    [Header("Visuals")]
    public GameObject point;
    public GameObject heading;
    public GameObject movement;
    public GameObject movement_arrow;

    //used to test closest beacon functionality
    [Header("ClosestBeacons")]
    public string[] closestBeacons;

    DataPoint d_point;

    public void SetDataPointInfo(DataPoint in_data)
    {
        d_point = new DataPoint(in_data);
        heading.transform.localRotation = Quaternion.Euler(0, d_point.angle, 0);
        Vector3 lookat = new Vector3(transform.position.x + d_point.movement.x, movement.transform.position.y, transform.position.z + d_point.movement.y);
        movement.transform.LookAt(lookat, Vector3.up);
        lookat.y = movement_arrow.transform.position.y;
        movement_arrow.transform.LookAt(lookat, Vector3.up);
        movement.transform.localScale = new Vector3(movement.transform.localScale.x, movement.transform.localScale.y, -d_point.movement.magnitude);
        closestBeacons = new string[d_point.closestBeacons.Length];
        d_point.closestBeacons.CopyTo(closestBeacons, 0);
        ShowInfo();
    }

    //show heading and movement
    public void ShowInfo()
    {
        heading.SetActive(true);
        movement.SetActive(true);
        movement_arrow.SetActive(true);
    }

    //hide heading and movement to see just points
    public void HideInfo()
    {
        heading.SetActive(false);
        movement.SetActive(false);
        movement_arrow.SetActive(false);
    }
}
