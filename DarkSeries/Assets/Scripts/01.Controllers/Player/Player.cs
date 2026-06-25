using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer sprite { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody2D rigid { get; private set; }
    public PlayerInputController inputController { get; private set; }

    public Vector2 movementInput { get; set; }

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
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
}