using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_FleeState : A_State
{
    private NPC_A _npc;
    private GameObject _target;

    private Vector3 origin;
    private Vector3 direction;
    LayerMask layerMask = 1 << 12;

    List<GameObject> borderNodes = new List<GameObject>();

    public A_FleeState(StateMachine sm, NPC_A npc_a) : base(sm, npc_a)
    {
        _npc = npc_a;
    }

    public override void Awake()
    {
        base.Awake();
        _target = _npc.target;
        borderNodes = Camera.main.GetComponent<NodeMatrixCreator>().bordernodes;
    }

    public override void Execute()
    {
        base.Execute();
        origin = new Vector3(_npc.transform.position.x, _npc.transform.position.y - _npc.transform.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y + 0.5f, _npc.transform.position.z);
        direction = (new Vector3(_npc.transform.position.x, _npc.transform.position.y - _npc.transform.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y + 0.5f, _npc.transform.position.z) - new Vector3(_target.transform.position.x, _target.transform.position.y - _target.transform.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y + 0.5f, _target.transform.position.z)).normalized;
        RaycastHit ray;
        if (Physics.Raycast(origin, direction, out ray, 500, layerMask))
        {
            if (borderNodes.Contains(ray.collider.gameObject))
                _npc.MoveTo(ray.collider.transform.position);
        }
    }

    public override void Sleep()
    {
        base.Sleep();
    }

}
