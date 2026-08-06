using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwordMaster_Land : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.Land;

    public SwordMaster_Land(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        owner.moveController.moveSpeed = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (stateMachine.owner.isCurrentAnimationFinished)
            OnAnimationEnd();
    }

    public override void FixedUpdate()
    {
    }

    public override void OnAnimationEnd()
    {
        ChangeToGroundedMovementState();
    }
}
