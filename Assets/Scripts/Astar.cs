using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Astar
{
    private List<Node> _openSet;
    private List<Node> _closedSet;

    public Astar()
    {
        _openSet = new List<Node>();
        _closedSet = new List<Node>();
    }

    public Node[] SearchPath(Node start, Node end)
    {
        var reset = GameObject.FindObjectsOfType<Node>();
        foreach (var item in reset)
        {
            item.isStart = false;
            item.isEnd = false;
            item.isWalkable = false;
            item.index = Mathf.Infinity;
        }
        start.isStart = true;
        end.isEnd = true;
        _openSet.Clear();
        _closedSet.Clear();

        _openSet.Add(start);

        start.SetG(start).SetH(end).SetF();

        Node C;
        while (_openSet.Count > 0)
        {
            C = BuscarFMasBajo();
            if (C == end)
                return ThetaStar(ReconstruirCamino(start, C));

            foreach (var V in C.neighbours)
            {
                if (V.isBlocked)
                {
                    _closedSet.Add(V);
                    V.H = Mathf.Infinity;
                    V.G = Mathf.Infinity;
                    V.F = Mathf.Infinity;
                    continue;
                }
                var g = (Vector3.Distance(C.transform.position, V.transform.position)) + C.G;
                if (_openSet.Contains(V))
                {
                    if (V.G <= g)
                        continue;
                }
                else if (_closedSet.Contains(V))
                {
                    if (V.G <= g)
                        continue;
                    _openSet.Add(V);
                    _closedSet.Remove(V);
                }
                else
                {
                    _openSet.Add(V);
                    V.SetH(end);
                }
                V.G = g;
                V.father = C;
                V.SetF();
            }
            _closedSet.Add(C);
            _openSet.Remove(C);
            if (C == end)
                return ThetaStar(ReconstruirCamino(start, C));
        }
        return null;
    }


    private Node[] ThetaStar(Node[] AstarPath)
    {

        foreach (var n1 in AstarPath)
        {
            n1.visibleneighbours = new List<Node>();
            foreach (var n2 in AstarPath)
            {
                Ray ray = new Ray(n1.transform.position, (n2.transform.position - n1.transform.position).normalized);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 500))
                    if (hit.collider.gameObject == n2.gameObject)
                        n1.visibleneighbours.Add(n2);
            }
        }

        foreach (var node in AstarPath)
            node.ReComputePath();

        List<Node> Camino = new List<Node>();
        if(AstarPath.Length > 1)
        {
            Node c = AstarPath[1];
            while (c != null)
            {
                if (!Camino.Contains(c))
                {
                    Camino.Add(c);
                    c = c.father;
                }
                else {
                    break;
                }
            }
            Camino.Add(c);
        }
        return Camino.ToArray();
    }

    private Node BuscarFMasBajo()
    {
        float f = float.MaxValue;
        int index = -1;
        for (int i = 0; i < _openSet.Count; i++)
        {
            if (!_openSet[i])
                continue;
            if (_openSet[i].H == 0)
            {
                index = i;
                break;
            }
            if (_openSet[i].F < f)
            {
                f = _openSet[i].F;
                index = i;
            }
        }

        return _openSet[index];
    }

    private Node[] ReconstruirCamino(Node start, Node end)
    {
        var C = end;
        List<Node> Camino = new List<Node>();
        while (C != start)
        {
            Camino.Add(C);
            C = C.father;
        }

        List<Node> path = new List<Node>();
        for (int i = Camino.Count - 1; i >= 0; i--)
        {
            path.Add(Camino[i]);
            Camino[i].index = i;
        }
        foreach (var item in path)
            item.isWalkable = true;

        return path.ToArray();
    }
}

public static class NodeFunctions
{
    public static Node SetF(this Node node)
    {
        node.F = node.G + node.H;
        return node;
    }

    public static Node SetG(this Node node, Node current)
    {
        node.G = (Vector3.Distance(current.transform.position, node.transform.position)) + current.G;
        return node;
    }

    public static Node SetH(this Node node, Node end)
    {
        var distX = end.transform.position.x - node.transform.position.x;
        var distZ = end.transform.position.z - node.transform.position.z;
        distX = distX < 0 ? -distX : distX;
        distZ = distZ < 0 ? -distZ : distZ;
        node.H = distX + distZ;
        return node;
    }


    public static void ReComputePath(this Node node)
    {
        if (node.isEnd)
        {
            node.father = null;
            return;
        }
        foreach (var vecino in node.visibleneighbours)
            if (vecino.index < node.father.index)
                node.father = vecino;
    }
}