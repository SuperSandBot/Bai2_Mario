using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    CircleCollider2D circleCollider2D;
    EnemyController enemyController;
    public Sprite stompedSprite;
    public List<Sprite> run;
    SpriteRenderer spriteRenderer;
    int currentSprite = 0;

    public bool shelled;
    public bool shellMoving => (shelled == true && enemyController.direction != Vector2.zero);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        enemyController = GetComponent<EnemyController>();
        InvokeRepeating(nameof(RunAnimate),0,.2f);
    }

    void RunAnimate()
    {
        currentSprite++;
        if(currentSprite >= run.Count) currentSprite = 0;

        spriteRenderer.sprite = run[currentSprite];
    }

    public void Stomped()
    {
        SoundManager.PlaySound(Sound.Stomp);
        shelled = true;
        enemyController.direction = Vector2.zero;
        enemyController.layerMask = LayerMask.GetMask("Platform");
        this.gameObject.layer = LayerMask.NameToLayer("Shelled");
        CancelInvoke();
        spriteRenderer.sprite = stompedSprite;
    }

    public async void DeathAnimate()
    {
        SoundManager.PlaySound(Sound.Kick);
        enemyController.active = false;
        circleCollider2D.enabled = false;
        CancelInvoke();
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {
            if(player.starPower)
            {
                GameManager.Instance.AddScore(100,transform.position);
                DeathAnimate();
                return;
            }
            if(this.transform.DotTest(player.transform,Vector2.down))
            {
                if(shelled)
                {     
                    if(shellMoving) 
                    {
                        ShellMovement(Vector2.zero);
                        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
                    }
                    else 
                    {
                        if(transform.position.x - player.transform.position.x > 0) ShellMovement(Vector2.right);
                        else ShellMovement(Vector2.left);       
                    }
                }
                else
                {
                    GameManager.Instance.AddScore(100,transform.position);
                    Stomped();
                }
            }
            else
            {
                if(shelled)
                {
                    if(shellMoving) player.Touched();
                }
                else
                {
                    player.Touched();
                }
            }

            return;
        }

        if(shellMoving)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Goomba goomba = other.gameObject.GetComponent<Goomba>();
                if(goomba != null) 
                {
                    GameManager.Instance.AddScore(100,transform.position);
                    goomba.DeathAnimate();
                    return;
                }
                Koopa koopa = other.gameObject.GetComponent<Koopa>();
                if(koopa != null)
                {
                    GameManager.Instance.AddScore(100,transform.position);
                    koopa.DeathAnimate();
                    return;
                }
            }

            if(other.gameObject.layer == LayerMask.NameToLayer("Shelled"))
            {
                enemyController.direction = -enemyController.direction;
            }
        }
        
    }

    public void ShellMovement(Vector2 direction)
    {
        SoundManager.PlaySound(Sound.Stomp);
        enemyController.speed = 12;
        enemyController.direction = direction;
    }
}
