using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    PlayerController controller;
    
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public List<Sprite> run;

    int currentSprite = 0;
    bool runAnimating = false;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponentInParent<PlayerController>();
    }

    void LateUpdate()
    {   
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
}
