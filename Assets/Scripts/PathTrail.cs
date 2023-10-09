using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathTrail : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    ParticleSystem VFX;
    [SerializeField] EventReference fmodEvent;
    EventInstance fmodInstance;
    float duration = 3;
    float damagePerSecond;
    float damage;
    bool dead = false;

    private void Start()
    {
        VFX = GetComponent<ParticleSystem>();
        damagePerSecond = 5 + playerData.arcane;
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        fmodInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        fmodInstance.start();
        fmodInstance.setTimelinePosition(Random.Range(0, 2000));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !dead)
        {
            EnemyScript enemyScript;
            enemyScript = other.gameObject.GetComponent<EnemyScript>();
            damage += damagePerSecond * Time.deltaTime;
            if (damage > 1)
            {
                enemyScript.LoseHealth(Mathf.FloorToInt(damage), 0);
                damage = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            if (!dead)
            {
                duration += 2;
                Death();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Death()
    {
        dead = true;
        VFX.Stop();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }
}
