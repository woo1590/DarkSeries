using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SwordMaster_SlashAttack : SwordMaster_BaseState
{
    public SwordMaster_SlashAttack(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    int comboIndex;

    public override void Enter()
    {
        base.Enter();

        SwordMasterAnimData animData = stateMachine.owner.animationData;
        stateMachine.owner.animator.SetBool(animData.slashParamHash, true);
    }

    public override void Exit()
    {
        base.Exit();

        SwordMasterAnimData animData = stateMachine.owner.animationData;
        stateMachine.owner.animator.SetBool(animData.slashParamHash, false);
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
}
