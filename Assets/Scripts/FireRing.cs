using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : MonoBehaviour
{
    [SerializeField] ParticleSystem fireRingFront;
    [SerializeField] ParticleSystem fireRingBack;
    [SerializeField] AudioClip fireRingSFX;
    [SerializeField] AudioSource SFX;

    public void Explode()
    {
        SFX.PlayOneShot(fireRingSFX, 1);
        fireRingBack.Play();
        fireRingFront.Play();
    }
}
