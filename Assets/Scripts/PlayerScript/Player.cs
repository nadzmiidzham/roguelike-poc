using System;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp, maxHp;
    public float ep, maxEp;
    public float atk, def, spd;

    bool isFacingRight = true;
    Animator animator;
    Rigidbody2D rigidbody2D;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        float movement = direction * spd * Time.deltaTime;

        if (!isFacingRight && (direction > 0))
        {
            Flip();
        }
        else if(isFacingRight && (direction < 0))
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
}
