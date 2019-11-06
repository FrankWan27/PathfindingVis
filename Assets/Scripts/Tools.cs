using System.Collections;
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

public class Node
{
    public int x;
    public int y;
    public int value;
    public Node parent;

    public Node(int a, int b)
    {
        x = a;
        y = b;
    }

    public Node(int a, int b, int v)
    {
        x = a;
        y = b;
        value = v;
    }

    public Node(int a, int b, Node n)
    {
        x = a;
        y = b;
        parent = n;
    }

    public Coord GetCoord()
    {
        return new Coord(x, y);
    }
}
