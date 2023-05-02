using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuard : MonoBehaviour
{

    public float speed = 2f;
    public int health = 1;
    private Animator anim;
    public Rigidbody2D rb;
    public LayerMask groundLayers;

    public DetectionZone attackZone;

    public Transform groundCheck;

    bool isFacingRight = true;

    RaycastHit2D hit;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool _hasTarget = false;

    public bool HasTarget { get { return _hasTarget; } private set 
        {

            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);

        } 
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            anim.SetBool("death", true);
            Destroy(gameObject);
            //Die();
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        hit = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayers);
    }

    private void FixedUpdate()
    {
        if (hit.collider != false)
        {
            if (isFacingRight)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            } else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }

        } else
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
        }
    }
}
