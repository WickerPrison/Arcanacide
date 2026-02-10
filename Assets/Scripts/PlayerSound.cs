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
    [SerializeField] EventReference[] fmodSoundEffects;
    Dictionary<PlayerSFX, int> playerSFXDict;

    private void Start()
    {
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

    public void PlaySoundEffect(EventReference fmodEvent, float volume)
    {
        FmodUtils.PlayOneShot(fmodEvent, volume);
    }

    public void PlaySoundEffect(PlayerSFX playerSFX, float volume)
    {
        FmodUtils.PlayOneShot(fmodSoundEffects[playerSFXDict[playerSFX]], volume, transform.position);
    }
}
