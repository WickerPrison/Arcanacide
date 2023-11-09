using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;

    public void PlayParticleSystems()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
}
