using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Navigation
{
    public class Cell : MonoBehaviour
    {
        float scale = 1;                    //scale of the point
        SpriteRenderer sr;                  //stores renderer of sprite to change color
        float cellCount = 0;                  //how many cells are in area
        int maxcelldensity = 1;             //how many points need to be in to be considered "full"


        //colors to lerp between 
        Color empty = new Color(0, 0, 0, 0);
        Color full = new Color(0, 1, 0, .5f);

        private void Awake()
        {
            sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            sr.color = empty;
        }

        public void SetScale(float newscale)
        {
            scale = newscale;
            transform.localScale = Vector3.one * scale;
        }

        public void SetDensity(int newdens)
        {
            maxcelldensity = newdens;
            SetColor();
        }

        public void ResetData()
        {
            cellCount = 0;
            SetColor();
        }

        public void AddCell(float amount = 1)
        {
            cellCount += amount;
            if (cellCount > maxcelldensity)
                cellCount = maxcelldensity;
            SetColor();
        }

        void SetColor()
        {
            if (sr == null)
                sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            sr.color = Color.Lerp(empty, full, cellCount / maxcelldensity);
        }
    }
}