using System.Buffers;
using UnityEngine.InputSystem;

using UnityEngine;

public class SwordMaster_CrouchHold : SwordMaster_BaseState
{
    public SwordMaster_CrouchHold(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

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

    protected override void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState<SwordMaster_CrouchEnd>();
    }
}
