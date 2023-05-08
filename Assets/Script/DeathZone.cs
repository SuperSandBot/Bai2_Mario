using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Invoke(nameof(player.ForceDeath),0.5f);
            return;
        }
        
        Destroy(other.gameObject,1.5f);
    }
}
