using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floodfill3D : MonoBehaviour
{
    //Exhaustive FloodFill
    FloorManager fm;
    Node[,] nodes;
    int startX = -1;
    int startY = -1;
    int endX = -1;
    int endY = -1;
    Queue<Coord> q;
    int lastX = -1;
    int lastY = -1;
    float height = 0;
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
                    fm.ColorBlock(lastX, lastY, Color.Lerp(blue, green, (float)nodes[lastX, lastY].value / solution));

                //if we reach destination
                if (n.x == endX && n.y == endY)
                {


                }

                //add neighbors to queue if unvisited
                for (int i = 0; i < 4; i++)
                {
                    int newX = n.x + dx[i];
                    int newY = n.y + dy[i];
                    if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                    {
                        float calcDist = nodes[n.x, n.y].value + 1 + Tools.HeightDiff(fm.floor[n.x, n.y], fm.floor[newX, newY]);
                        if (nodes[newX, newY].value <= 0 || calcDist < nodes[newX, newY].value)
                        {
                            nodes[newX, newY].parent = nodes[n.x, n.y];
                            nodes[newX, newY].value = calcDist;
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
                    fm.ColorBlock(lastX, lastY, Color.Lerp(blue, green, (float)nodes[lastX, lastY].value / solution));

                Coord n = new Coord(endX, endY);
                //travel through parent nodes to store path
                while ((n.x != startX || n.y != startY) && n != null)
                {
                    n = nodes[n.x, n.y].parent.GetCoord();
                    path.Add(n);
                }
                // -2 instead of -1 because we dont want to overwrite starting node
                travel = path.Count - 2;
                q.Clear();
                finishFill = true;
                return;
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
        nodes = new Node[fm.floor.GetLength(0), fm.floor.GetLength(1)];

        q.Enqueue(new Coord(startX, startY));

        //calculate solution in advance to determine colors
        FloodFillEarly();

    }

    void FloodFillEarly()
    {
        float[,] temp = new float[fm.floor.GetLength(0), fm.floor.GetLength(1)];
        for(int i = 0; i < fm.floor.GetLength(0); i++)
        {
            for(int j = 0; j < fm.floor.GetLength(1); j++)
            {
                nodes[i, j] = new Node(fm.floor[i, j].x, fm.floor[i, j].y, fm.floor[i, j].value, fm.floor[i, j].height);
                temp[i, j] = fm.floor[i, j].value;
            }
        }

        //setup for live update version
        nodes[startX, startY].value = 1;
        temp[startX, startY] = 1;

        Queue<Coord> tempQ = new Queue<Coord>();
        tempQ.Enqueue(new Coord(startX, startY));

        while(tempQ.Count > 0)
        {
            int[] dx = { 1, 0, -1, 0};
            int[] dy = { 0, 1, 0, -1};

            Coord n = tempQ.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int newX = n.x + dx[i];
                int newY = n.y + dy[i];
                if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                {
                    float calcDist = temp[n.x, n.y] + 1 + Tools.HeightDiff(fm.floor[n.x, n.y], fm.floor[newX, newY]);

                    if (temp[newX, newY] <= 0 || calcDist < temp[newX, newY])
                    {
                        solution = calcDist;
                        temp[newX, newY] = calcDist;
                        tempQ.Enqueue(new Coord(newX, newY));
                    }
                }
            }
        }

        solution = temp[endX, endY];                
        GameObject.Find("GameManager").GetComponent<GameManager>().SetDist(solution);


    }
}
