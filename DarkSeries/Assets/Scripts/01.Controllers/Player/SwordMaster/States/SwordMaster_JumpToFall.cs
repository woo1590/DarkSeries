using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwordMaster_JumpToFall : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.JumpToFall;

    public SwordMaster_JumpToFall(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        if (stateMachine.owner.moveController.isGrounded)
        {
            ChangeToLandingState();
            return;
        }

        if (stateMachine.owner.isCurrentAnimationFinished)
            OnAnimationEnd();
    }

    public override void FixedUpdate()
    {
    }

    public override void OnAnimationEnd()
    {
        stateMachine.ChangeState<SwordMaster_Fall>();
    }
}
