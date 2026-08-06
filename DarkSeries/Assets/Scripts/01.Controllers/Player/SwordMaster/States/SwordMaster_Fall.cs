using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwordMaster_Fall : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.Fall;

    public SwordMaster_Fall(StateMachine<SwordMaster> stateMachine)
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
        PlayerMoveController moveController = stateMachine.owner.moveController;

        if (moveController.isGrounded)
            ChangeToLandingState();
    }

    public override void FixedUpdate()
    {
    }

}
