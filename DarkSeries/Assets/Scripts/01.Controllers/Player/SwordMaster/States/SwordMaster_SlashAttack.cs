using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class SwordMaster_SlashAttack : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.SlashAttack;

    public SwordMaster_SlashAttack(StateMachine<SwordMaster> stateMachine)
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

    public override void LateUpdate()
    {
    }

    public override void Update()
    {
        if (stateMachine.owner.inputController.attackPressedThisFrame)
            stateMachine.owner.attackController.BufferSlash();

        if (stateMachine.owner.isCurrentAnimationFinished)
            OnAnimationEnd();
    }

    public override void OnAnimationEnd()
    {
        SwordMasterAttackController attackController = stateMachine.owner.attackController;

        if (!attackController.ConsumeBuffer())
        {
            attackController.ResetCombo();
            ChangeToGroundedMovementState();
        }
        else
        {
            attackController.IncreaseCombo();
            stateMachine.owner.SyncAnimation(SwordMasterAnimationState.SlashAttack);
        }
    }
}
