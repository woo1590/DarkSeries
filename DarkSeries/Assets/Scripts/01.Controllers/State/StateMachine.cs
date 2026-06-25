using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public T owner { get; private set; }
    protected IState<T> currState;
    Dictionary<Type,IState<T>> states = new();

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void ChangeState<TState>()
    {
        Type type = typeof(TState);

        if(states.TryGetValue(type,out IState<T> nextState))
        {
            currState?.Exit();
            currState = nextState;
            currState.Enter();
        }
        else
        {
            Debug.LogError($"{typeof(TState).Name} not exist");
            return;
        }
    }

    public void AddState(IState<T> state)
    {
        states[state.GetType()] = state;
    }

    public void HandleInput()
    {
        currState.HandleInput();
    }
    public void Update()
    {
        currState.Update();
    }

    public void LateUpdate()
    {
        currState.LateUpdate();
    }
    public void FixedUpdate()
    {
        currState.FixedUpdate();
    }
}