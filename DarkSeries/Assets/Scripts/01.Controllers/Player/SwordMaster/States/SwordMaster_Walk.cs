using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMaster_Walk : SwordMaster_BaseState
{
    public SwordMaster_Walk(StateMachine<SwordMaster> stateMachine) 
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMasterAnimData animData = stateMachine.owner.animationData;
        stateMachine.owner.animator.SetBool(animData.walkParamHash, true);
    }

    public override void Exit()
    {
        base.Exit();

        SwordMasterAnimData animData = stateMachine.owner.animationData;
        stateMachine.owner.animator.SetBool(animData.walkParamHash, false);
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

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<SwordMaster_Idle>();
    }
}