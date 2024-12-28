using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    [SerializeField] EventReference fmodEvent;
    ParticleSystem particles;
    PlayerAbilities playerAbilities;
    PlayerScript playerScript;
    EventInstance fmodInstance;
    public int damage;


    private void Start()
    {
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        //fmodInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        fmodInstance.start();
        fmodInstance.setTimelinePosition(Random.Range(0, 2000));
        fmodInstance.setVolume(0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(playerScript == null)
            {
                playerScript = collision.gameObject.GetComponent<PlayerScript>();
            }
            playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
            playerScript.StartStagger(0.5f);
        }
    }

    private void OnDisable()
    {
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }
}
