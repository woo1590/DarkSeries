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

        SwordMaster owner = stateMachine.owner;
        PlayerData playerData = owner.playerData;
        PlayerMoveController moveController = owner.moveController;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.idleParamHash,true);
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

    public override void LateUpdate()
    {
    }

    public override void Update()
    {
        PlayerMoveController moveController = stateMachine.owner.moveController;
        PlayerInputController inputController = stateMachine.owner.inputController;

        if(!moveController.isGrounded)
        {
            stateMachine.ChangeState<SwordMaster_JumpToFall>();
        }

        if (inputController.movementInput.sqrMagnitude >= 0.01f) 
        {
            stateMachine.ChangeState<SwordMaster_Walk>();
        }
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<SwordMaster_SlashAttack>();
    }

    protected override void OnJumpStatred(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<SwordMaster_Jump>();
    }

    protected override void OnCrouchStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<SwordMaster_CrouchStart>();
    }
}
