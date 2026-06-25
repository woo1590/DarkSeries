using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public PlayerActionMap inputActions { get; private set; }
    public PlayerActionMap.PlayerActions playerActions { get; private set; }

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
}
