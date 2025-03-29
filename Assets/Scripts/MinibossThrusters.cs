using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossThrusters : MonoBehaviour
{
    MinibossEvents minibossEvents;
    ParticleSystem particles;

    private void Awake()
    {
        minibossEvents = GetComponentInParent<MinibossEvents>();
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
        minibossEvents.onThrustersOn += AnimationEvents_onThrustersOn;
        minibossEvents.onThrustersOff += AnimationEvents_onThrustersOff;
    }

    private void OnDisable()
    {
        minibossEvents.onThrustersOn -= AnimationEvents_onThrustersOn;
        minibossEvents.onThrustersOff -= AnimationEvents_onThrustersOff;
    }
}
