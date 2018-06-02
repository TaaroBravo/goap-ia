using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {

    StateMachine _sm;

    public State(StateMachine sm)
    {
        _sm = sm;
    }

    public virtual void Awake()
    {

    }

    public virtual void Execute()
    {

    }

    public virtual void Sleep()
    {

    }


}
