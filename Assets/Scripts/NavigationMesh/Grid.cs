using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Navigation
{
    [System.Serializable]
    public struct CellEffect
    {
        [Range(0f,1f)]
        public float ratio;

        public CellEffect(float r)
        {
            ratio = Mathf.Clamp(r,0f,1f);
        }
    }

    [ExecuteInEditMode]
    public class Grid: MonoBehaviour
    {
        [Header("Room Info")]
        public Vector2 roomscale;       //scale of room (meters)
        public Vector2 extracells;      //number of extra
        [Header("Cell Info")]
        public GameObject prefab;       //prefab of the cells to generate
        public float cellscale = 1;     //how big the scales
        [HideInInspector]
        public List<List<Cell>> cells = new List<List<Cell>>();     //list of the prexisting cells (cells[column][row], cells[x][y])


        [Header("Cell Behaviour")]
        public int celldensity;         //how many points need to be in cell for it to be red
        public List<CellEffect> celleffects;     //index represents neighbor, value represents ratio (all 0-1)


        #region Generation //////////////////////////////////////////////////////////////////////////////////////////////

        GameObject curobj;
        Transform curcoltrans, cellholder;
        Cell curcell;
        List<Cell> col = new List<Cell>();
        Vector3 curpos = new Vector3(), gridpos;
        int currow = 0, curcol = 0;

        private void Start()
        {
            if(celleffects.Count == 0)
            {
                celleffects.Add(new CellEffect(1));
            }
        }

        public void GenerateCells()
        {
            //delete previous
            ClearPreviousCells();

            //initial values
            currow = 0;
            curcol = 0;
            gridpos = transform.position;

            //iterate through width
            for (float x = 0f; x < roomscale.x + 1; x+= cellscale)
            {
                curcoltrans = new GameObject("Column " + curcol).transform;
                curcoltrans.SetParent(cellholder);
                //iterate through height
                for(float y = 0f; y < roomscale.y + 1; y+= cellscale)
                {
                    curpos.x = gridpos.x + (curcol * cellscale);
                    curpos.z = gridpos.z + (currow * cellscale);
                    curobj = Instantiate(prefab, curpos, Quaternion.identity, curcoltrans);
                    curobj.name = "Cell (" + curcol + ", " + currow + ")";
                    curcell = curobj.GetComponent<Cell>();
                    curcell.SetScale(cellscale);
                    curcell.SetDensity(celldensity);
                    col.Add(curcell);   //add to current column
                    currow++;
                }
                curcol++;
                currow = 0;
                cells.Add(new List<Cell> (col)); //add current column to the rows of cells
                col.Clear();    //reset column for next one
            }
        }

        void ClearPreviousCells()
        {
            if(transform.Find("CELLS") != null)
            DestroyImmediate(transform.Find("CELLS").gameObject);

            cellholder = new GameObject("CELLS").transform;
            cellholder.parent = transform;
            cells.Clear();
        }

        #endregion Generation //////////////////////////////////////////////////////////////////////////////////////////////

        //tells the cells their data (by default, reset previous data)
        public void SetData(List<Vector2> newdata, bool reset = true)
        {

            try
            {


                DataPartition dp = new DataPartition(newdata); 

            }
            catch
            {
                Debug.Log("SOMETHING WENT WRONG, PROBS OUT OF BOUNDS");
            }
        }

        public void DisplayNavmesh(List<Vector2> newdata, bool reset = true)
        {
            try
            {
               if (reset)
               {
                for (int x = 0; x < cells.Count; x++)
                {
                    for (int y = 0; y < cells[x].Count; y++)
                    {
                        cells[x][y].ResetData();
                    }
                }


                int temprow = 0, tempcol = 0;
                for (int i = 0; i < newdata.Count; i++)
                {
                    temprow = (int)Mathf.Floor(newdata[i].y / cellscale);
                    tempcol = (int)Mathf.Floor(newdata[i].x / cellscale);
                    cells[tempcol][temprow].AddCell(celleffects[0].ratio);
                    for(int n = 1; n < celleffects.Count; n++)
                    {
                        List<Cell> curneighbs = GetNeighbors(tempcol, temprow, n);
                        foreach(Cell curneigh in curneighbs)
                        {
                            curneigh.AddCell(celleffects[n].ratio);
                        }
                        curneighbs.Clear();
                    }
                }
            }
        }
        catch
        {
            Debug.Log("SOMETHING WENT WRONG, PROBS OUT OF BOUNDS");
        }

    }

        //find a certain neighbor ring at distance
    List<Cell> GetNeighbors(int cellx, int celly, int distance)
    {
        if (distance == 0)
        return new List<Cell>() { cells[cellx][celly] };
        List<Cell> neighbors = new List<Cell>();
            //add the horizontal neighbors (including corners)
        for(int i = cellx-distance; i <= cellx+distance; i++)
        {
                //if out of bounds, dont do anything
            if (i < 0 || i >= cells.Count)
            continue;
                //add the top neighbors
            if(celly + distance < cells[0].Count)
            {
                neighbors.Add(cells[i][celly + distance]);
            }
                //add the bottom neighbors
            if(celly - distance >= 0)
            {
                neighbors.Add(cells[i][celly - distance]);
            }
        }

            //add the vertical neighbors (except for corners)
        for (int i = celly - distance+1; i <= celly + distance-1; i++)
        {
                //if out of bounds, dont do anything
            if (i < 0 ||i >= cells[0].Count)
            continue;
                //add the right neighbors
            if (cellx + distance < cells.Count)
            {
                neighbors.Add(cells[cellx + distance][i]);
            }
                //add the left neighbors
            if (cellx - distance >= 0)
            {
                neighbors.Add(cells[cellx - distance][i]);
            }
        }

        return neighbors;
    }
}
}