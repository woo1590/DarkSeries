using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public PlayerActionMap inputActions { get; private set; }
    public PlayerActionMap.PlayerActions playerActions { get; private set; }
    public Vector2 movementInput { get; private set; }
    public bool attackPressedThisFrame { get; private set; }
    public bool jumpPressedThisFrame { get; private set; }
    public bool crouchPressedThisFrame { get; private set; }
    public bool crouchReleasedThisFrame { get; private set; }
    public bool crouchIsPressed { get; private set; }

    private void Awake()
    {
        inputActions = new PlayerActionMap();
        playerActions = inputActions.Player;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    public void HandleInput()
    {
        movementInput = playerActions.Movement.ReadValue<Vector2>();
        attackPressedThisFrame = playerActions.Attack.WasPressedThisFrame();
        jumpPressedThisFrame = playerActions.Jump.WasPressedThisFrame();
        crouchPressedThisFrame = playerActions.Crouch.WasPressedThisFrame();
        crouchReleasedThisFrame = playerActions.Crouch.WasReleasedThisFrame();
        crouchIsPressed = playerActions.Crouch.IsPressed();
    }
}
