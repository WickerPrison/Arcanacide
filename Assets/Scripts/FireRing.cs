using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : MonoBehaviour
{
    [SerializeField] ParticleSystem fireRingFront;
    [SerializeField] ParticleSystem fireRingBack;
    [SerializeField] EventReference fireRingSFX;

    public void Explode()
    {
        FmodUtils.PlayOneShot(fireRingSFX, 1);
        fireRingBack.Play();
        fireRingFront.Play();
    }
}
