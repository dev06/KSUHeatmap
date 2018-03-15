using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour 
{

    public Vector2 width, height;
    public GameObject box;
    public List<GameObject> boxes;

    Vector3 pos;
    ClusterPoint[] points;
    GameObject temp;





    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            for(int i = 0; i < boxes.Count; i++)
            {
                temp = boxes[i];
                boxes.Remove(temp);
                Destroy(temp);
            }
        }
    }




    bool canPlace(Vector3 pos)
    {
        if(points == null)
        {
            points = GameObject.FindObjectsOfType<ClusterPoint>();

        }
        foreach(ClusterPoint cp in points)
        {
            if (Vector3.SqrMagnitude(pos - cp.position) < 3)
            return false;
        }

        if (boxes.Count > 0)
        {
            foreach (GameObject b in boxes)
            {
                if (Vector3.SqrMagnitude(pos - b.transform.position) < 8)
                return false;
            }
        }
        return true;
    }


    public void SpawnBoxes(Location location)
    {
        try
        {
            this.width = new Vector2(0, location.width); 
            this.height = new Vector2(0, location.height);

            float spacing = 3; 

            for(int y = 0;y < location.height / spacing; y++)
            {
                for(int x = 0; x < location.width / spacing;  x++)
                {
                    pos = new Vector3(x * spacing + 1f, 0, y * spacing); 
                    if (canPlace(pos))
                    {
                        GameObject go = (GameObject)Instantiate(box, pos, Quaternion.identity); 
                        go.transform.SetParent(transform);
                    } 
                }
            }

        }
        catch(System.Exception e)
        {
            Debug.Log("Location is null in spawner class"); 
        }


    }

    public void DestroyBoxes()
    {
     foreach(Transform t in transform)
     {
         Destroy(t.gameObject); 
     }
 }



 void OnDrawGizmos()
 {
    Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(width.x, 0, height.x), new Vector3(width.y, 0, height.x)); //bottom
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(width.x, 0, height.y), new Vector3(width.y, 0, height.y)); //top
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(width.x, 0, height.x), new Vector3(width.x, 0, height.y)); //left
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(width.y, 0, height.x), new Vector3(width.y, 0, height.y)); //right
    }
}
