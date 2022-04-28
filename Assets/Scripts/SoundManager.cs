using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource SFX;
    [SerializeField] AudioClip bossDefeated;
    [SerializeField] AudioClip death;
    [SerializeField] GameObject buttonAudioPrefab;
    [SerializeField] GameObject restAudioPrefab;

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
    }

    public void BossDefeated()
    {
        SFX.PlayOneShot(bossDefeated, 1);
    }

    public void DeathSoundEffect()
    {
        SFX.PlayOneShot(death, 1);
    }

    public void ButtonSound()
    {
        GameObject buttonAudio = Instantiate(buttonAudioPrefab);
        DontDestroyOnLoad(buttonAudio);
        buttonAudio.GetComponent<AudioSource>().Play();
    }

    public void RestSound()
    {
        GameObject restAudio = Instantiate(restAudioPrefab);
        DontDestroyOnLoad(restAudio);
        restAudio.GetComponent<AudioSource>().Play();
    }
}
