using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMaster_BaseState : IState<SwordMaster>
{
    protected virtual SwordMasterAnimationState animationState => SwordMasterAnimationState.None;
    protected virtual bool restartAnimationOnEnter => true;

    public SwordMaster_BaseState(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        if (animationState != SwordMasterAnimationState.None)
            stateMachine.owner.SyncAnimation(animationState, restartAnimationOnEnter);
    }

    public override void Exit() { }

    public override void FixedUpdate() { }
    public override void LateUpdate()
    {
        SwordMaster owner = stateMachine.owner;
        owner.UpdateFacing(owner.inputController.movementInput.x);
    }
    public override void Update() { }

    protected bool ChangeToAirborneStateIfNeeded()
    {
        if (stateMachine.owner.moveController.isGrounded)
            return false;

        stateMachine.ChangeState<SwordMaster_JumpToFall>();
        return true;
    }

    protected void ChangeToGroundedMovementState()
    {
        if (!stateMachine.owner.moveController.isGrounded)
        {
            stateMachine.ChangeState<SwordMaster_JumpToFall>();
            return;
        }

        if (stateMachine.owner.inputController.movementInput.sqrMagnitude >= 0.01f)
            stateMachine.ChangeState<SwordMaster_Walk>();
        else
            stateMachine.ChangeState<SwordMaster_Idle>();
    }

    protected void ChangeToLandingState()
    {
        if (stateMachine.owner.inputController.crouchIsPressed)
            stateMachine.ChangeState<SwordMaster_CrouchStart>();
        else
            stateMachine.ChangeState<SwordMaster_Land>();
    }
}
