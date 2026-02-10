using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationEvents : MonoBehaviour
{
    [SerializeField] ParticleSystem vfx;
    [SerializeField] EventReference fmodEvent;

    public void SoundEffect(float volume)
    {
        FmodUtils.PlayOneShot(fmodEvent, volume, transform.position);
    }

    public void VideoEffects()
    {
        vfx.Play();
    }
}
