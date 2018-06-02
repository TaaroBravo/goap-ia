using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> links = new List<Node>();
    public bool visited;

    public Node father;
    public List<Node> neighbours;
    public List<Node> visibleneighbours;
    public float index;
    public float G;
    public float H;
    public float F;
    public bool isStart;
    public bool isEnd;
    public bool isWalkable;
    public bool isBlocked;

    void Start()
    {
        foreach (var item in Camera.main.GetComponent<NodeMatrixCreator>().allnodes)
            links.Add(item.GetComponent<Node>());
    }

    void Update()
    {
        if (isEnd)
            gameObject.tag = "EndNode";
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (isWalkable)
            Gizmos.DrawWireSphere(transform.position, 1);

        Gizmos.color = Color.blue;
        if (isStart)
            Gizmos.DrawWireSphere(transform.position, 1);

        Gizmos.color = Color.blue;
        if (isEnd)
            Gizmos.DrawWireSphere(transform.position, 1);

        Gizmos.color = Color.red;
        if (isBlocked)
            Gizmos.DrawWireSphere(transform.position, 1);

        //Gizmos.color = Color.white;
        //foreach (var item in visibleneighbours)
        //    if (item)
        //        Gizmos.DrawLine(transform.position, item.transform.position);
    }
}
