using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
   
    public List<Node> DeepFirstSearch (Node start, Node end)
    {
        List<Node> S = new List<Node>();
        List<Node> path = new List<Node>();
        S.Add(start);
        start.visited = true;

        while (S.Count > 0)
        {
            Node V = S[S.Count-1];
            S.Remove(V);
            
            path.Add(V);

            if (V == end)
            {
                return path;
            }

            foreach (var w in V.links)
            {
                if(!w.visited)
                {
                    w.visited = true;
                    
                    S.Add(w);
                }
            }
        }
        
        return S;
	}


}
