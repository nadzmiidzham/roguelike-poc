using System;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp, maxHp;
    public float ep, maxEp;
    public float atk, def, spd;
    public float jumpPower;

    bool isFacingRight = true;
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] LayerMask groundLayer;
    Animator animator;
    Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider2D;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.Space))
        {
            Debug.Log(IsGrounded());
            Jump();
        }

        Movement();
    }

    void Jump()
    {
        rigidbody2D.velocity = Vector2.up * jumpPower * Time.deltaTime;
    }

    void Movement()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        float movement = direction * spd * Time.deltaTime;

        if (!isFacingRight && (direction > 0))
        {
            Flip();
        }
        else if (isFacingRight && (direction < 0))
        {
            Flip();
        }

        rigidbody2D.velocity = new Vector2(movement, rigidbody2D.velocity.y);
        animator.SetFloat("velocity", Math.Abs(direction));
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 thisScale = transform.localScale;
        thisScale.x *= -1;
        transform.localScale = thisScale;
    }

    bool IsGrounded()
    {
        return groundCheck.IsTouchingLayers(groundLayer);
    }
}
