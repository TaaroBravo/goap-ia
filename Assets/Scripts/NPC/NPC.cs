using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private DFS _dfs;
    private Astar _pathfinding;
    public Node nodeNearTarget;

    private bool _blockObstacles;
    public float timerReCalculatePathFinding;

    private bool _posiblePath;

    public virtual void Awake()
    {
        _pathfinding = new Astar();
        _dfs = new DFS();
        timerReCalculatePathFinding = 4;
    }

    int currentIndex = 0;
    public Node[] AstarPath;

    public virtual void Update()
    {
        if (AstarPath == null || currentIndex == AstarPath.Length)
        {
            return;
        }
        float dist = 0;
        if (AstarPath[currentIndex] != null)
            dist = Vector3.Distance(AstarPath[currentIndex].transform.position, transform.position);
        if (dist >= 1)
        {
            transform.LookAt(AstarPath[currentIndex].transform.position);
            transform.position += transform.forward * 2 * Time.deltaTime;
        }
        else
            currentIndex++;
        if (timerReCalculatePathFinding > 0)
        {
            timerReCalculatePathFinding -= Time.deltaTime;
        }

        if (FindNearNode(transform.position).isBlocked)
        {
            transform.position += transform.right * 0.5f;
        }
    }

    void LateUpdate()
    {
        if (!_blockObstacles)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                Vector3 position = new Vector3(item.transform.position.x, item.transform.position.y - item.transform.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y, item.transform.position.z);
                CalculateTargetNode(position).isBlocked = true;
            }
            _blockObstacles = true;
        }
    }

    public void MoveTo(Vector3 endPos)
    {
        _posiblePath = CalculatePathDFS(endPos);
        if (_posiblePath)
        {
            if (timerReCalculatePathFinding <= 0)
            {
                currentIndex = 0;
                AstarPath = null;
                AstarPath = _pathfinding.SearchPath(FindNearNode(transform.position),
                                                   FindNearNode(endPos));
                timerReCalculatePathFinding = 5f;
            }
            else if (AstarPath != null)
            {
                if (currentIndex == AstarPath.Length - 1)
                {
                    currentIndex = 0;
                    timerReCalculatePathFinding = 0;
                }
                else
                {
                    Node newNode = FindNearNode(endPos);
                    List<Node> newPath = new List<Node>();
                    for (int i = 0; i < AstarPath.Length; i++)
                        newPath.Add(AstarPath[i]);
                    if (!newPath.Contains(newNode))
                    {
                        newPath.Add(newNode);
                        foreach (var item in GameObject.FindGameObjectsWithTag("EndNode"))
                            if (item.GetComponent<Node>() != null)
                            {
                                item.GetComponent<Node>().isEnd = false;
                                item.tag = "Untagged";
                            }
                        newNode.isEnd = true;
                        newNode.isWalkable = true;
                    }
                    AstarPath = newPath.ToArray();
                }
            }
        }
    }

    bool CalculatePathDFS(Vector3 endPos)
    {
        if (endPos != null)
        {
            var dfs = _dfs.DeepFirstSearch(FindNearNode(transform.position), FindNearNode(endPos));
            if (dfs != null)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public Node CalculateTargetNode(Vector3 target)
    {
        return FindNearNode(target);
    }

    private Node FindNearNode(Vector3 pos)
    {
        var nod = Physics.OverlapSphere(pos, 2);
        var nodos = new List<Node>();
        foreach (var item in nod)
            if (item.GetComponent<Node>())
                nodos.Add(item.GetComponent<Node>());

        float dist = Mathf.Infinity;
        Node closest = null;
        foreach (var item in nodos)
        {
            var ds = Vector3.Distance(pos, item.transform.position);
            if (ds < dist)
            {
                dist = ds;
                closest = item;
            }
        }
        return closest;
    }
}
