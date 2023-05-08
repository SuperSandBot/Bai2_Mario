using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    CircleCollider2D circleCollider2D;
    EnemyController enemyController;
    public Sprite stompedSprite;
    public List<Sprite> run;
    SpriteRenderer spriteRenderer;
    int currentSprite = 0;
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
        enemyController.active = false;
        circleCollider2D.enabled = false;
        CancelInvoke();
        spriteRenderer.sprite = stompedSprite;
        Destroy(this.gameObject, 0.5f);
    }

    public async void DeathAnimate()
    {
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
                GameManager.Instance.AddScore(100,transform.position);
                Stomped();
            }
            else
            {
                player.Touched();
            }
        }
    }
}
