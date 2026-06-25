using System.Buffers;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMaster_Idle : SwordMaster_BaseState
{
    public SwordMaster_Idle(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMasterAnimData animData = stateMachine.owner.animationData;
        stateMachine.owner.animator.SetBool(animData.idleParamHash,true);
    }

    public override void Exit()
    {
        base.Exit();

        SwordMasterAnimData animData = stateMachine.owner.animationData;
        stateMachine.owner.animator.SetBool(animData.idleParamHash, false);
    }

    public override void FixedUpdate()
    {
    }

    public override void HandleInput()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Update()
    {
    }

    protected override void OnMovementStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<SwordMaster_Walk>();
    }
}
