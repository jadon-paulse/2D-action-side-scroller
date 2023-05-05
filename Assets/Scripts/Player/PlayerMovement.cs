using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [SerializeField] private float attackCooldown;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
            //anim.SetBool("running", true);
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
           // anim.SetBool("running", true);
        }

        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && canAttack())
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;

        //Set animator parameters
        anim.SetBool("running", horizontal != 0);
        anim.SetBool("ground", IsGrounded());
    }

    // private void FixedUpdate()
    // {
    //     rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    // }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        Console.WriteLine("Player Attack");
    }

    public bool canAttack()
    {
        return horizontal == 0 && IsGrounded(); 
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

}

