using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public int currentTrack;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] SettingsData settingsData;
    Coroutine musicFade;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(int musicIndex, float volume)
    {
        currentTrack = musicIndex;
        audioSource.clip = audioClips[musicIndex];
        audioSource.Play();
        audioSource.volume = volume;
        settingsData.SetVolume(VolumeChannel.MASTER, settingsData.masterVol);
        settingsData.SetVolume(VolumeChannel.MUSIC, settingsData.musicVol);
    }

    public void StartFadeOut(float duration)
    {
        musicFade = StartCoroutine(FadeOut(duration));
    }

    public void EndFadeOut()
    {
        if(musicFade == null)
        {
            return;
        }

        StopCoroutine(musicFade);
        audioSource.volume = 1;
    }

    IEnumerator FadeOut(float duration)
    {
        float rate = audioSource.volume / duration;

        float timer = duration;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            audioSource.volume -= rate * Time.unscaledDeltaTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 1;
    }
}
