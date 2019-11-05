using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floodfill : MonoBehaviour
{
    FloorManager fm;
    int startX = -1;
    int startY = -1;
    int endX = -1;
    int endY = -1;
    Queue<Coord> q;
    int lastX = -1;
    int lastY = -1;

    // Start is called before the first frame update
    void Start()
    {
        fm = GameObject.Find("GameManager").GetComponent<FloorManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(startX != -1)
        {
           
            if(q.Count != 0)
            {
                int[] dx = { 1, 0, -1, 0 };
                int[] dy = { 0, 1, 0, -1 };
                Coord n = q.Dequeue();



                fm.floorObjects[n.x, n.y].GetComponent<Renderer>().material.color = Color.red;
                fm.floorObjects[n.x, n.y].transform.localScale = new Vector3(0.9f, 0.9f * fm.floor[n.x, n.y] / 2, 0.9f);
                if (lastX != -1)
                    fm.floorObjects[lastX, lastY].GetComponent<Renderer>().material.color = Color.cyan;


                if (n.x == endX && n.y == endY)
                {
                    Debug.Log("Reached Destination");
                    q.Clear();
                    return;
                }

                for (int i = 0; i < 4; i++)
                {
                    int newX = n.x + dx[i];
                    int newY = n.y + dy[i];
                    if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                    {
                        if (fm.floor[newX, newY] <= 0)
                        {
                            fm.floor[newX, newY] = fm.floor[n.x, n.y] + 1;
                            q.Enqueue(new Coord(newX, newY));
                        }
                    }
                }

                lastX = n.x;
                lastY = n.y;


            }

        }
    }



    public void StartFlood(int sX, int sY, int eX, int eY)
    {
        startX = sX;
        startY = sY;
        endX = eX;
        endY = eY;
        q = new Queue<Coord>();

        q.Enqueue(new Coord(startX, startY));
        fm.floor[startX, startY] = 0;

    }
}
