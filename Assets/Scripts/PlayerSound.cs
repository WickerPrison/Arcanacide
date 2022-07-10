using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    AudioSource SFX;
    [SerializeField] List<AudioClip> soundEffects = new List<AudioClip>();

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
    }

    public void Footstep()
    {
        SFX.PlayOneShot(soundEffects[0], 0.75f);
    }

    public void SwordSwoosh()
    {
        SFX.PlayOneShot(soundEffects[1], 2);
    }

    public void Pain()
    {
        SFX.PlayOneShot(soundEffects[2], 0.05f);
    }

    public void SwordImpact()
    {
        SFX.PlayOneShot(soundEffects[3], .5f);
    }

    public void SwordClang()
    {
        SFX.PlayOneShot(soundEffects[4], 1);
    }

    public void Dodge()
    {
        SFX.PlayOneShot(soundEffects[5], 0.5f);
    }

    public void Heal()
    {
        SFX.PlayOneShot(soundEffects[6], 0.6f);
    }

    public void Magic()
    {
        SFX.clip = soundEffects[7];
        SFX.Play();
    }

    public void PerfectDodge()
    {
        SFX.PlayOneShot(soundEffects[8], 2f);
    }

    public void Shield()
    {
        SFX.PlayOneShot(soundEffects[9], 1);
    }
    public void StopSoundEffect()
    {
        SFX.Stop();
    }
}
