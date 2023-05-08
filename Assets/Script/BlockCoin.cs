using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BlockCoin : Block
{
    public GameObject coinPrefab;
    public override void BlockHit()
    {
        CoinHitAnimate();
        base.BlockHit();
        GameManager.Instance.AddCoin();
        GameManager.Instance.AddScoreRaw(200);
    }

    public async void CoinHitAnimate()
    {
        GameObject coin = Instantiate(coinPrefab,transform.position,Quaternion.identity);

        Vector3 startPoint = coin.transform.position;
        Vector3 toPoint = coin.transform.position + Vector3.up * 2f;
        float animateDuration = 0.2f;
        await MoveTarget(coin.transform,coin.transform.position,toPoint,animateDuration/2);
        await MoveTarget(coin.transform,coin.transform.position,startPoint,animateDuration/2);
        Destroy(coin);
    }
}
