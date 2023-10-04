using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSFX
{
    FOOTSTEP, DODGE
}

public class PlayerSound : MonoBehaviour
{
    AudioSource SFX;
    [SerializeField] AudioSource weaponMagicSFX;
    [SerializeField] List<AudioClip> soundEffects = new List<AudioClip>();
    [SerializeField] EventReference[] fmodSoundEffects;
    StudioEventEmitter fmodEmitter;
    Dictionary<PlayerSFX, int> playerSFXDict;

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
        fmodEmitter = GetComponent<StudioEventEmitter>();
        playerSFXDict = new Dictionary<PlayerSFX, int>()
        {
            {PlayerSFX.FOOTSTEP, 0 },
            {PlayerSFX.DODGE, 1 }
        };
    }

    public void PlaySoundEffect(AudioClip clip, float volume)
    {
        SFX.PlayOneShot(clip, volume); 
    }

    public void PlaySoundEffectFromList(PlayerSFX playerSFX)
    {
        RuntimeManager.PlayOneShot(fmodSoundEffects[playerSFXDict[playerSFX]], transform.position);
    }

    public void PlaySoundEffectFromList2(int index, float volume)
    {
        SFX.PlayOneShot(soundEffects[index], volume);
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

    public void WeaponMagic()
    {
        weaponMagicSFX.Play();
    }

    public void StopWeaponMagic()
    {
        weaponMagicSFX.Stop();
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
