using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int hitCount = -1;
    public Sprite emptyBlock;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;
    bool animating = false;
    public bool unBreakAble;
    public bool hidden = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (hidden) spriteRenderer.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (hitCount == 0)
        {
            SoundManager.PlaySound(Sound.Bump);
            return;
        }
        if (animating) return;
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (this.transform.DotTest(other.transform, Vector2.up))
            {
                if (unBreakAble)
                {
                    BlockHit();
                    return;
                }

                if (player.IsBig)
                {
                    BlockBreak();
                }
                else
                {
                    BlockHit();
                }
            }
        }
    }

    public virtual void BlockHit()
    {
        if (hidden) spriteRenderer.enabled = true;
        hitCount--;
        BlockHitAnimate();
        AboveCheck();
        if (hitCount == 0) spriteRenderer.sprite = emptyBlock;
    }

    public void BlockBreak()
    {
        SoundManager.PlaySound(Sound.Break);
        if (hidden) spriteRenderer.enabled = true;
        BlockBreakAnimate();
        AboveCheck();
    }

    public void AboveCheck()
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(boxCollider2D.bounds.center, boxCollider2D.size, 0, Vector2.up, 0.5f, LayerMask.GetMask("Enemy","Coin"));
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                Coin coin = hit[i].collider.gameObject.GetComponent<Coin>();
                if (coin != null)
                {
                    coin.OnHit();
                    return;
                }
                Goomba goomba = hit[i].collider.gameObject.GetComponent<Goomba>();
                if (goomba != null)
                {
                    GameManager.Instance.AddScore(100,transform.position);
                    goomba.DeathAnimate();
                    return;
                }
                Koopa koopa = hit[i].collider.gameObject.GetComponent<Koopa>();
                if (koopa != null)
                {
                    GameManager.Instance.AddScore(100,transform.position);
                    koopa.DeathAnimate();
                    return;
                }
            }
        }
    }

    public async void BlockBreakAnimate()
    {
        animating = true;
        Vector3 startPoint = transform.position;
        Vector3 toPoint = transform.position + Vector3.up * 0.5f;
        float animateDuration = 0.15f;
        await MoveTarget(this.transform, transform.position, toPoint, animateDuration / 2);
        boxCollider2D.enabled = false;
        spriteRenderer.sprite = null;
        animating = false;
    }

    public async void BlockHitAnimate()
    {
        animating = true;
        Vector3 startPoint = transform.position;
        Vector3 toPoint = transform.position + Vector3.up * 0.5f;
        float animateDuration = 0.15f;
        await MoveTarget(this.transform, transform.position, toPoint, animateDuration / 2);
        await MoveTarget(this.transform, transform.position, startPoint, animateDuration / 2);
        animating = false;
    }

    public async Task MoveTarget(Transform target, Vector3 form, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.transform.localPosition = Vector3.Lerp(form, to, elapsed / duration);
            elapsed += Time.deltaTime;

            await Task.Yield();
        }
        target.transform.localPosition = to;
    }
}
