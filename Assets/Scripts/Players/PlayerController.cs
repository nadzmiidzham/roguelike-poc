using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerState playerState;
    public LayerMask groundlayer;
    public Vector2 topCheck, bottomCheck, rightCheck, leftCheck;
    public float collisionRadius;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float slideSpeed;

    new Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;

    bool onGround = false;
    bool onWall = false;
    bool isFacingRight = true;
    bool isWallJump = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomCheck, collisionRadius, groundlayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightCheck, collisionRadius, groundlayer);

        // add gravity based on how long the jump
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if ((rigidbody.velocity.y > 0) && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // flip sprite
        if ((direction.x > 0) && !isFacingRight)
        {
            Flip();
        }
        if ((direction.x < 0) && isFacingRight)
        {
            Flip();
        }

        // check is wall jump
        if (onGround && isWallJump)
        {
            isWallJump = false;
        }

        // walk
        if (!isWallJump)
        {
            Walk(direction);
        }

        // jump
        if (Input.GetButtonDown("Jump") && onGround)
        {
            Jump();
        }

        // wall jump
        if (Input.GetButtonDown("Jump") && onWall && !onGround)
        {
            WallJump(Vector2.right);
            isWallJump = true;
        }
        if (onWall && !onGround && (direction.x == (isFacingRight ? Vector2.right.x : Vector2.left.x)))
        {
            WallSlide();
        }
    }

    void Walk(Vector2 direction)
    {
        rigidbody.velocity = (new Vector2(direction.x * playerState.spd, rigidbody.velocity.y));
    }

    void Jump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        rigidbody.velocity += Vector2.up * playerState.jumpForce;
    }

    void WallJump(Vector2 direction)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        rigidbody.velocity = new Vector2(playerState.jumpForce, playerState.spd);
        Debug.Log(rigidbody.velocity);
    }

    void WallSlide()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, -slideSpeed);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // change bottom & right check
        topCheck.x *= -1;
        bottomCheck.x *= -1;
        rightCheck.x *= -1;
        leftCheck.x *= -1;
    }

    void OnDrawGizmos()
    {
        var position = new Vector2[] { topCheck, bottomCheck, rightCheck, leftCheck };

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + topCheck, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomCheck, collisionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + rightCheck, collisionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + leftCheck, collisionRadius);
    }
}
