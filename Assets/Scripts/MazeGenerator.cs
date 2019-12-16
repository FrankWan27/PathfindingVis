﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    FloorManager fm;
    GameObject[,] floorObjects;
    List<Coord> past;
    int i = 0;
    bool finished = false;
    int option = 0;
    float h = 4; //Height of Tall Maze

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
            Debug.Log("WOO");
            if(option == 0)
            {
                fm.WalkBlock(past[i].x, past[i].y);
                fm.WalkBlock(past[i + 1].x, past[i + 1].y);
            }
            else if(option == 1)
            {
                fm.LowerBlock(past[i].x, past[i].y, h);
                fm.LowerBlock(past[i + 1].x, past[i + 1].y, h);
            }
            i += 2;
        }
        if (finished && i >= past.Count)
        {
            GameObject.Destroy(this.gameObject);
        }

    }

    public void Generate(int opt)
    {
        option = opt;
        //option = 0, generate Wall Maze
        //option = 1, generate Tall Maze
        finished = false;
        fm = GameObject.Find("GameManager").GetComponent<FloorManager>();

        if(option == 0)
            fm.AllWallFloor();
        else if(option == 1)
            fm.AllTallFloor(h);
        
        floorObjects = fm.floorObjects;

        //starting pos needs to be even
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
                if(option == 0)
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
                else if(option == 1)
                {
                    if (fm.floor[newX, newY].height > 0) //if unvisited tile
                    {
                        fm.floor[newX, newY].height = 0; //breakdown tile
                        fm.floor[newX - dx, newY - dy].height = 0; //breakdown wall
                        past.Add(new Coord(newX - dx, newY - dy));
                        past.Add(new Coord(newX, newY));
                        CarvePath(newX, newY);
                    }
                }
            }
        }

    }
}
