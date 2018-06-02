using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using Random = UnityEngine.Random;

public class A_Manager : MonoBehaviour
{

    public static GameObject npc_Lider;
    public static GameObject[] npc_a;
    public GameObject bulletPrefab;

    void Update()
    {
        //npc_a = GameObject.FindGameObjectsWithTag("NPC_A");
        //if (npc_a.Any() && !npc_Lider)
        //{

        //    npc_Lider = npc_a.Aggregate(Tuple.Create(new List<GameObject>(), Random.Range(0, npc_a.Count()), 0), (x, y) =>
        //    {
        //        if (x.Item3 == x.Item2)
        //            x.Item1.Add(y);
        //        return Tuple.Create(x.Item1, x.Item2, x.Item3 + 1);
        //    }).Item1.First();

        //    npc_a.Select(x => x.GetComponent<NPC_A>()).Select(x => x.GetComponent<A_Follower>() ? Destroy(x.GetComponent<A_Follower>()) : Destroy(x.GetComponent<A_Lider>()));

        //    npc_a.Where(x => x == npc_Lider).Select(x =>
        //    {
        //        x.AddComponent(typeof(A_Lider));
        //        x.name = "NPC_A@Lider";
        //    });

        //    npc_a.Where(x => x != npc_Lider).Select(x =>
        //    {
        //        x.AddComponent(typeof(A_Follower));
        //        x.name = "NPC_A@Follower";
        //    });
        //}

        //Select X
        //Where X 
        //Aggregate X 
        //OrderBy X
        //SelectMany o Concat(elija una o ambas)
        //Zip
        //Take, TakeWhile, Skip o SkipWhile(elija una o varias)


        if (!npc_Lider && npc_a.Length != 0)
        {
            npc_Lider = npc_a[Random.Range(0, npc_a.Length)];
            foreach (var npc in npc_a)
            {
                if (npc.GetComponent<A_Follower>() != null)
                    Destroy(npc.GetComponent<A_Follower>());
                else if (npc.GetComponent<A_Lider>() != null)
                    Destroy(npc.GetComponent<A_Lider>());
                if (npc == npc_Lider)
                {
                    npc.AddComponent(typeof(A_Lider));
                    npc.name = "NPC_A@Lider";
                }
                else
                {
                    npc.name = "NPC_A@Follower";
                    npc.AddComponent(typeof(A_Follower));
                }
            }
        }
    }
}
