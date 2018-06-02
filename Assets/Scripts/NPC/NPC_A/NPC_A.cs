using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_A : NPC
{
    public GameObject target;

    public float life = 20f;
    public static List<NPC_A> A_ArrayNPC = new List<NPC_A>();

    public float speed = 1.8f;
    public float rotationSpeed = 6f;
    public GameObject lider;
    public Vector3 _vecAvoide;
    private float _coefAvoid;

    public float yPos;

    public static bool attack;

    public LayerMask LayerObst = 1 << 11;
    public List<Collider> obstacles;
    public Collider closeObj;
    public float radiusObst = 2.5f;
    public float timerRecalculateEnemy;

    StateMachine _sm;

    public virtual void Start () {
        target = GameObject.Find("NPC_B");
        lider = GameObject.Find("NPC_A@Lider");
        yPos = transform.position.y;
        _coefAvoid = 10;
    }

    public override void Update () {
        base.Update();
        if (!target)
            target = GameObject.Find("NPC_B");
        timerRecalculateEnemy += Time.deltaTime;
        if(timerRecalculateEnemy > 2)
        {
            target = GameObject.Find("NPC_B");
            timerRecalculateEnemy = 0;
        }

        SetObstacles();
        closeObj = CloserObj();
        _vecAvoide = Avoid() * _coefAvoid;

        if (life <= 0)
            Destroy(gameObject);

        foreach (var bullet in Physics.OverlapSphere(transform.position, 0.5f))
        {
            if (bullet.gameObject.tag == "BulletB")
            {
                Destroy(bullet.gameObject);
                life -= 7;
            }
        }

        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    Vector3 Avoid()
    {
        if (closeObj)
            return transform.position - closeObj.transform.position;
        else
            return Vector3.zero;
    }

    void SetObstacles()
    {
        if (obstacles != null)
            obstacles.Clear();
        else
            obstacles = new List<Collider>();
        foreach (var obs in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (Vector3.Distance(transform.position, obs.transform.position) <= radiusObst)
                obstacles.Add(obs.GetComponent<Collider>());
        }
    }

    Collider CloserObj()
    {
        if (obstacles.Count > 0)
        {
            Collider closer = null;
            float dist = 10000;
            foreach (var obs in obstacles)
            {
                var newDist = Vector3.Distance(obs.transform.position, transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    closer = obs;
                }
            }
            return closer;
        }
        else
            return null;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.tag == "BulletB")
            life -= 4f;
    }
}
