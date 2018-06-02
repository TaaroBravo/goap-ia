using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_State : State {

    public NPC_B npc_b;

    public B_State(StateMachine sm, NPC_B _npc_b) : base(sm)
    {
        npc_b = _npc_b;
    }
}
