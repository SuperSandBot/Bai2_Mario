using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPowerUp : Block
{
    public GameObject mushroomPrefab;
    public float animateDuration = 0.65f;
    public override void BlockOnHit()
    {
        base.BlockOnHit();
        PowerUpAnimate();
    }

    public async void PowerUpAnimate()
    {
        GameObject mushroom = Instantiate(mushroomPrefab,transform.position + Vector3.up * 0.5f,Quaternion.identity);
        Vector3 startPoint = mushroom.transform.position;
        Vector3 toPoint = mushroom.transform.position + Vector3.up * 0.5f;
        await MoveTarget(mushroom.transform,mushroom.transform.position,toPoint,animateDuration);
        mushroom.GetComponent<PowerUp>().Activate();
    }
}
