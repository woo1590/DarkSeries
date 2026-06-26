using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveController : MonoBehaviour
{
    public float moveSpeed { get; set; }

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(float direction)
    {
        rigidbody.linearVelocityX = moveSpeed * direction;
    }
}