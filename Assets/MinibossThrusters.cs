using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossThrusters : MonoBehaviour
{
    MinibossAnimationEvents animationEvents;
    ParticleSystem particles;

    private void Awake()
    {
        animationEvents = GetComponentInParent<MinibossAnimationEvents>();
        particles = GetComponent<ParticleSystem>();
    }

    private void AnimationEvents_onThrustersOn(object sender, System.EventArgs e)
    {
        particles.Play();
    }

    private void AnimationEvents_onThrustersOff(object sender, System.EventArgs e)
    {
        particles.Stop();
        particles.Clear();
    }

    private void OnEnable()
    {
        animationEvents.onThrustersOn += AnimationEvents_onThrustersOn;
        animationEvents.onThrustersOff += AnimationEvents_onThrustersOff;
    }

    private void OnDisable()
    {
        animationEvents.onThrustersOn -= AnimationEvents_onThrustersOn;
        animationEvents.onThrustersOff -= AnimationEvents_onThrustersOff;
    }
}
