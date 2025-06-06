using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVfxListener : MonoBehaviour
{
    [SerializeField] string vfxName;
    ParticleSystem particles;
    EnemyEvents enemyEvents;

    // Start is called before the first frame update
    void Awake ()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
        particles = GetComponent<ParticleSystem>();
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
        if(vfxName == name)
        {
            particles.Play();
        }
    }
}
