using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject musicPlayerPrefab;
    [SerializeField] bool immediateStopMusic = false;
    [SerializeField] Music musicOption;
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

    private void onBossKilled(object sender, System.EventArgs e)
    {
        StopFadeOut();
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onBossKilled += onBossKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onBossKilled -= onBossKilled;
    }

}
