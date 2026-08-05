using System.Buffers;
using UnityEngine;
using UnityEngine.InputSystem;

using Unity.VisualScripting;

public class SwordMaster_CrouchStart : SwordMaster_BaseState
{
    public SwordMaster_CrouchStart(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.crouchParamHash,true);
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
    }

    public override void OnAnimationEnd()
    {
        stateMachine.ChangeState<SwordMaster_CrouchHold>();
    }
}
