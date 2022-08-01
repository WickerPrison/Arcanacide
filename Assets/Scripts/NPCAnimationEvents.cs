using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationEvents : MonoBehaviour
{
    [SerializeField] AudioSource sfx;
    [SerializeField] ParticleSystem vfx;

    public void SoundEffect()
    {
        sfx.Play();
    }

    public void VideoEffects()
    {
        vfx.Play();
    }
}
