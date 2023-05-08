using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUpMushroom : PowerUp
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {        
            Eated();
            GameManager.Instance.AddScore(2000,transform.position);
            GameManager.Instance.AddLife();
        }
    }
}
