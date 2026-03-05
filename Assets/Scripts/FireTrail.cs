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
    bool disableBecauseBoss = false;


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
        if (disableBecauseBoss) return;
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

    private void Global_onBossKilled(object sender, System.EventArgs e)
    {
        disableBecauseBoss = true;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onBossKilled += Global_onBossKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onBossKilled -= Global_onBossKilled;
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();        
    }
}
