using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public void OnTouch()
    {
        GameManager.Instance.AddCoin();
        GameManager.Instance.AddScoreRaw(200);
        Destroy(this.gameObject);
    }

    public void OnHit()
    {
        GameManager.Instance.AddCoin();
        GameManager.Instance.AddScoreRaw(200);
        CoinHitAnimate();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            OnTouch();
        }
    }

    public async void CoinHitAnimate()
    {
        Vector3 startPoint = transform.position;
        Vector3 toPoint = transform.position + Vector3.up * 2f;
        float animateDuration = 0.2f;
        await MoveTarget(transform,transform.position,toPoint,animateDuration/2);
        await MoveTarget(transform,transform.position,startPoint,animateDuration/2);
        Destroy(this.gameObject);
    }

    public async Task MoveTarget(Transform target, Vector3 form, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.transform.localPosition = Vector3.Lerp(form, to, elapsed / duration);
            elapsed += Time.deltaTime;

            await Task.Yield();
        }
        target.transform.localPosition = to;
    }
}
