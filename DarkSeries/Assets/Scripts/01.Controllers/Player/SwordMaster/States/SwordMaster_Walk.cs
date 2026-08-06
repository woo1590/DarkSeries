using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.InputSystem;

public class SwordMaster_Walk : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.Walk;

    public SwordMaster_Walk(StateMachine<SwordMaster> stateMachine) 
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        PlayerData playerData = owner.playerData;
        PlayerMoveController moveController = owner.moveController;
        moveController.moveSpeed = playerData.baseSpeed * playerData.walkSpeedModifier;
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

        if (input.movementInput.sqrMagnitude < 0.01f)
            stateMachine.ChangeState<SwordMaster_Idle>();
    }
}
