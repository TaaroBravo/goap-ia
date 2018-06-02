using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMatrixCreator : MonoBehaviour
{

    private int _xlength;
    private int _ylength;
    public int treeammount;
    private List<int> _xmatrix = new List<int>();
    private List<List<int>> _ymatrix = new List<List<int>>();
    public GameObject tree;
    public GameObject startpositionA;
    public GameObject startpositionB;
    public GameObject startpositionC;
    public GameObject node;
    public GameObject GAManager;
    public List<GameObject> allnodes;
    public List<GameObject> bordernodes;
    public NPC npc;

    void Start()
    {
        if (treeammount == 0) treeammount = 8;
        _xlength = 15;
        _ylength = 15;

        npc = new NPC();
        
        CreateMatrix();
        CreateNodes();
        NodesFindsNeighbours();
        CreateProps();
    }

    void CreateMatrix()
    {
        for (int i = 0; i < _xlength; i++)
        {
            _ymatrix.Add(new List<int>(_xmatrix));
            for (int e = 0; e < _ylength; e++)
            {
                _ymatrix[i].Add(new int());
            }
        }
    }

    void CreateNodes()
    {
        for (int i = 0; i < _ymatrix.Count; i++)
        {
            for (int e = 0; e < _ymatrix[i].Count; e++)
            {
                var nodo = Instantiate(node, GameObject.Find("Grid").transform);
                nodo.name = "node" + i + e;
                nodo.transform.position = new Vector3(i * 2.5f, 0, e * 2.5f);
                allnodes.Add(nodo);
                if (i == 0 && !bordernodes.Contains(nodo))
                {
                    bordernodes.Add(nodo);
                    nodo.layer = Layers.BORDER;
                }
                if (i == _ymatrix.Count - 1 && !bordernodes.Contains(nodo))
                {
                    bordernodes.Add(nodo);
                    nodo.layer = Layers.BORDER;
                }
                if (e == 0 && !bordernodes.Contains(nodo))
                {
                    bordernodes.Add(nodo);
                    nodo.layer = Layers.BORDER;
                }
                if (e == _ymatrix[i].Count - 1 && !bordernodes.Contains(nodo))
                {
                    bordernodes.Add(nodo);
                    nodo.layer = Layers.BORDER;
                }
            }
        }
    }

    void NodesFindsNeighbours()
    {
        foreach (var nodo in allnodes)
        {
            foreach (var nearnode in Physics.OverlapSphere(nodo.transform.position, 2.8f))
                if (nearnode.GetComponent<Node>()) nodo.GetComponent<Node>().neighbours.Add(nearnode.GetComponent<Node>());
            nodo.GetComponent<Node>().neighbours.Remove(nodo.GetComponent<Node>());
        }
    }
    void CreateProps()
    {
        for (int i = 0; i < treeammount; i++)
        {
            var treeprop = Instantiate(tree);
            List<GameObject> nodesWithoutBorders = allnodes;
            for (int e = 0; e < allnodes.Count; e++)
            {
                if (bordernodes.Contains(nodesWithoutBorders[e]))
                    nodesWithoutBorders.RemoveAt(e);
            }
            treeprop.transform.position = npc.CalculateTargetNode(nodesWithoutBorders[Random.Range(0, nodesWithoutBorders.Count)].transform.position).transform.position;
            treeprop.layer = Layers.OBSTACULE;
            treeprop.tag = "Obstacle";
        }
    }
}
