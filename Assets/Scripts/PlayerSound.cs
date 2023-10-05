using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSFX
{
    FOOTSTEP, DODGE, HEAL, SHIELD, BIGSMASH, RING
}

public class PlayerSound : MonoBehaviour
{
    AudioSource SFX;
    [SerializeField] AudioSource weaponMagicSFX;
    [SerializeField] List<AudioClip> soundEffects = new List<AudioClip>();
    [SerializeField] EventReference[] fmodSoundEffects;
    Dictionary<PlayerSFX, int> playerSFXDict;

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
        playerSFXDict = new Dictionary<PlayerSFX, int>()
        {
            {PlayerSFX.FOOTSTEP, 0 },
            {PlayerSFX.DODGE, 1 },
            {PlayerSFX.HEAL, 2 },
            {PlayerSFX.SHIELD, 3 },
            {PlayerSFX.BIGSMASH, 4 },
            {PlayerSFX.RING, 5 }
        };
    }

    public void PlaySoundEffect(AudioClip clip, float volume)
    {
        SFX.PlayOneShot(clip, volume); 
    }

    public void PlaySoundEffect(PlayerSFX playerSFX, float volume)
    {
        RuntimeManager.PlayOneShot(fmodSoundEffects[playerSFXDict[playerSFX]], volume, transform.position);
    }
}
