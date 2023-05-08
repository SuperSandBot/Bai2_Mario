using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public CircleCollider2D circleCollider2D;
    float gravity = -23f;
    Vector2 direction;
    Vector2 velocity;
    public LayerMask layerMask;
    public float speed;
    public float JumpForce = 3f;
    int jumpCooldown = 2;
    float jumpTime = 0;

    public bool jumpAble = false;
    bool active = false;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        rigidbody2D.isKinematic = true;
        circleCollider2D.enabled = false;
        velocity = Vector2.zero;
        direction = Vector2.right;
    }
    void FixedUpdate()
    {
        if(GameManager.Instance.running == false) return;
        if(!active) return;
        velocity.x = direction.x * speed;
        if(jumpAble)
        {
            jumpTime -= Time.deltaTime;
            if(jumpTime <= 0)
            {
                jumpTime = jumpCooldown;
                velocity.y = JumpForce;
            }
        }
        
        rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.fixedDeltaTime);
        velocity.y += gravity * Time.fixedDeltaTime;
        
        if(rigidbody2D.RayCastCheck(direction,layerMask))
        {
            direction = -direction;
        }

        if(rigidbody2D.RayCastCheck(Vector2.down,layerMask))
        {
            velocity.y = Mathf.Max(velocity.y,0f);
        }
    }

    public void Eated()
    {
        Destroy(this.gameObject);
    }

    public void Activate()
    {
        rigidbody2D.isKinematic = false;
        circleCollider2D.enabled = true;
        active = true;
    }
}
