using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutOpacity : MonoBehaviour {
    public Material[] materials;
    public Transform[] layouts;
    private void Start()
    {
        for(int i = 0; i < materials.Length; i++)
        {
            Color newCol = materials[i].color;
            newCol.a = 0;
            materials[i].color = newCol;
        }
        //fader.color = new Color(0, 0, 0, 1);
    }

    public void UpdateFade(float value)
    {
        //if (value == 1)
        //{
        //    for (int i = 0; i < layouts.length; i++)
        //        layouts[i].position = new vector3(0, -100, 0);
        //}
        //else
        //{
        //    for (int i = 0; i < layouts.length; i++)
        //        layouts[i].position = new vector3(0, 0, 0);
        //}

        for (int i = 0; i < materials.Length; i++)
        {
            Color newCol = materials[i].color;
            newCol.a = value;
            materials[i].color = newCol;
        }
        //fader.color = new Color(0, 0, 0, value);
    }

    public void Reset()
    {
        GetComponent<Slider>().value = 0;
    }
}
