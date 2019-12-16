using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//fix wall/height interaction
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    FloorManager fm;
    public bool pathMode = false;
    public bool wallMode = true;
    public bool heightMode = false;

    public Text distanceText;
    public float calcDist = 0;

    int mapMode = 0;

    int algorithm = 0;

    int pastX = -1;
    int pastY = -1;

    int startX = -1;
    int startY = -1;
    int endX = -1;
    int endY = -1;

    Color lightRed = new Color(1f, 222f / 255f, 222f / 255f);
    Color red = new Color(1f, 128f / 255f, 128f / 255f);

    GameObject arrowBase;
    LineRendererArrow arrowBody;

    void Start()
    {
        fm = GetComponent<FloorManager>();

        //Create Ground
        fm.Create();
        fm.Instantiate();

        arrowBase = GameObject.Find("ArrowBase");
        arrowBody = GameObject.Find("ArrowBody").GetComponent<LineRendererArrow>();
        arrowBase.SetActive(false);
        arrowBody.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        distanceText.text = "Distance: " + calcDist;

        Vector3 clickPosition = -Vector3.one;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            clickPosition = hit.point;
        }

        int x = Mathf.FloorToInt(clickPosition.x);
        int y = Mathf.FloorToInt(clickPosition.z);



        if (x > -1 && y > -1) //if mouse is within bounds
        {
            if (Input.GetMouseButton(0) && wallMode) //LMB while in wall mode
            {
                fm.WallBlock(x, y);
            }
            else if (Input.GetMouseButton(1) && wallMode) //RMB while in wall mode
            {
                fm.WalkBlock(x, y);
            }
            else if (Input.GetMouseButton(0) && heightMode) //LMB while in height mode
            {
                fm.RaiseBlock(x, y, 0.5f);
            }
            else if (Input.GetMouseButton(1) && heightMode) //RMB while in height mode
            {
                fm.LowerBlock(x, y, 0.5f);
            }
            else if (Input.GetMouseButtonDown(0) && pathMode) //LMB on click while in path mode
            {
                if (fm.floor[x, y].value == 0)
                {
                    arrowBase.SetActive(true);
                    arrowBase.transform.position = new Vector3(x + 0.5f, 0.7f, y + 0.5f);

                    startX = x;
                    startY = y;
                }
            }
            else if (Input.GetMouseButton(0) && pathMode && startX != -1) //dragging LMB
            {

                if ((x != startX || y != startY)) // off start node
                {
                    arrowBody.gameObject.SetActive(true);

                    arrowBody.ArrowOrigin = new Vector3(startX + 0.5f, 0.7f, startY + 0.5f);
                    arrowBody.ArrowTarget = new Vector3(x + 0.5f, 0.7f, y + 0.5f);
                    arrowBody.UpdateArrow();
                }
                else //still on start node
                {
                    arrowBody.gameObject.SetActive(false);
                }

                if (fm.floor[x, y].value != 0) //if hovering over wall
                {
                    arrowBody.GetComponent<Renderer>().material.color = lightRed;
                    arrowBase.GetComponent<Renderer>().material.color = lightRed;
                }
                else
                {
                    arrowBody.GetComponent<Renderer>().material.color = red;
                    arrowBase.GetComponent<Renderer>().material.color = red;
                }


                if (Input.GetMouseButtonDown(1)) //press RMB while holding LMB will cancel
                {
                    arrowBase.SetActive(false);
                    arrowBody.gameObject.SetActive(false);

                    startX = -1;
                    startY = -1;
                }
            }
            else if (Input.GetMouseButtonUp(0) && pathMode && startX >= 0) //released LMB 
            {
                arrowBase.SetActive(false);
                arrowBody.gameObject.SetActive(false);
                endX = x;
                endY = y;

                //check bounds for inputs
                if (fm.floor[endX, endY].value == 0)
                {
                    Debug.Log("Path from [" + startX + ", " + startY + "] to [" + endX + ", " + endY + "] using algorithm " + algorithm );
                    if (algorithm == 0) //Floodfill
                        fm.FloodFill(startX, startY, endX, endY);
                    else if (algorithm == 1) //Floodfill3D
                        fm.FloodFill3D(startX, startY, endX, endY);
                    else if (algorithm == 2) //Greedy
                        fm.Greedy(startX, startY, endX, endY);
                    else if (algorithm == 3) //AStar
                        fm.AStar(startX, startY, endX, endY);
                }
                startX = -1;
                startY = -1;

            }

        }
        else if (Input.GetMouseButton(0) && pathMode) // holding LMB while not on map
        {
            arrowBody.GetComponent<Renderer>().material.color = lightRed;
            arrowBase.GetComponent<Renderer>().material.color = lightRed;
        }
        else if (Input.GetMouseButtonUp(0) && pathMode) //let go of LMB while not on map
        {
            arrowBase.SetActive(false);
            arrowBody.gameObject.SetActive(false);

            startX = -1;
            startY = -1;
        }
        else
        { 

        }
    }

    public void changeAlgorithm(int v)                                                                          
    {
        algorithm = v;
    }

    public void changeMap(int v)
    {
        mapMode = v;
    }

    public void createMap()
    {
        if (mapMode == 0) //Clear
        {
            fm.ClearAllFloor();
        }
        else if (mapMode == 1) //Wall Maze
        {
            fm.GenerateMaze(0);
        }
        else if (mapMode == 2) //Tall Maze
        {
            fm.GenerateMaze(1);
        }
        else if (mapMode == 3) //2D Noise
        {
            fm.GenerateNoise(3, true, 0, 0);
        }
        else if (mapMode == 4) //3D Noise
        {
            fm.GenerateNoise(0, false, 0, 20);
        }
        else if (mapMode == 5) //Pillars
        {
            fm.GenerateNoise(16, false, 14, 20);
        }
    }

    public void SetDist(float d)
    {
        calcDist = d;
    }
}
