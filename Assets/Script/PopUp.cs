using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUp : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void Setup(string content)
    {
        text.SetText(content);
        Destroy(gameObject,1f);
    }

    void Update()
    {
        transform.position = transform.position + Vector3.up * 2f * Time.deltaTime;
    }
}
