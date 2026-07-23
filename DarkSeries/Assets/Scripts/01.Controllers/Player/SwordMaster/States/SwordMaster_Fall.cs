using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwordMaster_Fall : SwordMaster_BaseState
{
    public SwordMaster_Fall(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.fallParamHash, true);
    }

    public override void Exit()
    {
        base.Exit();

        SwordMaster owner = stateMachine.owner;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.fallParamHash, false);
    }

    public override void Update()
    {
        PlayerMoveController moveController = stateMachine.owner.moveController;

        if (moveController.isGrounded)
            stateMachine.ChangeState<SwordMaster_Land>();
    }

    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {
    }
}