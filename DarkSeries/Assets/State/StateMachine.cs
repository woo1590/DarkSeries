using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class StateMachine<T>
{
    protected IState currState;

    public void ChangeState(IState newState)
    {
        currState?.Exit();
        currState = newState;
        currState.Enter();
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