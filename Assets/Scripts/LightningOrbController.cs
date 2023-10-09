using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightningOrbController : EnemyController
{
    [SerializeField] GameObject lightningExplosion;
    [SerializeField] ParticleSystem lightning;
    [SerializeField] ParticleSystem attack;
    [SerializeField] float selfDestructTime;
    [SerializeField] EventReference impactSFX;
    ElectricAlly allyScript;
    Vector3 playerDirection;

    public override void Update()
    {
        if (state == EnemyState.DYING)
        {
            return;
        }

        EnemyAI();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerScript.transform.position);
            }

            if(attackTime <= 0 && playerDistance <= attackRange)
            {
                attackTime = attackMaxTime;
                StartCoroutine(SelfDestructCoroutine());
                //StartCoroutine(AttackCoroutine());
            }

            TowardsPlayer();
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    IEnumerator SelfDestructCoroutine()
    {
        lightning.Stop();
        yield return new WaitForSeconds(selfDestructTime);
        SelfDestruct();
    }

    void Attack()
    {
        if(playerDistance <= attackRange)
        {
            playerScript.LoseHealth(spellAttackDamage, EnemyAttackType.NONPARRIABLE, null);
            playerScript.LosePoise(spellAttackPoiseDamage);
        }
    }

    void TowardsPlayer()
    {
        playerDirection = playerScript.transform.position - transform.position;
        playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z);
        attack.transform.rotation = Quaternion.LookRotation(playerDirection.normalized);
    }

    public override void StartDying()
    {
        SelfDestruct();

        /*
        enemyScript.Death();
        GameObject explosion = Instantiate(lightningExplosion);
        explosion.transform.position = transform.position + Vector3.up * 1.5f;
        */
    }

    void SelfDestruct()
    {
        GameObject explosion = Instantiate(lightningExplosion);
        explosion.transform.position = transform.position + Vector3.up * 1.5f;
        if (playerDistance <= attackRange)
        {
            if(playerScript.gameObject.layer == 3)
            {
                RuntimeManager.PlayOneShot(impactSFX, 2);
                playerScript.StartStagger(0.1f);
                playerScript.LoseHealth(spellAttackDamage, EnemyAttackType.NONPARRIABLE, null);
                playerScript.LosePoise(spellAttackPoiseDamage);
            }
            else if(playerScript.gameObject.layer == 8)
            {
                playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE);
            }

        }
        enemyScript.Death();
    }

    public override void StartStagger(float staggerDuration)
    {
        
    }
}
