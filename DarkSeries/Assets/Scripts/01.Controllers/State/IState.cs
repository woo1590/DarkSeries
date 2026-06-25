using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class IState<T>
{
    public StateMachine<T> stateMachine { get; private set; }

    protected IState(StateMachine<T> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit() ;
    public abstract void HandleInput() ;
    public abstract void Update();
    public abstract void LateUpdate();
    public abstract void FixedUpdate();
}
