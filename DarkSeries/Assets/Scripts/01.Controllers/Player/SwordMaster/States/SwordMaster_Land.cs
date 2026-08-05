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
        SwordMasterAnimData animData = owner.animationData;

        owner.animator.SetBool(animData.isGroundParamHash, true);
        owner.moveController.moveSpeed = 0f;
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
        if(stateMachine.owner.inputController.movementInput.sqrMagnitude >= 0.01f)
        {
            stateMachine.ChangeState<SwordMaster_Walk>();
        }
        else
        {
            stateMachine.ChangeState<SwordMaster_Idle>();
        }
    }
}
