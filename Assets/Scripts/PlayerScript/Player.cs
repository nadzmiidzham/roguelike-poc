using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp, ep;
    public float maxHp, maxEp;
    public float atk, def, spd;
    [Range(0.0f, 1.0f)] public float jumpPower;
    [Range(0.0f, 1.0f)] public float jumpFriction;
    [Range(1.0f, 10.0f)] public float wallJumpPower;
    [Range(-5.0f, 5.0f)] public float wallSlidingFriction;
    public Vector2 wallJumpAngle;

    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] BoxCollider2D wallCheck;
    [SerializeField] LayerMask wallLayer;

    bool isFacingRight = true;
    bool isGrounded = false;
    bool isTouchingWall = false;
    bool isWallSliding = false;
    bool isWallJumping = false;
    Animator animator;
    Rigidbody2D rigidbody2D;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // set state
        isGrounded = groundCheck.IsTouchingLayers(groundLayer);
        isTouchingWall = wallCheck.IsTouchingLayers(wallLayer);

        // check if wall sliding has finished
        if (isGrounded || !isTouchingWall)
        {
            isWallSliding = false;
        }

        // check if wall jumping has finished
        if (isWallJumping && isGrounded)
        {
            isWallJumping = false;
        }

        // action
        // flip character based on the direction
        float direction = Input.GetAxisRaw("Horizontal");
        if (!isFacingRight && (direction > 0))
        {
            Flip();
        }
        else if (isFacingRight && (direction < 0))
        {
            Flip();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if ((Math.Abs(Input.GetAxisRaw("Horizontal")) > 0) && isTouchingWall && !isGrounded && (rigidbody2D.velocity.y < 0))
        {
            WallSlide();
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isTouchingWall || isWallSliding) && !isGrounded)
        {
            WallJump();
        }
        if (!isWallJumping)
        {
            Movement(direction);
        }
    }

    void Jump()
    {
        // add force upward
        rigidbody2D.AddForce(new Vector2(0, jumpPower * 50000) * Time.deltaTime);
    }

    void WallSlide()
    {
        isWallSliding = true;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, wallSlidingFriction);
    }

    void WallJump()
    {
        float direction = isFacingRight ? -1f : 1f; // set jump direction

        // apply wall jump action
        rigidbody2D.AddForce(new Vector2((jumpPower * direction * wallJumpAngle.x), (wallJumpPower * wallJumpAngle.y)), ForceMode2D.Impulse);
        isWallJumping = true;
    }

    void Movement(float direction)
    {
        float movementSpeed = isGrounded ? spd : (spd * jumpFriction); // reduce movement speed when character is in the air
        float movement = isTouchingWall ? 0f : direction * movementSpeed * Time.deltaTime;

        rigidbody2D.velocity = new Vector2(movement, rigidbody2D.velocity.y); // move character
        animator.SetFloat("velocity", Math.Abs(direction)); // set animation for movement
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 thisScale = transform.localScale;
        thisScale.x *= -1;
        transform.localScale = thisScale;
    }
}
