using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireWave : MonoBehaviour, IDamageEnemy
{
    [SerializeField] Collider hitBox;
    [SerializeField] GameObject fireWaveTrailPrefab;
    [SerializeField] EventReference enemyImpactSFX;
    [SerializeField] EventReference impactSFX;
    [SerializeField] float impactSFXvolume;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] AttackProfiles attackProfile;
    private PlayerTrailManager trailManager;
    bool launched = false;
    float speed = 15;
    float dist;
    float addedDOT;
    public bool blockable { get; set; } = true;
    private bool instantiatedCorrectly = false;

    public static PlayerFireWave Instantiate(GameObject prefab, Vector3 spawnPosition, Vector3 direction, PlayerTrailManager trailManager)
    {
        PlayerFireWave wave = Instantiate(prefab).GetComponent<PlayerFireWave>();
        wave.transform.position = spawnPosition;
        wave.transform.LookAt(spawnPosition + direction);
        wave.trailManager = trailManager;
        wave.instantiatedCorrectly = true;
        return wave;
    }

    private void Start()
    {
        if (!instantiatedCorrectly)
        {
            Utils.IncorrectInitialization("PlayerFireWave");
        }
    }

    private void FixedUpdate()
    {
        if (launched)
        {
            transform.Translate(Time.fixedDeltaTime * speed * Vector3.forward);
            dist += Time.fixedDeltaTime * speed;
            if(dist >= 0.9)
            {
                SpawnTrailElement();
                dist = 0;
            }
        }
    }

    public void LaunchFireWave()
    {
        hitBox.enabled = true;
        launched = true;
        SpawnTrailElement();
    }

    void SpawnTrailElement()
    {
        PlayerFireWaveTrail.Instantiate(fireWaveTrailPrefab, transform.position, transform.rotation, trailManager);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(collision);
        }
        else
        {
            HitObject(collision);
        }
    }

    void HitEnemy(Collider collision)
    {
        EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();
        int damage = Mathf.RoundToInt(playerData.ArcaneDamage() * attackProfile.magicDamageMultiplier);
        if (attackProfile.attackType == AttackType.SPECIAL && playerData.equippedPatches.Contains(Patches.ARCANE_MASTERY))
        {
            damage += Mathf.RoundToInt(damage * emblemLibrary.arcaneMastery.value);
        }
        enemyScript.LoseHealth(damage, damage * attackProfile.poiseDamageMultiplier, this, () =>
        {
            enemyScript.ImpactVFX();
            if (attackProfile.attackType == AttackType.DEFLECT && playerData.equippedPatches.Contains(Patches.BURNING_REFLECTION))
            {
                addedDOT = 10;
            }
            else addedDOT = 0;
            enemyScript.GainDOT(attackProfile.durationDOT + addedDOT);
            RuntimeManager.PlayOneShot(impactSFX, impactSFXvolume, transform.position);
        });
    }

    void HitObject(Collider collision)
    {
        RuntimeManager.PlayOneShot(impactSFX, impactSFXvolume, transform.position);
        Destroy(gameObject);
    }
}
