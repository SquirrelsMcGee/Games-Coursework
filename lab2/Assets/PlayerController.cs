using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 2.0f;
    private Rigidbody2D r;
    private Animator a;
    private SpriteRenderer s;

    private float xMove = 0.0f;
    private float yMove = 0.0f;

    int groundMask = 1 << 8;
    private bool canJump = false;

    public float gravity = 5;

    float idleVelocity = 0.1f;

    bool isIdle = true;
    bool isLeft = false;
    int isIdleKey = Animator.StringToHash("isIdle");


    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        s = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        a.SetBool(isIdleKey, isIdle);
        s.flipX = isLeft;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        xMove = Input.GetAxis("Horizontal");
        yMove = r.velocity.y;

        if (xMove < 0) // left
        {
            isLeft = true;
        } else if (xMove > 0) // right 
        {
            isLeft = false;
        }

        
        if (xMove > -idleVelocity && xMove < idleVelocity)
        {
            isIdle = true;
        } else
        {
            isIdle = false;
        }

        xMove *= speed;


        if (Input.GetButton("Jump") && canJump)
        {
            canJump = false;
            // Jump
            yMove = 1.5f * gravity;
        }

        // Test the ground immediately below the Player
        // and if it tagged as a Ground layer, then we allow the
        // Player to jump again.
        if (Physics2D.Raycast(new Vector2 (transform.position.x, transform.position.y), -Vector2.up, 1.0f, groundMask))
        {
            canJump = true;
        }
        //

        velocity.x = xMove;
        velocity.y = yMove;
        r.velocity = new Vector2(velocity.x, velocity.y);
    }
}
