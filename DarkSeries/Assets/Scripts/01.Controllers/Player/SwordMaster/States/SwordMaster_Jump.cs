using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SwordMaster_Jump : SwordMaster_BaseState
{
    protected override SwordMasterAnimationState animationState => SwordMasterAnimationState.Jump;

    public SwordMaster_Jump(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        SwordMaster owner = stateMachine.owner;
        PlayerData playerData = owner.playerData;
        PlayerMoveController moveController = owner.moveController;
        moveController.jumpPower = playerData.baseJumpPower * playerData.jumpPowerModifier;
        moveController.Jump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        Rigidbody2D rigid = stateMachine.owner.GetComponent<Rigidbody2D>();

        if (rigid.linearVelocityY <= 0f)
            stateMachine.ChangeState<SwordMaster_JumpToFall>();
    }

    public override void FixedUpdate()
    {
    }

}
