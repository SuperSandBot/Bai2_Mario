using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    float gravity = -9.81f;
    public Vector2 direction;
    Vector2 velocity;
    public LayerMask layerMask;
    public float speed;

    public bool active = true;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        this.enabled = false;
        velocity = Vector2.zero;
    }

    void OnBecameVisible() 
    {
        this.enabled = true;
    }

    void OnBecameInvisible()
    {
        this.enabled = false;
    }

    void OnEnable() 
    {
        rigidbody2D.WakeUp();
    }

    void OnDisable()
    {
        velocity = Vector2.zero;
        rigidbody2D.Sleep();    
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

    void FixedUpdate()
    {
        if(GameManager.Instance.running == false) return;
        if(!active) return;
        velocity.x = direction.x * speed;
        velocity.y += gravity * Time.fixedDeltaTime;
        
        rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.fixedDeltaTime);

        if(rigidbody2D.RayCastCheck(direction,layerMask))
        {
            direction = -direction;
            flip();
        }

        if(rigidbody2D.RayCastCheck(Vector2.down,layerMask))
        {
            velocity.y = Mathf.Max(velocity.y,0f);
        }
    }
}
