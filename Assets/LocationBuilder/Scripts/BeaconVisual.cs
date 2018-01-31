using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconVisual : MonoBehaviour {
    public string macID;
    public Vector2 position;

    public void SetInfo(string id)
    {
        macID = id;
        position = new Vector2(transform.position.x, transform.position.z);
    }
}
