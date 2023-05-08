using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    public float speed = 2; 
    
    void LateUpdate()
    {
        if(GameManager.Instance.running == false) return;
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x,Mathf.Max(pos.x,player.transform.position.x),Time.deltaTime * speed);
        transform.position = pos;
    }
}
