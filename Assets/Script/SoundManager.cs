using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound{
        BGM = 0,
        Gameover = 1,
        StageClear = 2,
        Die = 3,
        LiveUp = 4,
        Kick = 5,
        JumpBig = 6,
        JumpSmall = 7,
        Coin = 8,
        Stomp = 9,
        Bump = 10,
        Break = 11,
        Pause = 12,
        Grow = 13,
        Shrink = 14,
        StartPower = 15,
    }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    public List<AudioClip> audioLists;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        PlayLevelBGM();
    }
    public static void PlaySound(Sound soundType)
    {
        Instance.effectsSource.PlayOneShot(Instance.audioLists[(int)soundType]);
    }

    public static void PlayLevelBGM()
    {
        Instance.musicSource.clip = Instance.audioLists[(int)Sound.BGM];
        Instance.musicSource.Play();
    }

    public static void PlayMusic(Sound soundType)
    {
        Instance.musicSource.clip = Instance.audioLists[(int)soundType];
        Instance.musicSource.Play();
    }

    public static void StopMusic()
    {
        Instance.musicSource.Stop();
    }

    public static void PauseMusic()
    {
        Instance.musicSource.Pause();
    }

    public static void ContinueMusic()
    {
        Instance.musicSource.Play();
    }

}
