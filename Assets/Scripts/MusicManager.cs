using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(int musicIndex)
    {
        audioSource.clip = audioClips[musicIndex];
        audioSource.Play();
    }

    public void StartFadeOut(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }

    IEnumerator FadeOut(float duration)
    {
        float rate = audioSource.volume / duration;

        float timer = duration;

        while (timer > 0)
        {
            Debug.Log(timer);
            timer -= Time.unscaledDeltaTime;
            audioSource.volume -= rate * Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
