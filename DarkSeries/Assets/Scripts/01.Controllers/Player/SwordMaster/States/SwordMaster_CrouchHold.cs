using System.Buffers;
using UnityEngine.InputSystem;

using UnityEngine;

public class SwordMaster_CrouchHold : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.CrouchHold;
    protected override bool restartAnimationOnEnter => false;

    public SwordMaster_CrouchHold(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        stateMachine.owner.moveController.moveSpeed = 0f;
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

        if (!stateMachine.owner.inputController.crouchIsPressed)
            stateMachine.ChangeState<SwordMaster_CrouchEnd>();
    }
}
