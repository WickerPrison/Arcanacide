using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceChunk : MonoBehaviour
{
    [SerializeField] int limb;
    ParticleSystem particles;
    Renderer particleRenderer;
    SpriteRenderer spriteRenderer;
    HalfGolemController controller;

    private void Awake()
    {
        controller = GetComponentInParent<HalfGolemController>();
        particles = GetComponent<ParticleSystem>();
        particleRenderer = particles.GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleRenderer.sortingOrder = spriteRenderer.sortingOrder;
    }
    private void onIceBreak(object sender, System.EventArgs e)
    {
        if(controller.remainingIce == limb)
        {
            particles.Play();
            spriteRenderer.enabled = false;
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
