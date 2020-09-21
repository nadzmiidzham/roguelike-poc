using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour
{
    [Header("Custom 2D Physic")]
    public float gravity = 1f;
    public float minGroundNormalY = 0.65f;

    protected Vector2 velocity;
    protected Rigidbody2D rigidbody;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected Vector2 targetVelocity;

    protected bool grounded;
    protected Vector2 groundNormal;

    protected const float minMoveDistance = 0.001f;
    protected const float shellradius = 0.01f;

    void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    void FixedUpdate()
    {
        velocity += gravity * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;
        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
    }

    protected virtual void ComputeVelocity() { }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rigidbody.Cast(move, contactFilter, hitBuffer, distance + shellradius);

            hitBufferList.Clear();
            for (int x = 0; x < count; x++)
            {
                hitBufferList.Add(hitBuffer[x]);
            }

            for (int x = 0; x < hitBufferList.Count; x++)
            {
                Vector2 currentNormal = hitBufferList[x].normal;

                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;

                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[x].distance - shellradius;
                distance = (modifiedDistance < distance) ? modifiedDistance : distance;
            }
        }

        rigidbody.position = rigidbody.position + move.normalized * distance;
    }
}
