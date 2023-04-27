using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;

    public int world {get; private set;}
    public int stage {get; private set;}
    public int lives {get; private set;}
    public int coins {get; private set;}

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);  
    }
    
    void Start()
    {
        
    }

    public void NewGame()
    {

    }

    public void NextStage()
    {

    }

    public void OnDeath()
    {
        if(lives > 0)
        {
            lives--;
        }
        else
        {
            GameOver();
        }
    }

    public void LoadLevel(int world, int stage)
    {

    }

    public void GameOver()
    {

    }

    public void AddCoin()
    {
        coins++;
    }

    public void AddLife()
    {

    }
}
