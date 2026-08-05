using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveController : MonoBehaviour
{
    private Vector2 groundCheckSize = new Vector2(0.3f, 0.1f);
    private Vector2 groundCheckOffset = new Vector2(0f,-0.5f);
    public bool isGrounded { get; private set; }

    public float moveSpeed { get; set; }
    public float jumpPower { get; set; }    

    private int platformLayer;
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.GetMask("Platform");
    }

    public void Move(float direction)
    {
        rigidbody.linearVelocityX = moveSpeed * direction;
    }

    public void Jump()
    {
        rigidbody.linearVelocityY = jumpPower;  
    }

    private void FixedUpdate()
    {
        Vector2 groundCheckPosition = (Vector2)transform.position + groundCheckOffset;

        isGrounded = Physics2D.OverlapBox(groundCheckPosition, groundCheckSize, 0f, platformLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;

        Gizmos.color = isGrounded? Color.green: Color.red;
        Gizmos.DrawWireCube(checkPosition, groundCheckSize);
    }
}
