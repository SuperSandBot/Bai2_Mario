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
        if(hidden) spriteRenderer.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(hitCount == 0) return;
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(this.transform.DotTest(other.transform,Vector2.up))
            {
                if(animating == false) BlockOnHit();
            }
        }
    }

    public virtual void BlockOnHit()
    {
        if(hidden) spriteRenderer.enabled = true;
        hitCount--;
        BlockHitAnimate();
        if(hitCount == 0)
        {    
            if(unBreakAble)
            {        
                spriteRenderer.sprite = emptyBlock;
                return;
            }
            boxCollider2D.enabled = false;
            spriteRenderer.sprite = null;
        }
    }

    public async void BlockHitAnimate()
    {
        animating = true;
        Vector3 startPoint = transform.position;
        Vector3 toPoint = transform.position + Vector3.up * 0.5f;
        float animateDuration = 0.15f;

        await MoveTarget(this.transform,transform.position,toPoint,animateDuration/2);
        await MoveTarget(this.transform,transform.position,startPoint,animateDuration/2);

        animating = false;
        
    }

    public async Task MoveTarget(Transform target ,Vector3 form, Vector3 to, float duration)
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
