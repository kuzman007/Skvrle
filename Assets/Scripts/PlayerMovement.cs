using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float dirX = 0f;
    private SpriteRenderer sprite;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 14f;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource skakanjeZvuk;
    private enum Movement { idle, running, jumping, falling}
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            skakanjeZvuk.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        Movement state;
        if (dirX > 0f)
        {
            state = Movement.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = Movement.running;
            sprite.flipX = true;
        }
        else
        {
            state = Movement.idle;
        }
        if(rb.velocity.y > .1f)
        {
            state = Movement.jumping;
        }else if (rb.velocity.y< -.1f)
        {
            state = Movement.falling;
        }
        anim.SetInteger("movement", (int)state);
    }
    private bool isGrounded()
    {
       return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
