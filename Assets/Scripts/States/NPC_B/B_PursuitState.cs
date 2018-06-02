using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class B_PursuitState : B_State
{
    private NPC_B _npc;
    private GameObject _target;

    public B_PursuitState(StateMachine sm, NPC_B npc_b) : base(sm, npc_b)
    {
        _npc = npc_b;
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
    }

    public override void Sleep()
    {
        base.Sleep();
    }

}
