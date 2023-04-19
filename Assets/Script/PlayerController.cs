using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extenstions
{
    public static bool RayCastCheck(this Rigidbody2D rigidbody2D, Vector2 dir,LayerMask layerMask)
    {
        if(rigidbody2D.isKinematic) return false;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody2D.position, 0.25f, dir.normalized, .375f, layerMask);
        return hit.collider != null;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = transform.position - other.position;
        return Vector2.Dot(direction.normalized,testDirection) > .00001f;
    }
}

public class PlayerController : MonoBehaviour
{

    CapsuleCollider2D capsuleCollider2D;
    new Rigidbody2D rigidbody2D;
    public Vector2 velocity;
    Camera cam;
    public LayerMask layerMask;
    Animator animator;

    public float speed;
    public float maxJumpHeight;
    public float maxJumpTime;
    public float inputAxis;
    public float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f),2);

    public bool grounded => (rigidbody2D.RayCastCheck(Vector2.down,layerMask));
    public bool jumping => (velocity.y > 0f);
    public bool running  => (Mathf.Abs(velocity.x) > .25f || Mathf.Abs(inputAxis) > .25f);
    public bool sliding => ((inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f));

    

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        velocity = Vector2.zero;
        cam = Camera.main;
    }

    void Update()
    {
        Movement();
        if(grounded)
        {
            Grounded();
        }
        ApplyGravity();
    }

    public void Grounded()
    {  
        velocity.y = Mathf.Max(velocity.y,0f);
        if(Input.GetButtonDown("Jump")) velocity.y = JumpForce;
    }
    public void Movement()
    {
        inputAxis = Input.GetAxisRaw("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * speed, speed * Time.deltaTime);
        flip();
        if(rigidbody2D.RayCastCheck(Vector2.right * velocity.x,layerMask)) velocity.x = 0;
    }

    public void ApplyGravity()
    {
        bool falling =  velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y,gravity / 2f);
    }

    void FixedUpdate()
    {
        Vector2 pos = rigidbody2D.position;
        pos += velocity * Time.fixedDeltaTime;
        Vector2 leftEdge = cam.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = cam.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));
        pos.x = Mathf.Clamp(pos.x,leftEdge.x + 0.5f,rightEdge.x - 0.5f);
        rigidbody2D.MovePosition(pos);
    }

    public void flip()
    {
        if(velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0,180f,0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.layer == LayerMask.NameToLayer("PowerUp")) return;
        if(transform.DotTest(other.transform,Vector2.up))
        {
            velocity.y = 0f;
        }
    }
    
}
