using System.Buffers;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMaster_Run : SwordMaster_BaseState
{
    public SwordMaster_Run(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        
        SwordMaster owner = stateMachine.owner;
        PlayerData playerData = owner.playerData;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.runParamHash, true);
        owner.moveController.moveSpeed = playerData.baseSpeed * playerData.runSpeedModifier;
    }

    public override void Exit()
    {
        base.Exit();

        SwordMaster owner = stateMachine.owner;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.runParamHash, false);
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
