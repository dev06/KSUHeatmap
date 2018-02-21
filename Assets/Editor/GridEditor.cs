using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Navigation
{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Grid myScript = (Grid)target;
            if (GUILayout.Button("Build Grid"))
            {
                myScript.GenerateCells();
            }
            if(GUILayout.Button("GenerateRandomPoints"))
            {
                List<Vector2> randpoints = new List<Vector2>();
                for(int i = 0; i < 500; i++)
                {
                    randpoints.Add(new Vector2(Random.Range(0, myScript.roomscale.x), Random.Range(0, myScript.roomscale.y)));
                }
                myScript.SetData(randpoints);
            }
        }
    }
}
