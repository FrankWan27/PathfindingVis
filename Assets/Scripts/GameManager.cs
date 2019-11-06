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
        Vector3 clickPosition = -Vector3.one;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            clickPosition = hit.point;
        }

        int x = Mathf.FloorToInt(clickPosition.x);
        int y = Mathf.FloorToInt(clickPosition.z);

        if (x > -1 && y > -1)
        {
            if (Input.GetMouseButton(0) && wallMode)
            {

                Debug.Log("Wall");

                fm.WallBlock(x, y);


            }
            else if (Input.GetMouseButtonDown(0) && pathMode)
            {
                arrowBase.SetActive(true);
                arrowBase.transform.position = new Vector3(x + 0.5f, 0.7f, y + 0.5f);
                

                startX = x;
                startY = y;

                pastX = x;
                pastY = y;
            }
            else if (Input.GetMouseButton(0) && pathMode && startX != -1 && (x != startX || y != startY))
            {
                arrowBody.gameObject.SetActive(true);

                arrowBody.ArrowOrigin = new Vector3(startX + 0.5f, 0.7f, startY + 0.5f);
                arrowBody.ArrowTarget = new Vector3(x + 0.5f, 0.7f, y + 0.5f);
                arrowBody.UpdateArrow();

               

            }
            else if (Input.GetMouseButtonUp(0) && pathMode)
            {
                arrowBase.SetActive(false);
                arrowBody.gameObject.SetActive(false);
                endX = x;
                endY = y;

                Debug.Log("Path from [" + startX + ", " + startY + "] to [" + endX + ", " + endY + "]");
                fm.FloodFill(startX, startY, endX, endY);

                pastX = -1;
                pastY = -1;
                startX = -1;
                startY = -1;

            }
            else if (Input.GetMouseButton(1))
            {
                if (wallMode && x > -1 && y > -1)
                {
                    Debug.Log("Walk");

                    fm.ResetBlock(x, y);
                }
            }
        }
        else
        {
            pastX = -1;
            pastY = -1;
        }



    }
}
