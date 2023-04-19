using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    Player player;

    public float speed = 2; 

    void Start()
    {
        gameManager  = GameManager.Instance;
        player = gameManager.player;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x,Mathf.Max(pos.x,player.transform.position.x),Time.deltaTime * speed);
        transform.position = pos;
    }
}
