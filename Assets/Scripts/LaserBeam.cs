using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] int beamDamage;
    [SerializeField] float beamPoiseDamage;
    [SerializeField] EnemyScript enemyOfOrigin;
    [SerializeField] EventReference playerImpactSFX;
    [SerializeField] float impactSFXvolume;
    [SerializeField] EventReference beamSFX;
    [SerializeField] EnemySound enemySound;
    float maxDamageDelay = 0.5f;
    float damageDelay;

    private void Update()
    {
        if (damageDelay > 0) damageDelay -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageDelay > 0) return;
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.layer == 3)
            {
                HitPlayer(other);
            }
            else if(other.gameObject.layer == 8)
            {
                PerfectDodge(other);
            }
        }
    }

    void HitPlayer(Collider other)
    {
        PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
        playerScript.LoseHealth(beamDamage, EnemyAttackType.PROJECTILE, enemyOfOrigin);
        playerScript.LosePoise(beamPoiseDamage);
        RuntimeManager.PlayOneShot(playerImpactSFX, impactSFXvolume, transform.position);
        damageDelay = maxDamageDelay;
    }

    void PerfectDodge(Collider other)
    {
        PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
        playerScript.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin);
    }

    private void OnEnable()
    {
        enemySound.Play(beamSFX, 0.5f);
    }

    private void OnDisable()
    {
        enemySound.Stop();
    }
}
