using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwordMaster_Land : SwordMaster_BaseState
{
    public SwordMaster_Land(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        PlayerData playerData = owner.playerData;
        PlayerMoveController moveController = owner.moveController;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.isGroundParamHash, true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void OnAnimationEnd()
    {
        stateMachine.ChangeState<SwordMaster_Idle>();
    }
}