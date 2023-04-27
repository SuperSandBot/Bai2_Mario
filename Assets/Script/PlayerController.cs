using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public Vector2 velocity;
    Camera cam;
    public LayerMask layerMask;

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

    public bool active  = true;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
        cam = Camera.main;
    }

    void Update()
    {
        if(!active) return;
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
        if(!active) return;
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

    void OnCollisionEnter2D(Collision2D other) 
    {   
        
        if(other.gameObject.layer == LayerMask.NameToLayer("PowerUp")) return;
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(other.transform.DotTest(this.transform,Vector2.down))
            {
                velocity.y = JumpForce / 2f;
            }
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            if(other.transform.DotTest(this.transform,Vector2.up))
            {
                velocity.y = 0f;
            } 
        }
    }
    
}
