using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerState playerState;
    public float jumpForce;

    // ground check
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;

    // wall check
    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckRadius;

    private float moveDirection;
    private new Rigidbody2D rigidbody;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool isTouchingWall = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // setup variables
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
        moveDirection = Input.GetAxisRaw("Horizontal");

        // action
        // movement
        rigidbody.velocity = new Vector2(moveDirection * playerState.spd, rigidbody.velocity.y);
        if (!isFacingRight && (moveDirection > 0))
        {
            Flip();
        }
        else if (isFacingRight && (moveDirection < 0))
        {
            Flip();
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isTouchingWall)
        {
            Debug.Log("Jump");
            rigidbody.velocity = Vector2.up * jumpForce;
        }

        // wall jump
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && isTouchingWall)
        {
            Debug.Log("Wall Jump");
            rigidbody.velocity = (new Vector2(5, 1)) * jumpForce;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
