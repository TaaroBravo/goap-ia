using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_ShootState : A_State {

    private NPC_A _npc;
    private GameObject _target;

    private float _viewAngle;
    private float _viewDistance;

    private Vector3 direction;
    private float _angle;
    private float _distanceTarget;
    private float _timer;
    private bool _targetInSight;

    private GameObject bulletPrefab;
    private Transform startBulletPosition;
    List<GameObject> bulletsList = new List<GameObject>();

    public A_ShootState(StateMachine sm, NPC_A npc_a) : base(sm, npc_a)
    {
        _npc = npc_a;
    }

    public override void Awake()
    {
        bulletPrefab = GameObject.Find("A_Manager").GetComponent<A_Manager>().bulletPrefab;
        _viewDistance = 7f;
        _viewAngle = 50f;
        _target = _npc.target;
        base.Awake();
    }

    public override void Execute()
    {
        base.Execute();
        if (_target)
        {
            direction = (_target.transform.position - _npc.transform.position).normalized;

            _npc.transform.forward = Vector3.Lerp(_npc.transform.forward, direction, _npc.rotationSpeed * Time.deltaTime);

            _angle = Vector3.Angle(_npc.transform.forward, direction);

            _distanceTarget = Vector3.Distance(_npc.transform.position, _target.transform.position);

            if (_angle <= _viewAngle && _distanceTarget <= _viewDistance)
            {
                RaycastHit ray;
                bool obstaclesBetween = false;

                if (Physics.Raycast(_npc.transform.position, direction, out ray, _distanceTarget))
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
            
        }
    }

    public override void Sleep()
    {
        base.Sleep();
    }

    private void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = _npc.transform.position + (_npc.transform.position - _target.transform.position).normalized / 2;
        bullet.transform.up = _target.transform.position - bullet.transform.position;
        bullet.GetComponent<Bullet>().targetsName = "NPC_B";
        bullet.tag = "BulletA";
        bullet.GetComponent<Bullet>().target = _target;
        bullet.GetComponent<Bullet>().myshooter = _npc.gameObject;
        bullet.GetComponent<Bullet>().Myshooterpos = _npc.transform.position;
        bullet.GetComponent<Bullet>().dir = (_target.transform.position - _npc.transform.position).normalized;
        bullet.GetComponent<Bullet>().Targetposition = _target.transform.position;

    }
}
