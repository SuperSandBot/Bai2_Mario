using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public int world {get; private set;}
    public int stage {get; private set;}
    public int lives {get; private set;}
    public int coins {get; private set;}
    public float time {get; private set;}
    public int score {get; private set;}
    public UIManager uIManager;
    public bool running = false;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);  
    }
    
    void Start()
    {
        NewGame();
    }
    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            running = !running;
            uIManager.Pause.SetActive(!running);
            if(running == false)
            {
                SoundManager.StopMusic();
                SoundManager.PlaySound(Sound.Pause);
            }
            else
            {
                SoundManager.ContinueMusic();
            }
        }

        if(running == false) return;

        time -= Time.deltaTime;
        if(time <= 0)
        {
            running = false;
            GameOver();
        }

        uIManager.SetTimeTXT(time);
    }

    public async void NewGame()
    {
        LoadLevel(1,1);
        lives = 3;
        time = 400;
        coins = 0;
        score = 0;
        await Task.Delay(1000);
        running = true;
    }

    public async void NextStage()
    {
        SoundManager.StopMusic();
        SoundManager.PlaySound(Sound.StageClear);
        uIManager.blackScreen.SetActive(true);
        await Task.Delay(500);
        uIManager.SetLiveTXT(lives);
        await Task.Delay(5000);
        time = 400;
        score = 0;
        LoadLevel(world, stage + 1);
        await Task.Delay(1000);
        running = true;
    }

    public void OnDeath()
    {
        running = false;
        lives--;
        if(lives > 0)
        {
            ResetLevel();
        }
        else
        {
            Invoke(nameof(GameOver),1f);
        }
    }

    public async void ResetLevel()
    {
        SoundManager.StopMusic();
        uIManager.blackScreen.SetActive(true);
        await Task.Delay(500);
        uIManager.SetLiveTXT(lives);
        await Task.Delay(500);
        time = 400;
        score = 0;
        LoadLevel(world, stage);
        await Task.Delay(1000);
        running = true;
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;
        SceneManager.LoadScene(world + "_" + stage);
    }

    public void GameOver()
    {
        SoundManager.StopMusic();
        SoundManager.PlaySound(Sound.Gameover);
        uIManager.GameOver.SetActive(true);
        Invoke(nameof(NewGame),5f);
    }

    public void AddCoin()
    {
        SoundManager.PlaySound(Sound.Coin);
        coins++;
        if(coins > 99)
        {
            coins = 0;
            AddLife();
        }

        uIManager.SetCoinTXT(coins);
    }

    public void AddLife()
    {
        SoundManager.PlaySound(Sound.LiveUp);
        uIManager.CreatePopUp("1UP",player.transform.position);
        lives++;
    }

    public void AddScoreRaw(int score)
    {
        this.score += score;
        if(this.score > 999999)
        {
            this.score = 0;
            AddLife();
        }

        uIManager.SetScoreTXT(this.score);
    }

    public void AddScore(int score, Vector3 position)
    {
        this.score += score;
        uIManager.CreatePopUp(score.ToString(),position);
        uIManager.SetScoreTXT(this.score);
    }
}
