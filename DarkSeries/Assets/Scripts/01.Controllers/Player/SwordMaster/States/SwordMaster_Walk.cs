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
    public SwordMaster_Walk(StateMachine<SwordMaster> stateMachine) 
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        PlayerData playerData = owner.playerData;
        PlayerMoveController moveController = owner.moveController;
        SwordMasterAnimData animData = owner.animationData;

        stateMachine.owner.animator.SetBool(animData.walkParamHash, true);
        moveController.moveSpeed = playerData.baseSpeed * playerData.walkSpeedModifier;
    }

    public override void Exit()
    {
        base.Exit();

        SwordMaster owner = stateMachine.owner;
        PlayerData playerData = owner.playerData;
        PlayerMoveController moveController = owner.moveController;
        SwordMasterAnimData animData = owner.animationData;

        stateMachine.owner.animator.SetBool(animData.walkParamHash, false);
    }

    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Update()
    {
        SwordMaster owner = stateMachine.owner;
        float direction = owner.inputController.movementInput.x;

        owner.moveController.Move(direction);
        owner.UpdateFacing(direction);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<SwordMaster_Idle>();
    }
}