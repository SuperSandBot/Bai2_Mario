using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float MaxSpeed;
    public float JumpForce;
    CapsuleCollider2D boxCollider2D;
    Rigidbody2D rb;
    public LayerMask layerMask;
    Animator animator;
    bool facing = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void Jump()
    {
        rb.velocity += (Vector2.up * JumpForce);
    }
    public void Movement(int dir)
    {
        if (dir < 0 && facing)
        {
            flip();
        }
        else if(dir > 0 && !facing) 
        {
            flip();
        }

        if (Groundcheck())
        {
            rb.velocity = new Vector2(speed * dir, rb.velocity.y);    
        }
    }

    public bool Groundcheck()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.transform.position,boxCollider2D.bounds.size, 0f, Vector2.down, .1f,layerMask);
        return raycastHit2D.collider != null;
    }

    public void flip()
    {
        facing = !facing;
        transform.Rotate(0f, 180f, 0f);
    }
    
}
