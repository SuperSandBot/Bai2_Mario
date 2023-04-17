using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    GameManager gameManager;
    Player player;

    void Start()
    {
        gameManager = GameManager.Instance;
        player = gameManager.player;
    }

    void Update()
    {
        if(player.playerController.Groundcheck() == false) return;

        int dir = (int)Input.GetAxisRaw("Horizontal");
        player.playerController.Movement(dir);

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            player.playerController.Jump();
        }
    }
}
