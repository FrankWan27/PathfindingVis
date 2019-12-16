using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    FloorManager fm;
    Node[,] nodes;
    int startX = -1;
    int startY = -1;
    int endX = -1;
    int endY = -1;
    int lastX = -1;
    int lastY = -1;
    float solution = 0;
    bool finishAStar = false;
    List<Coord> path;
    int travel = -1;
    MinHeap mh;


    //define colors
    Color purple = new Color(203f / 255f, 128f / 255f, 1f);
    Color blue = new Color(128f / 255f, 154f / 255f, 1f);
    Color green = new Color(128f / 255f, 1f, 159f / 255f);
    Color red = new Color(1f, 128f / 255f, 128f / 255f);

    private void Update()
    {

        if (startX != -1 && !finishAStar)
        {
            if (mh.HasNext())
            {
                Node n = mh.Pop();
                int[] dx = { 1, 0, -1, 0 };
                int[] dy = { 0, 1, 0, -1 };

                //color current block red
                fm.ColorBlock(n.x, n.y, red);

                //if previous node was not start node, color it gradient
                if (lastX != -1 && (lastX != startX || lastY != startY))
                    fm.ColorBlock(lastX, lastY, Color.Lerp(blue, green, (float)nodes[lastX, lastY].value / solution));

                //check if we arrived at destination
                if (n.x == endX && n.y == endY)
                {
                    mh.Clear();

                    //travel through parent nodes to store path
                    while (n.x != startX || n.y != startY)
                    {
                        n = nodes[n.x, n.y].parent;
                        path.Add(n.GetCoord());
                    }
                    // -2 instead of -1 because we dont want to overwrite starting node
                    travel = path.Count - 2;
                    finishAStar = true;
                    return;
                }

                lastX = n.x;
                lastY = n.y;

                for (int i = 0; i < 4; i++)
                {
                    int newX = n.x + dx[i];
                    int newY = n.y + dy[i];
                    if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                    {
                        //if not a wall
                        if (fm.floor[newX, newY].value == 0)
                        {
                            float tempH = nodes[n.x, n.y].value + 1 + Vector2.Distance(new Vector2(newX, newY), new Vector2(endX, endY)) + Tools.HeightDiff(fm.floor[n.x, n.y], fm.floor[newX, newY]);
                            if (tempH < nodes[newX, newY].heuristic)
                            {
                                nodes[newX, newY].heuristic = tempH;
                                nodes[newX, newY].value = nodes[n.x, n.y].value + 1 + Tools.HeightDiff(fm.floor[n.x, n.y], fm.floor[newX, newY]);
                                nodes[newX, newY].parent = nodes[n.x, n.y];
                                mh.Insert(nodes[newX, newY]);
                            }
                        }
                    }
                }
            }
            else
            {
                //if we run out of nodes to explore, finish coloring
                if (lastX != -1 && (lastX != startX || lastY != startY))
                    fm.ColorBlock(lastX, lastY, Color.Lerp(blue, green, (float)nodes[lastX, lastY].value / solution));
            }
        }

        if (finishAStar && travel >= 0)
        {
            fm.ColorBlock(path[travel].x, path[travel].y, purple);
            travel--;
        }
    }

    public void StartAStar(int sX, int sY, int eX, int eY)
    {
        startX = sX;
        startY = sY;
        endX = eX;
        endY = eY;
        fm = GameObject.Find("GameManager").GetComponent<FloorManager>();
        path = new List<Coord>();
        mh = new MinHeap();

        nodes = new Node[fm.floor.GetLength(0), fm.floor.GetLength(1)];
        //calculate solution in advance to determine colors
        AStarEarly();
    }

    void AStarEarly()
    {
        Node[,] temp = new Node[fm.floor.GetLength(0), fm.floor.GetLength(1)];
        for (int i = 0; i < fm.floor.GetLength(0); i++)
        {
            for (int j = 0; j < fm.floor.GetLength(1); j++)
            {
                nodes[i, j] = new Node(fm.floor[i, j].x, fm.floor[i, j].y, fm.floor[i, j].value);
                temp[i, j] = new Node(fm.floor[i, j].x, fm.floor[i, j].y, fm.floor[i, j].value);
                temp[i, j].heuristic = float.MaxValue;
                nodes[i, j].heuristic = float.MaxValue;
            }
        }

        mh.Insert(nodes[startX, startY]);
        MinHeap mhtemp = new MinHeap();
        mhtemp.Insert(temp[startX, startY]);
        temp[startX, startY].heuristic = 1 + Vector2.Distance(new Vector2(startX, startY), new Vector2(endX, endY)) + Tools.HeightDiff(fm.floor[startX, startY], fm.floor[endX, endY]);
        nodes[startX, startY].heuristic = 1 + Vector2.Distance(new Vector2(startX, startY), new Vector2(endX, endY)) + Tools.HeightDiff(fm.floor[startX, startY], fm.floor[endX, endY]);

        nodes[startX, startY].value = 1;
        temp[startX, startY].value = 1;

        while (mhtemp.HasNext())
        {
            Node n = mhtemp.Pop();

            int[] dx = { 1, 0, -1, 0 };
            int[] dy = { 0, 1, 0, -1 };

            if (n.x == endX && n.y == endY)
            {
                solution = temp[endX, endY].value;
                GameObject.Find("GameManager").GetComponent<GameManager>().SetDist(solution);
                return;

            }

            for (int i = 0; i < 4; i++)
            {
                int newX = n.x + dx[i];
                int newY = n.y + dy[i];
                if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1))
                {

                    float tempH = temp[n.x, n.y].value + 1 + Vector2.Distance(new Vector2(newX, newY), new Vector2(endX, endY)) + Tools.HeightDiff(fm.floor[n.x, n.y], fm.floor[newX, newY]);
                    if(tempH < temp[newX, newY].heuristic)
                    {
                        // solution = Mathf.Max(temp[n.x, n.y].value + 1);
                        temp[newX, newY].heuristic = tempH;
                        temp[newX, newY].value = temp[n.x, n.y].value + 1 + Tools.HeightDiff(fm.floor[n.x, n.y], fm.floor[newX, newY]);
                        temp[newX, newY].parent = temp[n.x, n.y];
                        mhtemp.Insert(temp[newX, newY]);
                    }       
                }
            }
        }
    }
}
