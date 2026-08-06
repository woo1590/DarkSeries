using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public T owner { get; private set; }
    public IState<T> currState { get; private set; }
    private readonly Dictionary<Type, IState<T>> states = new();

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public bool ChangeState<TState>()
    {
        Type type = typeof(TState);

        if (!states.TryGetValue(type, out IState<T> nextState))
        {
            Debug.LogError($"{type.Name} not exist");
            return false;
        }

        if (ReferenceEquals(currState, nextState))
            return false;

        currState?.Exit();
        currState = nextState;
        currState.Enter();
        return true;
    }

    public void AddState(IState<T> state)
    {
        states[state.GetType()] = state;
    }

    public void Update()
    {
        currState?.Update();
    }

    public void LateUpdate()
    {
        currState?.LateUpdate();
    }
    public void FixedUpdate()
    {
        currState?.FixedUpdate();
    }

    public void OnAnimationEnd()
    {
        currState?.OnAnimationEnd();
    }
}
