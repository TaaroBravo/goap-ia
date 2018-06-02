using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{

    State _currentState;
    List<State> _stateList = new List<State>();

    public void Update()
    {
        if (_currentState != null)
            _currentState.Execute();
    }

    public void AddState(State state)
    {
        if (!_stateList.Contains(state))
            _stateList.Add(state);
    }

    public void SetState<T>() where T : State
    {
        foreach (var newState in _stateList)
        {
            if (newState.GetType() == typeof(T))
            {
                if (_currentState != null)
                    _currentState.Sleep();
                if (_currentState != newState)
                {
                    _currentState = newState;
                    _currentState.Awake();
                }
            }
        }
    }

    public bool IsActualState<T>() where T : State
    {
        if (_currentState.GetType() == typeof(T))
            return true;
        else
            return false;
    }

    private int SearchState(Type t)
    {
        for (int i = 0; i < _stateList.Count; i++)
        {
            if (_stateList[i].GetType() == t)
                return i;
        }
        return -1;
    }

}
