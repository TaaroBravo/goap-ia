using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Follower : NPC_A
{

    public List<Collider> npcs_a = new List<Collider>();
    float flockRadius = 5;
    float friendsRadius = 500;
    public Vector3 dir;

    private Vector3 _vecCohesion;
    private Vector3 _vecAlineacion;
    private Vector3 _vecSeparacion;
    private Vector3 _vecLider;

    public float coefCohesion;
    public float coefAlineacion;
    public float coefSeparacion;
    public float coefLider;

    private float _viewAngle;
    private float _viewDistance;

    private Vector3 direction;
    private float _angle;
    private float _distanceTarget;
    private float _timer;
    private bool _targetInSight;
    public float fireRate;
    private float _timerFireRate;

    private GameObject bulletPrefab;
    private Transform startBulletPosition;
    List<GameObject> bulletsList = new List<GameObject>();

    public override void Start()
    {
        base.Start();
        coefCohesion = 5;
        coefAlineacion = 5;
        coefSeparacion = 5;
        coefLider = 5f;
        A_ArrayNPC.Add(this);
        bulletPrefab = GameObject.Find("A_Manager").GetComponent<A_Manager>().bulletPrefab;
        _viewDistance = 7f;
        _viewAngle = 50f;
        fireRate = 1;
    }

    public override void Update()
    {
        base.Update();
        npcs_a.Clear();
        Collider[] allCol = Physics.OverlapSphere(transform.position, friendsRadius);
        foreach (var coll in allCol)
        {
            if (coll.transform.gameObject.tag == "NPC_A")
                npcs_a.Add(coll);
        }
        npcs_a.Remove(GetComponent<Collider>());

        foreach (var bullet in Physics.OverlapSphere(transform.position, 0.5f))
        {
            if (bullet.gameObject.tag == "BulletB")
            {
                Destroy(bullet.gameObject);
                life -= 7;
            }
        }

        if (lider)
            Flock();
        else
            lider = GameObject.Find("NPC_A@Lider");

        if (_timerFireRate < fireRate)
            _timerFireRate += Time.deltaTime;

        if (attack && target)
        {
            if (_timerFireRate > fireRate)
            {
                direction = (target.transform.position - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, direction, rotationSpeed * Time.deltaTime);

                _angle = Vector3.Angle(transform.forward, direction);

                _distanceTarget = Vector3.Distance(transform.position, target.transform.position);

                if (_angle <= _viewAngle && _distanceTarget <= _viewDistance)
                {
                    RaycastHit ray;
                    bool obstaclesBetween = false;

                    if (Physics.Raycast(transform.position, direction, out ray, _distanceTarget))
                        if (ray.collider.gameObject.layer == Layers.OBSTACULE)
                            obstaclesBetween = true;

                    if (!obstaclesBetween)
                        _targetInSight = true;
                    else
                        _targetInSight = false;
                }
                else
                    _targetInSight = false;
                if (_targetInSight)
                    Shoot();
                _timerFireRate = 0;
            }
        }

    }

    void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = transform.position + (transform.position - target.transform.position).normalized / 2;
        bullet.transform.up = target.transform.position - bullet.transform.position;
        bullet.GetComponent<Bullet>().targetsName = "NPC_B";
        bullet.tag = "BulletA";
        bullet.GetComponent<Bullet>().target = target;
        bullet.GetComponent<Bullet>().myshooter = gameObject;
        bullet.GetComponent<Bullet>().Myshooterpos = transform.position;
        bullet.GetComponent<Bullet>().dir = (target.transform.position - transform.position).normalized;
        bullet.GetComponent<Bullet>().Targetposition = target.transform.position;
    }

    #region Flock
    void Flock()
    {
        _vecAlineacion = Alienacion() * coefAlineacion;
        _vecCohesion = Cohesion() * coefCohesion;
        _vecSeparacion = Separacion() * coefSeparacion;
        _vecLider = Lider() * coefLider;

        dir = _vecAlineacion + _vecCohesion + _vecLider + _vecSeparacion + _vecAvoide;

        transform.forward = Vector3.Slerp(transform.forward, dir, rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    Vector3 Cohesion()
    {
        Vector3 c = new Vector3();
        foreach (var bird in npcs_a)
            c += (bird.transform.position - transform.position);
        return c /= npcs_a.Count;
    }
    Vector3 Alienacion()
    {
        Vector3 a = new Vector3();
        foreach (var bird in npcs_a)
            a += bird.transform.forward;
        return a /= npcs_a.Count;
    }
    Vector3 Separacion()
    {
        Vector3 s = new Vector3();
        foreach (var bird in npcs_a)
        {
            Vector3 f = new Vector3();
            f = transform.position - bird.transform.position;
            float mag = flockRadius - f.magnitude;
            f.Normalize();
            f *= mag;
            s += f;
        }
        return s /= npcs_a.Count;
    }
    Vector3 Lider()
    {
        return lider.transform.position - transform.position;
    }
    #endregion

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.transform.tag == "BulletB")
            Destroy(this.gameObject);
    }

}
