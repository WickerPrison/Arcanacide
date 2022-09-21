using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] bool fadeOutMusic = true;
    [SerializeField] bool immediateStopMusic = false;
    [SerializeField] int trackNumber = 0;
    MusicPlayer musicPlayer;
    AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        musicSource = musicPlayer.gameObject.GetComponent<AudioSource>();

        if (immediateStopMusic)
        {
            musicSource.Stop();
            return;
        }

        if (fadeOutMusic)
        {
            StartFadeOut(4);
        }
        else
        {
            musicPlayer.EndFadeOut();

            if (trackNumber != musicPlayer.currentTrack || !musicSource.isPlaying)
            {
                musicPlayer.PlayMusic(trackNumber, 0.5f);
            }
        }
    }

    public void StartFadeOut(int duration)
    {
        musicPlayer.StartFadeOut(duration);
    }
}
