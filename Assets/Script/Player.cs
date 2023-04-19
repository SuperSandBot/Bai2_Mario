using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController playerController;
    PlayerAnimation miniMario;
    PlayerAnimation bigMario;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        miniMario = transform.GetChild(0).GetComponent<PlayerAnimation>();
        bigMario = transform.GetChild(1).GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        
    }
    
 
}
