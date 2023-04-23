using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    CapsuleCollider2D capsuleCollider2D;
    public PlayerController playerController;
    PlayerAnimation miniMario;
    PlayerAnimation bigMario;

    public bool IsBig = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        miniMario = transform.GetChild(0).GetComponent<PlayerAnimation>();
        bigMario = transform.GetChild(1).GetComponent<PlayerAnimation>();
    }

    public void Touched()
    {
       if(IsBig) TurnSmall();
       else Invoke(nameof(DeathAnimate),0);
    }

    public void TurnBig()
    {
        capsuleCollider2D.offset = new Vector2(0,0.5f);
        capsuleCollider2D.size = new Vector2(0.9f,2);
    }

    public void TurnSmall()
    {
        capsuleCollider2D.offset = new Vector2(0,0);
        capsuleCollider2D.size = new Vector2(0.8f,1);
    }

    async void DeathAnimate()
    {
        playerController.active = false;
        miniMario.active = false;
        bigMario.active = false;
        capsuleCollider2D.enabled = false;
        miniMario.Touched();

        float elapsed = 0f;

        float jumpHeight = 10f;
        float gravity = -36f;

        Vector3 velocity = Vector2.up * jumpHeight;

        while (elapsed < 3f)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;

            await Task.Yield();
        }
        Destroy(this.gameObject, 0.5f);
    }
}
