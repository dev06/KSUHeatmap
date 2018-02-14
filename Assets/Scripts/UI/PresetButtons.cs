using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetButtons : MonoBehaviour {
    //presets
    public string text_location;
    public string text_data;
    public int num_clusters;
    public float scale_point;
    public float threshold_distance;
    public float delay_calculatecentroid;

    //cluster ui to tell that we changed
    public ClusterUI clusterui;

    public void PressButton()
    {
        clusterui.SetPresets(this);
    }
}
