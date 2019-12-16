using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public Node[,] floor;
    public GameObject[,] floorObjects;
    public GameObject floorTile;
    public GameObject mazeGen;
    public GameObject floodfill;
    public GameObject floodfill3D;
    public GameObject greedy;
    public GameObject aStar;
    public int sizeX;
    public int sizeY;

    public void Create()
    {
        floor = new Node[sizeX, sizeY];
    }

    public void Instantiate()
    {
        DestroyObjects();

        GameObject parent = new GameObject();
        parent.name = "Floor";

        floorObjects = new GameObject[sizeX, sizeY];


        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                GameObject tile = Instantiate(floorTile, new Vector3(0.5f + i * 1f, 0 ,0.5f + j * 1f), Quaternion.identity);
                floorObjects[i, j] = tile;
                floor[i, j] = new Node(i, j, 0, 0);

                WalkBlock(i, j);
                tile.transform.parent = parent.transform;
            }
        }
    }

    public void GenerateMaze(int option)
    {
        //option = 0, generate Wall Maze
        //option = 1, generate Tall Maze
        DestroyObjects();
        //Using Recursive Backtracking Algorithm
        //http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking
        Instantiate();
        MazeGenerator mg = GameObject.Instantiate(mazeGen, Vector3.zero, Quaternion.identity).GetComponent<MazeGenerator>();

        mg.Generate(option);
    }

    public void GenerateNoise(int rand, bool wall, int minHeight, int maxHeight)
    {
        DestroyObjects();
        Instantiate();
        for (int i = 0; i < floor.GetLength(0); i++)
        {
            for (int j = 0; j < floor.GetLength(1); j++)
            {
                int r = Random.Range(0, rand);
                if (r == 0)
                {
                    if(wall)
                        WallBlock(i, j);
                    else
                        RaiseBlock(i, j, Random.Range(minHeight, maxHeight) * 0.5f);
                }
            }
        }
    }

    public void FloodFill(int startX, int startY, int endX, int endY)
    {

        ClearPath();
        Floodfill ff = GameObject.Instantiate(floodfill, Vector3.zero, Quaternion.identity).GetComponent<Floodfill>();

        ff.StartFlood(startX, startY, endX, endY);
    }

    public void FloodFill3D(int startX, int startY, int endX, int endY)
    {

        ClearPath();
        Floodfill3D ff3d = GameObject.Instantiate(floodfill3D, Vector3.zero, Quaternion.identity).GetComponent<Floodfill3D>();

        ff3d.StartFlood(startX, startY, endX, endY);
    }

    public void Greedy(int startX, int startY, int endX, int endY)
    {
        ClearPath();
        Greedy g = GameObject.Instantiate(greedy, Vector3.zero, Quaternion.identity).GetComponent<Greedy>();

        g.StartGreed(startX, startY, endX, endY);
    }

    public void AStar(int startX, int startY, int endX, int endY)
    {
        ClearPath();
        AStar a = GameObject.Instantiate(aStar, Vector3.zero, Quaternion.identity).GetComponent<AStar>();

        a.StartAStar(startX, startY, endX, endY);
    }

    public void AllWallFloor()
    {
        for (int i = 0; i < floor.GetLength(0); i++)
        {
            for (int j = 0; j < floor.GetLength(1); j++)
            {
                WallBlock(i, j);
            }
        }
    }

    public void AllTallFloor(float h)
    {
        for (int i = 0; i < floor.GetLength(0); i++)
        {
            for (int j = 0; j < floor.GetLength(1); j++)
            {
                RaiseBlock(i, j, h);
            }
        }
    }


    public void ClearAllFloor()
    {
        Instantiate();
    }
    
    public void ClearPath()
    {

        DestroyPathfind();
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (floor[i, j].value == 1)
                    WallBlock(i, j);
                else if (floor[i, j].value == 0)
                    WalkBlock(i, j);
            }
        }
    }

    public void ChangeSize(float s)
    {
        DestroyObjects();
        sizeX = Mathf.RoundToInt(s);
        sizeY = Mathf.RoundToInt(s);
        Create();
        Instantiate();
    }

    public void DestroyObjects()
    {
        GameObject.Destroy(GameObject.Find("Floor"));
        GameObject.Destroy(GameObject.Find("MazeGenerator(Clone)"));
        DestroyPathfind();
    }

    public void DestroyPathfind()
    {
        GameObject.Destroy(GameObject.Find("Floodfill(Clone)"));
        GameObject.Destroy(GameObject.Find("Greedy(Clone)"));
        GameObject.Destroy(GameObject.Find("AStar(Clone)"));
    }

    public void ColorBlock(int x, int y, float r, float g, float b)
    {
        ColorBlock(x, y, new Color(r / 255f, g / 255f, b /255f));
    }

    public void ColorBlock(int x, int y, Color c)
    {
        floorObjects[x, y].GetComponent<Renderer>().material.color = c;
    }

    public void WalkBlock(int x, int y)
    {
        floor[x, y].value = 0;
        ColorBlock(x, y, Color.white);
        ResizeBlock(x, y);
    }

    public void WallBlock(int x, int y)
    {
        floor[x, y].value = 1;
        ColorBlock(x, y, Color.black);
        ResizeBlock(x, y);
    }

    public void RaiseBlock(int x, int y, float h)
    {
        floor[x, y].height += h;

        if(floor[x, y].height >= 20)
            floor[x, y].height = 20;
        ResizeBlock(x, y);
        MoveBlock(x, y);
    }

    public void LowerBlock(int x, int y, float h)
    {
        floor[x, y].height -= h;
        if(floor[x, y].height <= 0) //floor height at 0
            floor[x, y].height = 0;
        ResizeBlock(x, y);
        MoveBlock(x, y);
    }

    public void ResizeBlock(int x, int y)
    {
        floorObjects[x, y].transform.localScale = new Vector3(0.92f, 0.92f + floor[x, y].value + floor[x, y].height, 0.92f);
    }

    public void MoveBlock(int x, int y)
    {
        floorObjects[x, y].transform.localPosition = new Vector3(0.5f + x * 1f, floor[x, y].height / 2 ,0.5f + y * 1f);
    }

}

