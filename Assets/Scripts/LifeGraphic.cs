using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGraphic : MonoBehaviour {

    public GameObject camera;
    public List<Sprite> lifesprites = new List<Sprite>();
    public SpriteRenderer spr;
    public GameObject mynpc;
    public float life;
	void Start () {
        spr = GetComponent<SpriteRenderer>();
    }
	void Update () {
        if (mynpc.GetComponent<NPC_B>()) life = mynpc.GetComponent<NPC_B>().life;
        else if (mynpc.GetComponent<A_Follower>()) life = mynpc.GetComponent<A_Follower>().life;
        else life = mynpc.GetComponent<A_Lider>().life;
        transform.position = mynpc.transform.position + new Vector3(0, 2,0);
        transform.forward = ((camera.transform.position - transform.position).normalized);
        if (life > 8) spr.sprite = lifesprites[Mathf.RoundToInt(life / 5)];
        else spr.sprite = lifesprites[0];
	}
}
