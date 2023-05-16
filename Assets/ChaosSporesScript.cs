using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosSporesScript : MonoBehaviour
{
    ParticleSystem vfx;
    PlayerScript playerScript;
    float chaosSporesDuration;
    float chaosSporesDamageRate = 5;
    float chaosSporesCounter;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        vfx = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(chaosSporesDuration > 0)
        {
            chaosSporesDuration -= Time.deltaTime;
            if(chaosSporesDuration <= 0)
            {
                vfx.Stop();
            }

            chaosSporesCounter += Time.deltaTime * chaosSporesDamageRate;
            if(chaosSporesCounter > 1)
            {
                int damage = Mathf.FloorToInt(chaosSporesCounter);
                chaosSporesCounter -= damage;
                playerScript.LoseHealth(damage);
            }
        }
    }

    public void StartChaosSpores(float duration)
    {
        vfx.Play();
        if(chaosSporesDuration < duration)
        {
            chaosSporesDuration = duration;
        }
    }
}
