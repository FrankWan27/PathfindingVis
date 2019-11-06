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
    List<Coord> path;
    bool finishFill = false;
    int travel;
    float solution = 0;
    
    // Update is called once per frame
    void Update()
    {
        if(startX != -1 && !finishFill)
        {
           
            if(q.Count != 0)
            {
                int[] dx = { 1, 0, -1, 0 };
                int[] dy = { 0, 1, 0, -1 };
                Coord n = q.Dequeue();


                fm.floorObjects[n.x, n.y].GetComponent<Renderer>().material.color = new Color(1f, 128f/255f, 128f/255f);

                if (lastX != -1 && (lastX != startX || lastY != startY))
                    fm.floorObjects[lastX, lastY].GetComponent<Renderer>().material.color = Color.Lerp(new Color(128f / 255f, 154f / 255f, 1f), new Color(128f / 255f, 1f, 159f/255f), (float)fm.floor[lastX, lastY].value / solution);


                if (n.x == endX && n.y == endY)
                {
                    Debug.Log("Reached Destination");
                    
                    while(n.x != startX || n.y != startY)
                    {
                        n = fm.floor[n.x, n.y].parent.GetCoord();
                        path.Add(n);
                    }
                    travel = path.Count - 2;
                    q.Clear();
                    finishFill = true;

                    return;
                }

                for (int i = 0; i < 4; i++)
                {
                    int newX = n.x + dx[i];
                    int newY = n.y + dy[i];
                    if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                    {
                        if (fm.floor[newX, newY].value <= 0)
                        {
                            fm.floor[newX, newY].parent = fm.floor[n.x, n.y];
                            fm.floor[newX, newY].value = fm.floor[n.x, n.y].value + 1;
                            q.Enqueue(new Coord(newX, newY));
                        }
                    }
                }

                lastX = n.x;
                lastY = n.y;


            }

        }

        if(finishFill && travel >= 0)
        {
            fm.floorObjects[path[travel].x, path[travel].y].GetComponent<Renderer>().material.color = new Color(203f/255f, 128f/255f, 1f);
            travel--;
        }
    }



    public void StartFlood(int sX, int sY, int eX, int eY)
    {
        startX = sX;
        startY = sY;
        endX = eX;
        endY = eY;
        q = new Queue<Coord>();
        fm = GameObject.Find("GameManager").GetComponent<FloorManager>();
        path = new List<Coord>();

        q.Enqueue(new Coord(startX, startY));
        fm.floor[startX, startY].value = 1;

        //calculate solution in advance to determine colors
        FloodFill();

    }

    void FloodFill()
    {
        int[,] temp = new int[fm.floor.GetLength(0), fm.floor.GetLength(1)];
        for(int i = 0; i < fm.floor.GetLength(0); i++)
        {
            for(int j = 0; j < fm.floor.GetLength(1); j++)
            {
                temp[i, j] = fm.floor[i, j].value;
            }
        }

        Queue<Coord> tempQ = new Queue<Coord>();
        tempQ.Enqueue(new Coord(startX, startY));

        while(tempQ.Count != 0)
        {
            int[] dx = { 1, 0, -1, 0 };
            int[] dy = { 0, 1, 0, -1 };

            Coord n = tempQ.Dequeue();

            if (n.x == endX && n.y == endY)
            {
                solution = temp[endX, endY];
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                int newX = n.x + dx[i];
                int newY = n.y + dy[i];
                if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                {
                    if (temp[newX, newY] <= 0)
                    {
                        temp[newX, newY] = temp[n.x, n.y] + 1;
                        tempQ.Enqueue(new Coord(newX, newY));
                    }
                }
            }
        }


    }
}
