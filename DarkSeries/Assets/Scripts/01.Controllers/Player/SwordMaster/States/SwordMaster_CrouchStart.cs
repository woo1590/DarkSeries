using System.Buffers;
using UnityEngine;
using UnityEngine.InputSystem;

using Unity.VisualScripting;

public class SwordMaster_CrouchStart : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.CrouchStart;

    public SwordMaster_CrouchStart(StateMachine<SwordMaster> stateMachine)
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

    public override void LateUpdate()
    {
    }

    public override void Update()
    {
        if (ChangeToAirborneStateIfNeeded())
            return;

        PlayerInputController input = stateMachine.owner.inputController;
        if (input.crouchReleasedThisFrame || !input.crouchIsPressed)
        {
            stateMachine.ChangeState<SwordMaster_CrouchEnd>();
            return;
        }

        if (stateMachine.owner.isCurrentAnimationFinished)
            OnAnimationEnd();
    }

    public override void OnAnimationEnd()
    {
        if (stateMachine.owner.inputController.crouchIsPressed)
            stateMachine.ChangeState<SwordMaster_CrouchHold>();
        else
            stateMachine.ChangeState<SwordMaster_CrouchEnd>();
    }
}
