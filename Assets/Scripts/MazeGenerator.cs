using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    FloorManager fm;
    GameObject[,] floorObjects;
    List<Coord> past;
    int i = 0;
    bool finished = false;

    private void Start()
    {
        fm = GameObject.Find("GameManager").GetComponent<FloorManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (finished && i < past.Count)
        {
            //go by two
            floorObjects[past[i].x, past[i].y].GetComponent<Renderer>().material.color = Color.white;
            floorObjects[past[i].x, past[i].y].transform.localScale = Vector3.one * 0.9f;
            floorObjects[past[i + 1].x, past[i + 1].y].GetComponent<Renderer>().material.color = Color.white;
            floorObjects[past[i + 1].x, past[i + 1].y].transform.localScale = Vector3.one * 0.9f;
            i += 2;
        }
        if (finished && i >= past.Count)
        {
            GameObject.Destroy(this.gameObject);
        }

    }

    public void Generate()
    {
        finished = false;
        //starting pos needs to be even
        fm = GameObject.Find("GameManager").GetComponent<FloorManager>();
        floorObjects = fm.floorObjects;

        int startX = Random.Range(0, fm.floor.GetLength(0) / 2) * 2;
        int startY = Random.Range(0, fm.floor.GetLength(1) / 2) * 2;
        past = new List<Coord>();

        CarvePath(startX, startY);
        finished = true;
    }

    private void CarvePath(int currX, int currY)
    {
        List<char> directions = new List<char>();
        directions.Add('N');
        directions.Add('S');
        directions.Add('E');
        directions.Add('W');
        Tools.Shuffle<char>(directions);

        foreach (char d in directions)
        {
            int dx = 0, dy = 0;
            switch (d)
            {
                case 'N':
                    dx = 0;
                    dy = 1;
                    break;
                case 'S':
                    dx = 0;
                    dy = -1;
                    break;
                case 'E':
                    dx = 1;
                    dy = 0;
                    break;
                case 'W':
                    dx = -1;
                    dy = 0;
                    break;
            }

            int newX = currX + 2 * dx;
            int newY = currY + 2 * dy;

            if (newX >= 0 && newX < fm.floor.GetLength(0) && newY >= 0 && newY < fm.floor.GetLength(1)) //if in bounds
            {
                if (fm.floor[newX, newY].value == 1) //if unvisited tile
                {
                    fm.floor[newX, newY].value = 0; //breakdown tile
                    fm.floor[newX - dx, newY - dy].value = 0; //breakdown wall
                    past.Add(new Coord(newX - dx, newY - dy));
                    past.Add(new Coord(newX, newY));
                    CarvePath(newX, newY);
                    
                }
            }
        }

    }
}
