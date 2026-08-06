using System;
public abstract class IState<T>
{
    public StateMachine<T> stateMachine { get; private set; }

    protected IState(StateMachine<T> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract void LateUpdate();
    public abstract void FixedUpdate();
    
    /* Call back */
    public virtual void OnAnimationEnd() { }
}
