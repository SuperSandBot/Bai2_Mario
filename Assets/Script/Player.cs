using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController playerController;
    

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }
    
 
}
