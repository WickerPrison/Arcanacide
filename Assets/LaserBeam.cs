using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] int beamDamage;
    [SerializeField] float beamPoiseDamage;
    [SerializeField] EnemyScript enemyOfOrigin;
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
        // add sfx
        damageDelay = maxDamageDelay;
    }

    void PerfectDodge(Collider other)
    {
        PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
        playerScript.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin);
    }
}
