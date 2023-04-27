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
    public bool changing = false;
    public bool starPower = false;
    public float powerDuration = 10f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        miniMario = transform.GetChild(0).GetComponent<PlayerAnimation>();
        bigMario = transform.GetChild(1).GetComponent<PlayerAnimation>();
        bigMario.spriteRenderer.enabled = false;
    }

    public void Touched()
    {
        if(changing) return;
        if(starPower) return;
        if(IsBig) Shrink();
        else
        {
            playerController.active = false;
            miniMario.active = false;
            bigMario.active = false;
            capsuleCollider2D.enabled = false;
            miniMario.Death();
            Invoke(nameof(DeathAnimate),0);
        }
    }

    public void PowerUp()
    {
        if(IsBig == false) Grow();
    }

    public void StarPowerOn()
    {
        starPower = true;
        InvokeRepeating(nameof(StarPowerAnimate), 0, .02f);
    }
    public void StarPowerOff()
    {
        starPower = false;
        bigMario.spriteRenderer.color = Color.white;
        miniMario.spriteRenderer.color = Color.white;
        CancelInvoke();
    }

    void Update()
    {
        if(starPower)
        {
            powerDuration -= Time.deltaTime;
            if(powerDuration <= 0) StarPowerOff();
        }
    }

    public async void Grow()
    {
        IsBig = true;
        changing = true;
        playerController.active = false;
        playerController.rigidbody2D.isKinematic = true;
        await TransferAnimate();
        miniMario.spriteRenderer.enabled = false;
        bigMario.spriteRenderer.enabled = true;
        capsuleCollider2D.offset = new Vector2(0,0.5f);
        capsuleCollider2D.size = new Vector2(0.9f,2);
        playerController.active = true;
        playerController.rigidbody2D.isKinematic = false;
        await Task.Delay(200);
        changing = false;
    }

    public async void Shrink()
    {
        IsBig = false;
        changing = true;
        playerController.active = false;
        playerController.rigidbody2D.isKinematic = true;
        await TransferAnimate();
        miniMario.spriteRenderer.enabled = true;
        bigMario.spriteRenderer.enabled = false;
        capsuleCollider2D.offset = new Vector2(0,0);
        capsuleCollider2D.size = new Vector2(0.8f,1);
        playerController.active = true;
        playerController.rigidbody2D.isKinematic = false;
        await Task.Delay(200);
        changing = false;
    }

    public async void DeathAnimate()
    {
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
    
    public async Task TransferAnimate()
    {
        float elapsed = 0f;
        while (elapsed < 0.75f)
        {         
            elapsed += Time.deltaTime;
            if(Time.frameCount % 16 == 0)
            {
                miniMario.spriteRenderer.enabled = !bigMario.spriteRenderer.enabled;
                bigMario.spriteRenderer.enabled = miniMario.spriteRenderer.enabled;
            }
            await Task.Yield();
        }
    }

    public void StarPowerAnimate()
    {
        Color color = Random.ColorHSV(0f,1f,1f,1f,1f,1f);
        if(IsBig) bigMario.spriteRenderer.color = color;
        else
        {
            miniMario.spriteRenderer.color = color;
        }
    }
}
