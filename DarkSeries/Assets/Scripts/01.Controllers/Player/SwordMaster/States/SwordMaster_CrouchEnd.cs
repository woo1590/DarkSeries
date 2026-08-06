using System.Buffers;

using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMaster_CrouchEnd : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.CrouchEnd;

    public SwordMaster_CrouchEnd(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        owner.moveController.moveSpeed = 0f;
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (ChangeToAirborneStateIfNeeded())
            return;

        if (stateMachine.owner.isCurrentAnimationFinished)
            OnAnimationEnd();
    }

    public override void OnAnimationEnd()
    {
        ChangeToGroundedMovementState();
    }
}
