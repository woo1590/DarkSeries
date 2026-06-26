using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer sprite { get; private set; }
    public Animator animator { get; private set; }
    public PlayerMoveController moveController { get; private set; }
    public PlayerInputController inputController { get; private set; }

    private int facingDirection;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveController = GetComponent<PlayerMoveController>();
        inputController = GetComponent<PlayerInputController>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void LateUpdate()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        
    }

    public void UpdateFacing(float moveX)
    {
        if (Mathf.Abs(moveX) < 0.001f)
            return;

        facingDirection = moveX < 0f ? -1 : 1;
        sprite.flipX = facingDirection < 0;
    }
}