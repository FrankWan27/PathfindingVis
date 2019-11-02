using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public int[,] floor;
    public GameObject[,] floorObjects;
    public GameObject floorTile;
    public GameObject mazeGen;
    public int sizeX;
    public int sizeY;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create()
    {
        floor = new int[sizeX, sizeY];
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
                GameObject tile = Instantiate(floorTile, new Vector3(i * 1.1f, 0, j * 1.1f), Quaternion.identity);
                floorObjects[i, j] = tile;
                floor[i, j] = 0;
                
                tile.GetComponent<Renderer>().material.color = Color.white;
                
                tile.transform.parent = parent.transform;
            }
        }
    }

    public void GenerateMaze()
    {
        DestroyObjects();
        //Using Recursive Backtracking Algorithm
        //http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking
        Instantiate();
        AllWallFloor();
        MazeGenerator mg = GameObject.Instantiate(mazeGen, Vector3.zero, Quaternion.identity).GetComponent<MazeGenerator>();

        mg.Generate();
    }

    public void AllWallFloor()
    {
        for (int i = 0; i < floor.GetLength(0); i++)
        {
            for (int j = 0; j < floor.GetLength(1); j++)
            {
                floorObjects[i, j].GetComponent<Renderer>().material.color = Color.black;
                floorObjects[i, j].transform.localScale = new Vector3(1, 2, 1);
            }
        }
    }

    public void changeSize(float s)
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

    }
}

