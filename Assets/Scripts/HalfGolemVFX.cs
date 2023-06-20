using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfGolemVFX : MonoBehaviour
{
    [SerializeField] int limb;
    ParticleSystem particles;
    HalfGolemController controller;

    private void Awake()
    {
        controller = GetComponentInParent<HalfGolemController>();
        particles = GetComponent<ParticleSystem>();
    }
    private void onIceBreak(object sender, System.EventArgs e)
    {
        if(controller.remainingIce == limb)
        {
            particles.Play();
        }
    }

    private void OnEnable()
    {
        controller.onIceBreak += onIceBreak;
    }

    private void OnDisable()
    {
        controller.onIceBreak -= onIceBreak;
    }
}
