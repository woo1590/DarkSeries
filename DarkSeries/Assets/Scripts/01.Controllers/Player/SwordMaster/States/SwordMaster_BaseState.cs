using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMaster_BaseState : IState<SwordMaster>
{
    public SwordMaster_BaseState(StateMachine<SwordMaster> stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        AddInputActionCallBacks();
    }

    public override void Exit()
    {
        RemoveInputActionCallBacks();
    }

    public override void FixedUpdate() { }
    public override void LateUpdate() { }
    public override void Update() { }

    /* Input Action Callbacks */
    protected virtual void AddInputActionCallBacks()
    {
        PlayerInputController input = stateMachine.owner.inputController;

        input.playerActions.Movement.started += OnMovementStarted;
        input.playerActions.Movement.performed += OnMovementPerformed;
        input.playerActions.Movement.canceled += OnMovementCanceled;

        input.playerActions.Attack.started += OnAttackStarted;
        input.playerActions.Attack.canceled += OnAttackCanceled;

        input.playerActions.Jump.started += OnJumpStatred;

        input.playerActions.Crouch.started += OnCrouchStarted;
        input.playerActions.Crouch.canceled += OnCrouchCanceled;

    }

    protected virtual void RemoveInputActionCallBacks()
    {
        PlayerInputController input = stateMachine.owner.inputController;

        input.playerActions.Movement.started -= OnMovementStarted;
        input.playerActions.Movement.performed -= OnMovementPerformed;
        input.playerActions.Movement.canceled -= OnMovementCanceled;

        input.playerActions.Attack.started -= OnAttackStarted;
        input.playerActions.Attack.canceled -= OnAttackCanceled;

        input.playerActions.Jump.started -= OnJumpStatred;

        input.playerActions.Crouch.started -= OnCrouchStarted;
        input.playerActions.Crouch.canceled -= OnCrouchCanceled;

    }

    protected virtual void OnMovementStarted(InputAction.CallbackContext context) { }
    protected virtual void OnMovementPerformed(InputAction.CallbackContext context) { }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) { }

    protected virtual void OnAttackStarted(InputAction.CallbackContext context) { }
    protected virtual void OnAttackCanceled(InputAction.CallbackContext context) { }

    protected virtual void OnJumpStatred(InputAction.CallbackContext context) { }

    protected virtual void OnCrouchStarted(InputAction.CallbackContext context) { }
    protected virtual void OnCrouchCanceled(InputAction.CallbackContext context) { }
}
