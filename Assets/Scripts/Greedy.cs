using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greedy : MonoBehaviour
{
    FloorManager fm;
    Node[,] nodes;
    int startX = -1;
    int startY = -1;
    int endX = -1;
    int endY = -1;
    int lastX = -1;
    int lastY = -1;
    Coord curr;
    float[,] estimate;
    float solution = 0;
    bool finishGreed = false;
    List<Coord> path;
    int travel = -1;


    //define colors
    Color purple = new Color(203f / 255f, 128f / 255f, 1f);
    Color blue = new Color(128f / 255f, 154f / 255f, 1f);
    Color green = new Color(128f / 255f, 1f, 159f / 255f);
    Color red = new Color(1f, 128f / 255f, 128f / 255f);

    private void Update()
    {

        if (startX != -1 && !finishGreed)
        {
            int[] dx = { 1, 0, -1, 0 };
            int[] dy = { 0, 1, 0, -1 };

            //color current block red
            fm.ColorBlock(curr.x, curr.y, red);

            //if previous node was not start node, color it gradient
            if (lastX != -1 && (lastX != startX || lastY != startY))
               fm.ColorBlock(lastX, lastY, Color.Lerp(blue, green, (float)nodes[lastX, lastY].value / solution));

            //check if we arrived at destination
            if (curr.x == endX && curr.y == endY)
            {

                //travel through parent nodes to store path
                while (curr.x != startX || curr.y != startY)
                {
                    curr = nodes[curr.x, curr.y].parent.GetCoord();
                    path.Add(curr);
                }
                // -2 instead of -1 because we dont want to overwrite starting node
                travel = path.Count - 2;
                finishGreed = true;
                return;
            }

            lastX = curr.x;
            lastY = curr.y;

            //get best direction
            float lowestEstimate = -1;
            Coord next = new Coord(-1, -1);
            for (int i = 0; i < 4; i++)
            {
                int newX = curr.x + dx[i];
                int newY = curr.y + dy[i];
                if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                {
                    //check if unvisited
                    if (nodes[newX, newY].value <= 0)
                    {
                        if (lowestEstimate == -1 || estimate[newX, newY] < lowestEstimate)
                        {
                            lowestEstimate = estimate[newX, newY];
                            next.x = newX;
                            next.y = newY;
                        }
                    }
                }
            }

            if (lowestEstimate != -1)
            {
                //move once in that direction
                nodes[next.x, next.y].value = nodes[curr.x, curr.y].value + 1;
                nodes[next.x, next.y].parent = nodes[curr.x, curr.y];
                curr = next;
            }
            else //can't keep going in this direction, move back
            {
                if (nodes[curr.x, curr.y].parent != nodes[curr.x, curr.y])
                {
                    curr = nodes[curr.x, curr.y].parent.GetCoord();
                }
                else //cant go anywhere on root node
                {
                    finishGreed = true;
                }
            }

            
        }

        if(finishGreed && travel >= 0)
        {
            fm.ColorBlock(path[travel].x, path[travel].y, purple);
            travel--;
        }
    }

    public void StartGreed(int sX, int sY, int eX, int eY)
    {
        startX = sX;
        startY = sY;
        endX = eX;
        endY = eY;
        fm = GameObject.Find("GameManager").GetComponent<FloorManager>();
        path = new List<Coord>();
        estimate = new float[fm.floor.GetLength(0), fm.floor.GetLength(1)];

        nodes = new Node[fm.floor.GetLength(0), fm.floor.GetLength(1)];
        curr = new Coord(startX, startY);
        //calculate solution in advance to determine colors
        GreedEarly();
    }

    void GreedEarly()
    {
        Node[,] temp = new Node[fm.floor.GetLength(0), fm.floor.GetLength(1)];
        for (int i = 0; i < fm.floor.GetLength(0); i++)
        {
            for (int j = 0; j < fm.floor.GetLength(1); j++)
            {
                nodes[i, j] = new Node(fm.floor[i, j].x, fm.floor[i, j].y, fm.floor[i, j].value);
                temp[i, j] = new Node(fm.floor[i, j].x, fm.floor[i, j].y, fm.floor[i, j].value);
                estimate[i, j] = Vector2.Distance(new Vector2(i, j), new Vector2(endX, endY));
            }
        }


        nodes[startX, startY].value = 1;
        temp[startX, startY].value = 1;
        temp[startX, startY].parent = temp[startX, startY];
        nodes[startX, startY].parent = nodes[startX, startY];
        Coord n = temp[startX, startY].GetCoord();
        bool abort = false;
        while(!abort)
        {
            int[] dx = { 1, 0, -1, 0 };
            int[] dy = { 0, 1, 0, -1 };

            if (n.x == endX && n.y == endY)
            {
                solution = temp[endX, endY].value;
                abort = true;
            }

            //get best direction
            float lowestEstimate = -1;
            Coord next = new Coord(-1, -1);
            for (int i = 0; i < 4; i++)
            {
                int newX = n.x + dx[i];
                int newY = n.y + dy[i];
                if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                {
                    //check if unvisited
                    if (temp[newX, newY].value <= 0)
                    {
                        if (lowestEstimate == -1 || estimate[newX, newY] < lowestEstimate)
                        {
                            lowestEstimate = estimate[newX, newY];
                            next.x = newX;
                            next.y = newY;
                        }
                    }
                }
            }

            if (lowestEstimate != -1)
            {
                //move once in that direction
                solution = Mathf.Max(solution, temp[n.x, n.y].value + 1);
                temp[next.x, next.y].value = temp[n.x, n.y].value + 1;
                temp[next.x, next.y].parent = temp[n.x, n.y];
                n = next;
            }
            else //can't keep going in this direction, move back
            {
                if (temp[n.x, n.y].parent != temp[n.x, n.y])
                {
                    n = temp[n.x, n.y].parent.GetCoord();
                }
                else
                {
                    Debug.Log("Exhausted all paths");
                    abort = true;
                }
            }
        }
    }
}
