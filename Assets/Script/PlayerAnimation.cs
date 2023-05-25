using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    PlayerController controller;
    
    public Sprite touched;
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public List<Sprite> run;

    int currentSprite = 0;
    bool runAnimating = false;
    
    public bool active = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponentInParent<PlayerController>();
    }

    void LateUpdate()
    {   
        if(!active) return;
        if(controller.jumping)
        {
            CancelInvoke();
            runAnimating = false;
            spriteRenderer.sprite = jump;
        }
        else if(controller.sliding)
        {
            CancelInvoke();
            runAnimating = false;
            spriteRenderer.sprite = slide;
        }
        else if(controller.running)
        {
            if(runAnimating == false)
            {
                runAnimating = true;
                currentSprite = 0;
                InvokeRepeating(nameof(RunAnimate),0,.1f);
            }
        }
        else
        {
            CancelInvoke();
            runAnimating = false;
            spriteRenderer.sprite = idle;
        }
    }

    void RunAnimate()
    {
        currentSprite++;
        if(currentSprite >= run.Count) currentSprite = 0;

        spriteRenderer.sprite = run[currentSprite];
    }

    public void Death()
    {
        CancelInvoke();
        spriteRenderer.sprite = touched;
    }
}
