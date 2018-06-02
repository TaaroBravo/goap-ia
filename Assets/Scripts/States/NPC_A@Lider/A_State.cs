using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_State : State {

    public NPC_A npc_a;

    public A_State(StateMachine sm, NPC_A _npc_a) : base(sm)
    {
        npc_a = _npc_a;
    }
}
