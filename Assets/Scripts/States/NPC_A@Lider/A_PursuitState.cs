using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_PursuitState : A_State
{

    private NPC_A _npc;
    private GameObject _target;

    public A_PursuitState(StateMachine sm, NPC_A npc_a) : base(sm, npc_a)
    {
        _npc = npc_a;
    }

    public override void Awake()
    {
        base.Awake();
        _target = _npc.target;
    }

    public override void Execute()
    {
        base.Execute();
        if(_target)
            _npc.MoveTo(_target.transform.position);
        else
            _target = _npc.target;
    }

    public override void Sleep()
    {
        base.Sleep();
    }
}
