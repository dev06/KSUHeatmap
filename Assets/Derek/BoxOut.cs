using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOut : MonoBehaviour {

    public bool left, right, up, down;
    public GameObject l, r, u, d;
    public Vector2 width, height;
    public float bound;


    private Vector3 initialScale;
    private Vector3 initialPosition;
    private bool okay;
	// Use this for initialization
    void Awake()
    {
        initialScale = transform.localScale;
        initialPosition = transform.position;
        okay = true;
        //if (up)
        //    StartCoroutine("GrowUp");
        //if (down)
        //    StartCoroutine("GrowDown");
        //if (left)
        //    StartCoroutine("GrowLeft");
        //if (right)
        //    StartCoroutine("GrowRight");
        //box = Instantiate(boxPrefab, new Vector3(Random.Range(width.x, width.y), 0, Random.Range(height.x, height.y)), Quaternion.identity);
        //blocker = new Box(box);
    }
    
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {//blocker.self.transform.position = new Vector3(Random.Range(width.x, width.y), 0, Random.Range(height.x, height.y));
            okay = !okay;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            transform.localScale = initialScale;
            transform.position = initialPosition;
        }

        if (up)
        StartCoroutine("GrowUp");
        if (down)
        StartCoroutine("GrowDown");
        if (left)
        StartCoroutine("GrowLeft");
        if (right)
        StartCoroutine("GrowRight");


    }

    private float speed = .03f; 

    IEnumerator GrowUp()
    {
        while (okay)
        {
            transform.position += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            l.transform.localScale += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            l.transform.position += new Vector3(0, 0, .5f) * Time.deltaTime * speed;
            r.transform.localScale += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            r.transform.position += new Vector3(0, 0, .5f) * Time.deltaTime * speed;
            yield return null;
        }
        StopCoroutine("GrowUp");
    }

    IEnumerator GrowDown()
    {
        while (okay)
        {
            transform.position -= new Vector3(0, 0, 1) * Time.deltaTime * speed;
            l.transform.localScale += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            l.transform.position -= new Vector3(0, 0, .5f) * Time.deltaTime * speed;
            r.transform.localScale += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            r.transform.position -= new Vector3(0, 0, .5f) * Time.deltaTime * speed;
            yield return null;
        }
        StopCoroutine("GrowDown");
    }

    IEnumerator GrowLeft()
    {
        while (okay)
        {
            transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * speed;
            u.transform.localScale += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            u.transform.position -= new Vector3(.5f, 0, 0) * Time.deltaTime * speed;
            d.transform.localScale += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            d.transform.position -= new Vector3(.5f, 0, 0) * Time.deltaTime * speed;
            yield return null;
        }
        StopCoroutine("GrowLeft");
    }

    IEnumerator GrowRight()
    {
        while (okay)
        {
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            u.transform.localScale += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            u.transform.position += new Vector3(.5f, 0, 0) * Time.deltaTime * speed;
            d.transform.localScale += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            d.transform.position += new Vector3(.5f, 0, 0) * Time.deltaTime * speed;
            yield return null;
        }
        StopCoroutine("GrowRight");
    }

    ClusterPoint point;
    Centroid centroid;
    float magnitude;
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Point")
        {
            point = col.gameObject.GetComponent<ClusterPoint>();
            centroid = col.gameObject.GetComponent<ClusterPoint>().centroid;

            if (point.distance > bound)
            {

                col.gameObject.GetComponent<ClusterPoint>().Active = false; 
            //col.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                col.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            else
            okay = false;
            return;
        }

        if(col.gameObject.tag == "Player")
        {
            okay = false;
        }
        //}

        //if (okey)
        //{
        //    if (col.gameObject.tag == "Point")
        //    {
        //        point = col.gameObject.GetComponent<ClusterPoint>();
        //        centroid = col.gameObject.GetComponent<ClusterPoint>().centroid;
        //        //magnitude = Vector3.SqrMagnitude(centroid.position - point.position);
        //        //Debug.Log(magnitude);
        //        if (point.distance > bound)
        //            col.gameObject.GetComponent<MeshRenderer>().enabled = false;
        //        else
        //            okey = false;
        //    }
        //}
    }
}
