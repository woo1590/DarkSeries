using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SwordMaster_Slash1 : SwordMaster_BaseState
{
    public SwordMaster_Slash1(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

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

    public override void HandleInput()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Update()
    {
    }
}
