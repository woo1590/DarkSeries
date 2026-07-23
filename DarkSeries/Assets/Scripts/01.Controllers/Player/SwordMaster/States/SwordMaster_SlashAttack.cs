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
    public SwordMaster_SlashAttack(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    private static readonly int comboIndexHash = Animator.StringToHash("ComboIndex");

    public override void Enter()
    {
        base.Enter();
        
        SwordMasterAnimData animData = stateMachine.owner.animationData;
        int comboIndex = stateMachine.owner.attackController.slashComboIndex;

        stateMachine.owner.animator.SetInteger(comboIndexHash, comboIndex);
        stateMachine.owner.animator.SetBool(animData.attackParamHash, true);

    }

    public override void Exit()
    {
        base.Exit();

        SwordMasterAnimData animData = stateMachine.owner.animationData;
        stateMachine.owner.animator.SetBool(animData.attackParamHash, false);
    }

    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Update()
    {
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        stateMachine.owner.attackController.BufferSlash();
    }

    public override void OnAnimationEnd()
    {
        SwordMasterAttackController attackController = stateMachine.owner.attackController;

        if (!attackController.ConsumeBuffer())
        {
            attackController.ResetCombo();
            stateMachine.ChangeState<SwordMaster_Idle>();
        }
        else
        {
            attackController.IncreaseCombo();
        }

        stateMachine.owner.animator.SetInteger(comboIndexHash, attackController.slashComboIndex);
    }
}
