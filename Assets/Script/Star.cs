using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : PowerUp
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {        
            Eated();
            player.StarPowerOn();
        }
    }

}
