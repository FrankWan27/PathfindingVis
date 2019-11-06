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

    //define colors
    Color purple = new Color(203f / 255f, 128f / 255f, 1f);
    Color blue = new Color(128f / 255f, 154f / 255f, 1f);
    Color green = new Color(128f / 255f, 1f, 159f / 255f);
    Color red = new Color(1f, 128f / 255f, 128f / 255f);

    void Update()
    {
        if(startX != -1 && !finishFill)
        {

            if (q.Count != 0)
            {
                int[] dx = { 1, 0, -1, 0 };
                int[] dy = { 0, 1, 0, -1 };
                Coord n = q.Dequeue();

                //color current block red
                fm.ColorBlock(n.x, n.y, red);

                //if previous node was not start node, color it gradient
                if (lastX != -1 && (lastX != startX || lastY != startY))
                    fm.ColorBlock(lastX, lastY, Color.Lerp(blue, green, (float)fm.floor[lastX, lastY].value / solution));

                //if we reach destination
                if (n.x == endX && n.y == endY)
                {

                    //travel through parent nodes to store path
                    while (n.x != startX || n.y != startY)
                    {
                        n = fm.floor[n.x, n.y].parent.GetCoord();
                        path.Add(n);
                    }
                    // -2 instead of -1 because we dont want to overwrite starting node
                    travel = path.Count - 2;
                    q.Clear();
                    finishFill = true;
                    return;
                }

                //add neighbors to queue if unvisited
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
            else
            {
                //if we run out of nodes to explore, finish coloring
                if (lastX != -1 && (lastX != startX || lastY != startY))
                    fm.ColorBlock(lastX, lastY, Color.Lerp(blue, green, (float)fm.floor[lastX, lastY].value / solution));
            }
        }

        if(finishFill && travel >= 0)
        {
            fm.ColorBlock(path[travel].x, path[travel].y, purple);
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
                        solution = temp[n.x, n.y] + 1;
                        temp[newX, newY] = temp[n.x, n.y] + 1;
                        tempQ.Enqueue(new Coord(newX, newY));
                    }
                }
            }
        }


    }
}
