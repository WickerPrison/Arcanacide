using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float cd = 0;
    float maxCd = 0.3f;
    List<EnemyScript> touchingEnemies = new List<EnemyScript>();
    [System.NonSerialized] public List<PathTrail> pathTrails;

    private void Start()
    {
        if (!HasSpace())
        {
            Destroy(gameObject);
            return;
        }
        if(pathTrails != null) pathTrails.Add(this);
        VFX = GetComponentInChildren<ParticleSystem>();
        damagePerSecond = 1 + playerData.arcane * 0.5f;
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        fmodInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        fmodInstance.setTimelinePosition(Random.Range(0, 2000));
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Enemy") && !dead)
    //    {
    //        EnemyScript enemyScript;
    //        enemyScript = other.gameObject.GetComponent<EnemyScript>();
    //        damage += damagePerSecond * Time.deltaTime;
    //        if (damage > 1)
    //        {
    //            enemyScript.LoseHealth(Mathf.FloorToInt(damage), 0);
    //            damage = 0;
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            touchingEnemies.Add(other.gameObject.GetComponent<EnemyScript>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            touchingEnemies.Remove(other.gameObject.GetComponent<EnemyScript>());
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

        damage += damagePerSecond * Time.deltaTime;

        if(cd > 0 || damage < 1)
        {
            cd -= Time.deltaTime;
            return;
        }

        cd = maxCd;
        foreach(EnemyScript enemy in touchingEnemies)
        {
            enemy.LoseHealth(Mathf.FloorToInt(damage), 0);
        }
        damage = 0;
    }

    bool HasSpace()
    {
        if (pathTrails == null) return true;
        foreach(PathTrail pathTrail in pathTrails)
        {
            if(Vector3.Distance(pathTrail.transform.position, transform.position) < 0.7)
            {
                return false;
            }
        }
        return true;
    }

    void Death()
    {
        dead = true;
        VFX.Stop();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
        if(pathTrails != null) pathTrails.Remove(this);
    }
}
