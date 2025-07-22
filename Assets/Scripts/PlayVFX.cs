using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    EnemyEvents enemyEvents;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    public void PlayParticleSystems()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    private void OnEnable()
    {
        enemyEvents.OnTriggerVfx += EnemyEvents_OnTriggerVfx;
    }

    private void OnDisable()
    {
        enemyEvents.OnTriggerVfx -= EnemyEvents_OnTriggerVfx;
    }

    private void EnemyEvents_OnTriggerVfx(object sender, string name)
    {
        if (name == "land") PlayParticleSystems();
    }
}
