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
    public float heuristic;
    public Node parent;
    public Node next;

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

public class MinHeap
{
    private Node head;

    public bool HasNext()
    {
        return this.head != null;
    }

    public void Insert(Node n)
    {
        if(this.head == null)
        {
            this.head = n;
        }
        else if(n.heuristic < this.head.heuristic)
        {
            n.next = this.head;
            this.head = n;
        }
        else
        {
            Node cur = this.head;
            while(cur.next != null && cur.next.heuristic <= n.heuristic)
            {
                cur = cur.next;
            }
            n.next = cur.next;
            cur.next = n;
        }
    }

    public Node Pop()
    {
        Node n = this.head;
        this.head = this.head.next;

        return n;
    }

    public void Clear()
    {
        this.head = null;
    }
}


