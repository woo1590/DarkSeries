using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwordMaster_JumpToFall : SwordMaster_BaseState
{
    public SwordMaster_JumpToFall(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.startFallParamHash,true);
    }

    public override void Exit()
    {
        base.Exit();

        SwordMaster owner = stateMachine.owner;
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.startFallParamHash, false);
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
        stateMachine.ChangeState<SwordMaster_Fall>();
    }
}
