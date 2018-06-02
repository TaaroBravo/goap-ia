using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject target;
    public Vector3 dir;
    public string targetsName;
    public float speed;
    public float destroytimer;
    public GameObject myshooter;
    public Vector3 Myshooterpos;
    public Vector3 Targetposition;
    public Vector3 GAshoot;
    public bool myshooterdied;
    public bool lastbreath;
    public float distancetotarget;
    public bool ihavebeenavoided;
    void Start()
    {
        speed = 5;
    }
    void Update()
    {
        if (Targetposition == Vector3.zero && target)
            Targetposition = target.transform.position;

        if (target)
        {
            if (target.GetComponent<NPC_A>() != null)
                targetsName = "NPC_A";
            else if (target.GetComponent<NPC_B>() != null)
                targetsName = "NPC_B";
        }

        distancetotarget = (Vector3.Distance(Targetposition, Myshooterpos)
            - Vector3.Distance(Targetposition, transform.position)) / Vector3.Distance(Targetposition, Myshooterpos) * 100f;
        transform.position += (dir + GAshoot) * speed * Time.deltaTime;
        destroytimer += Time.deltaTime;
        if (destroytimer > 2f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.tag == targetsName)
        {
            if (col.transform.GetComponent<NPC_A>() != null)
            {
                if (GameObject.FindGameObjectsWithTag("NPC_A").Length == 1)
                    Destroy(col.transform.gameObject);
            }
            else if (col.transform.GetComponent<NPC_B>() != null)
            {
                if (GameObject.FindGameObjectsWithTag("NPC_B").Length == 1)
                    Destroy(col.transform.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

}
