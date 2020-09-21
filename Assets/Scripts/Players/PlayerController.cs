using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicObject
{
    [Header("Player Behaviour")]
    public PlayerState playerState;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = playerState.jumpForce;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            velocity.y = (velocity.y > 0) ? 0.5f : velocity.y;
        }

        Debug.Log(move.x);
        bool flipSprite = spriteRenderer.flipX ? (move.x > 0) : (move.x < 0);
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        targetVelocity = move * playerState.spd;
    }
}
