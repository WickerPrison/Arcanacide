using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] EventReference death;
    [SerializeField] EventReference buttonSFX;

    public void DeathSoundEffect()
    {
        RuntimeManager.PlayOneShot(death, 1f);
    }

    public void ButtonSound()
    {
        RuntimeManager.PlayOneShot(buttonSFX);
    }
}
