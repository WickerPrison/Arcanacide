using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosSporesScript : MonoBehaviour
{
    ParticleSystem vfx;
    PlayerScript playerScript;
    float chaosSporesDuration;
    float chaosSporesDamageRate = 10;
    float chaosSporesCounter;

    [SerializeField] EventReference sporeSFX;
    [SerializeField] float sfxVolume;
    EventInstance fmodInstance;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        vfx = GetComponent<ParticleSystem>();
        fmodInstance = RuntimeManager.CreateInstance(sporeSFX);
        fmodInstance.setVolume(sfxVolume);
    }

    private void Update()
    {
        if(chaosSporesDuration > 0)
        {
            chaosSporesDuration -= Time.deltaTime;
            if(chaosSporesDuration <= 0)
            {
                vfx.Stop();
                fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            chaosSporesCounter += Time.deltaTime * chaosSporesDamageRate;
            if(chaosSporesCounter > 1)
            {
                int damage = Mathf.FloorToInt(chaosSporesCounter);
                chaosSporesCounter -= damage;
                playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
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

        fmodInstance.start();
    }

    private void OnDisable()
    {
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }
}
