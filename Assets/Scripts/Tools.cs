﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Coord
{
    public int x;
    public int y;

    public Coord(int a, int b)
    {
        x = a;
        y = b;
    }
}
