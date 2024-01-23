using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject musicPlayerPrefab;
    [SerializeField] bool immediateStopMusic = false;
    [SerializeField] Music musicOption;
    [SerializeField] MusicState musicState;
    MusicPlayer musicPlayer;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("MusicPlayer") == null)
        {
            Instantiate(musicPlayerPrefab);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();

        if(musicOption == Music.NONE)
        {
            if (immediateStopMusic)
            {
                StopImmediate();
            }
            else
            {
                StopFadeOut();
            }
        }
        else if(musicOption != musicPlayer.currentTrack)
        {
            musicPlayer.PlayMusic(musicOption);
            musicPlayer.ChangeState(musicState);
        }
        else
        {
            musicPlayer.ChangeState(musicState);
        }
    }

    public void StopFadeOut()
    {
        musicPlayer.StopFadeOut();
    }

    public void StopImmediate()
    {
        musicPlayer.StopImmediate();
    }

    public void ChangeMusic(Music music) 
    {
        musicPlayer.PlayMusic(music);
    }

    public void ChangeMusicState(MusicState state)
    {
        musicPlayer.ChangeState(state);
    }

    public void UpdateBossHealth(int healthPercent)
    {
        musicPlayer.UpdateBossHealth(healthPercent);
    }

    private void onPlayerDeath(object sender, System.EventArgs e)
    {
        musicPlayer.ChangeState(MusicState.DEATH);
    }

    private void onBossKilled(object sender, System.EventArgs e)
    {
        musicPlayer.ChangeState(MusicState.BOSSVICTORY);
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onBossKilled += onBossKilled;
        GlobalEvents.instance.onPlayerDeath += onPlayerDeath;
    }


    private void OnDisable()
    {
        GlobalEvents.instance.onBossKilled -= onBossKilled;
        GlobalEvents.instance.onPlayerDeath -= onPlayerDeath;
    }

}
