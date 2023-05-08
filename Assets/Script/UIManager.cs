using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreTXT;
    public TextMeshProUGUI worldTXT;
    public TextMeshProUGUI coinsTXT;
    public TextMeshProUGUI timeTXT;
    public TextMeshProUGUI liveTXT;
    public GameObject blackScreen;
    public GameObject GameOver;
    public GameObject Pause;
    public GameObject SocrePopUp;

    void Start()
    {
        GameManager.Instance.uIManager = this;
        SetLiveTXT(GameManager.Instance.lives);
        SetScoreTXT(GameManager.Instance.score);
        SetCoinTXT(GameManager.Instance.coins);
        SetTimeTXT(GameManager.Instance.time);
        SetWorldTXT(GameManager.Instance.world,GameManager.Instance.stage);
    }

    public void CreatePopUp(string content, Vector3 position)
    {
        PopUp popUp = Instantiate(SocrePopUp,position,Quaternion.identity).GetComponent<PopUp>();
        popUp.Setup(content);
    }

    public void SetLiveTXT(int live)
    {
        liveTXT.text = "x" + live;
    }
    
    public void SetTimeTXT(float time)
    {
        timeTXT.text = string.Format("{0:000}", ((int)time));
    }

    public void SetScoreTXT(int score)
    {
        ScoreTXT.text = string.Format("{0:000000}", score);
    }

    public void SetCoinTXT(int coins)
    {
        coinsTXT.text = string.Format("x" + "{0:00}", coins);
    }

    public void SetWorldTXT(int world, int stage)
    {
        worldTXT.SetText( world + "_" + stage);
    }
}
