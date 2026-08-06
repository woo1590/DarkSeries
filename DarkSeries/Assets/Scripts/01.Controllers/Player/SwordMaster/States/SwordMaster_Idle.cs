using System.Buffers;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMaster_Idle : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.Idle;

    public SwordMaster_Idle(StateMachine<SwordMaster> stateMachine)
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
        PlayerInputController input = stateMachine.owner.inputController;

        if (ChangeToAirborneStateIfNeeded())
            return;

        if (input.crouchPressedThisFrame || input.crouchIsPressed)
        {
            stateMachine.ChangeState<SwordMaster_CrouchStart>();
            return;
        }

        if (input.attackPressedThisFrame)
        {
            stateMachine.ChangeState<SwordMaster_SlashAttack>();
            return;
        }

        if (input.jumpPressedThisFrame)
        {
            stateMachine.ChangeState<SwordMaster_Jump>();
            return;
        }

        if (input.movementInput.sqrMagnitude >= 0.01f)
            stateMachine.ChangeState<SwordMaster_Walk>();
    }
}
