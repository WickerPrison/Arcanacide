using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireWave : MonoBehaviour
{
    [SerializeField] Collider hitBox;
    [SerializeField] GameObject fireWaveTrailPrefab;
    [SerializeField] EventReference enemyImpactSFX;
    [SerializeField] EventReference impactSFX;
    [SerializeField] float impactSFXvolume;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] AttackProfiles attackProfile;
    bool launched = false;
    float speed = 15;
    float dist;
    float addedDOT;

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
        Transform trail = Instantiate(fireWaveTrailPrefab).transform;
        trail.position = transform.position;
        trail.rotation = transform.rotation;
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
        if (attackProfile.attackType == AttackType.SPECIAL && playerData.equippedEmblems.Contains(emblemLibrary.arcane_mastery))
        {
            damage += Mathf.RoundToInt(damage * emblemLibrary.arcaneMasteryPercent);
        }
        enemyScript.LoseHealth(damage, 0);
        enemyScript.ImpactVFX();
        if (attackProfile.attackType == AttackType.DEFLECT && playerData.equippedEmblems.Contains(emblemLibrary.burning_reflection))
        {
            addedDOT = 10;
        }
        else addedDOT = 0;
        enemyScript.GainDOT(attackProfile.durationDOT + addedDOT);
        RuntimeManager.PlayOneShot(impactSFX, impactSFXvolume, transform.position);
        Destroy(gameObject);
    }

    void HitObject(Collider collision)
    {
        RuntimeManager.PlayOneShot(impactSFX, impactSFXvolume, transform.position);
        Destroy(gameObject);
    }
}
