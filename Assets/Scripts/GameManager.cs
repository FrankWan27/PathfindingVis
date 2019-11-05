using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    FloorManager fm;
    public bool pathMode = false;
    public bool wallMode = true;

    int pastX = -1;
    int pastY = -1;

    int startX = -1;
    int startY = -1;
    int endX = -1;
    int endY = -1;

    void Start()
    {
        fm = GetComponent<FloorManager>();
        //Create Ground
        fm.Create();
        fm.Instantiate();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            Vector3 clickPosition = -Vector3.one;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                clickPosition = hit.point;
            }

            int x = Mathf.FloorToInt(clickPosition.x);
            int y = Mathf.FloorToInt(clickPosition.z);

            Debug.Log(x + " " + y);

            if (wallMode && x > -1 && y > -1 && !(x == pastX && y == pastY))
            {
                Debug.Log("Flipping");
                if (fm.floor[x, y] == 0)
                {
                    fm.floor[x, y] = 1;
                    fm.floorObjects[x, y].GetComponent<Renderer>().material.color = Color.black;
                    fm.floorObjects[x, y].transform.localScale = new Vector3(0.9f, 1.8f, 0.9f);
                }

                //else if (fm.floor[x, y] == 1)
                //{
                //    fm.floor[x, y] = 0;
                //    fm.floorObjects[x, y].GetComponent<Renderer>().material.color = Color.white;
                //    fm.floorObjects[x, y].transform.localScale = Vector3.one * 0.9f;
                // }
                pastX = x;
                pastY = y;
            }
            
            if(pathMode)
            {
                if(pastX == -1 && pastY == -1)
                {
                    startX = x;
                    startY = y;    
                }
                pastX = x;
                pastY = y;
            }
        }
        else if(pathMode && pastX != -1 && pastY != -1)
        {
            endX = pastX;
            endY = pastY;

            Debug.Log("Path from [" + startX + ", " + startY + "] to [" + endX + ", " + endY + "]");
            fm.FloodFill(startX, startY, endX, endY);

            pastX = -1;
            pastY = -1;

        }
        else
        {
            pastX = -1;
            pastY = -1;
        }


    }
}
