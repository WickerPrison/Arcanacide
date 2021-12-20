using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : MonoBehaviour
{
    [SerializeField] ParticleSystem fireRingFront;
    [SerializeField] ParticleSystem fireRingBack;

    public void Explode()
    {
        fireRingBack.Play();
        fireRingFront.Play();
    }
}
