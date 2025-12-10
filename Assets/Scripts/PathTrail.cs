using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTrail : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] ParticleSystem VFX;
    [SerializeField] EventReference fmodEvent;
    public float radius;
    EventInstance fmodInstance;
    float duration;
    float damagePerSecond;
    float damage;
    bool dead = false;
    float cd = 0;
    float maxCd = 0.3f;
    [System.NonSerialized] public AttackProfiles attackProfile;
    List<EnemyScript> touchingEnemies = new List<EnemyScript>();
    [System.NonSerialized] public PlayerTrailManager trailManager;
    [System.NonSerialized] public bool initializedCorrectly = false;

    public static PathTrail Instantiate(GameObject pathTrailPrefab, Vector3 spawnPosition, PlayerTrailManager trailManager, AttackProfiles attackProfile)
    {
        PathTrail pathTrail = GameObject.Instantiate(pathTrailPrefab).GetComponent<PathTrail>();
        pathTrail.transform.position = spawnPosition;
        pathTrail.trailManager = trailManager;
        pathTrail.attackProfile = attackProfile;
        pathTrail.initializedCorrectly = true;
        return pathTrail;
    }

    private void Start()
    {
        if (!initializedCorrectly)
        {
            Utils.IncorrectInitialization("PathTrail");
        }

        if (!trailManager.HasSpace(transform.position, radius))
        {
            Destroy(gameObject);
            return;
        }
        trailManager.pathTrails.Add(this);
        duration = attackProfile.durationDOT;
        damagePerSecond = 1f + playerData.arcane * attackProfile.magicDamageMultiplier;
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        fmodInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        fmodInstance.setTimelinePosition(Random.Range(0, 2000));
    }

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

        if (cd > 0 || damage < 1)
        {
            cd -= Time.deltaTime;
            return;
        }

        cd = maxCd;
        foreach (EnemyScript enemy in touchingEnemies)
        {
            enemy.LoseHealthUnblockable(Mathf.FloorToInt(damage), 0);
        }
        damage = 0;
    }

    void Death()
    {
        dead = true;
        VFX.Stop();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
        trailManager.pathTrails.Remove(this);
    }
}
