using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public PlayerState playerState;
    public float jumpForce;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;

    [Header("Wall Check")]
    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckRadius;

    [Header("Movement Physic")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;

    private new Rigidbody2D rigidbody;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool isTouchingWall = false;

    private float movementInput;
    private bool jumpInput = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    void FixedUpdate()
    {
        // setup variables
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        // action
        Movement();
        Jump();
    }

    void Movement()
    {
        rigidbody.AddForce(Vector2.right * movementInput * playerState.spd);
        if (Mathf.Abs(rigidbody.velocity.x) > maxSpeed)
        {
            rigidbody.velocity = new Vector2((Mathf.Sign(rigidbody.velocity.x) * maxSpeed), rigidbody.velocity.y);
        }

        // flip character
        if (!isFacingRight && (movementInput > 0))
        {
            Flip();
        }
        else if (isFacingRight && (movementInput < 0))
        {
            Flip();
        }

        // apply linear drag physic
        bool isChangingDirection = (movementInput > 0 && rigidbody.velocity.x < 0) || (movementInput < 0 && rigidbody.velocity.x > 0);
        if (Mathf.Abs(movementInput) < 0.4f || isChangingDirection)
        {
            rigidbody.drag = linearDrag;
        }
        else
        {
            rigidbody.drag = 0f;
        }
    }

    void Jump()
    {
        if (jumpInput && (isGrounded || isTouchingWall))
        {
            // wall jump
            if (isTouchingWall)
            {
                Debug.Log("Wall Jump");
                rigidbody.AddForce((new Vector2(5, 1) * jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Jump");
                rigidbody.AddForce((transform.up * jumpForce), ForceMode2D.Impulse);
            }

            jumpInput = false;
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
