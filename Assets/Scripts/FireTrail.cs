using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    [SerializeField] EventReference fmodEvent;
    [SerializeField] ParticleSystem particles;
    PlayerAbilities playerAbilities;
    PlayerScript playerScript;
    EventInstance fmodInstance;
    public float duration = 5;
    public float damagePerSecond = 5;
    float damage;


    private void Start()
    {
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        //fmodInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        fmodInstance.start();
        fmodInstance.setTimelinePosition(Random.Range(0, 2000));
        fmodInstance.setVolume(0.1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.layer == 3)
        {
            if (GetPlayerAbilities(other).shield)
            {
                return;
            }
            damage += damagePerSecond * Time.deltaTime;
            if(damage > 1)
            {
                playerScript.LoseHealth(Mathf.RoundToInt(damage), EnemyAttackType.NONPARRIABLE, null);
                damage = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            particles.Stop();
            StartCoroutine(DestroyFireTrail());
        }
    }

    PlayerAbilities GetPlayerAbilities(Collider other)
    {
        if(playerAbilities == null)
        {
            playerAbilities = other.gameObject.GetComponent<PlayerAbilities>();
            playerScript = playerAbilities.GetComponent<PlayerScript>();
        }
        return playerAbilities;
    }

    IEnumerator DestroyFireTrail()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();        
    }
}
