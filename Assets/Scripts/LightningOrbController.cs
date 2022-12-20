using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightningOrbController : EnemyController
{
    [SerializeField] GameObject lightningExplosion;
    [SerializeField] ParticleSystem lightning;
    [SerializeField] ParticleSystem attack;
    ElectricAlly allyScript;
    Vector3 playerDirection;
    float playerDistance;

    public override void Update()
    {
        if (enemyScript.isDying)
        {
            return;
        }

        EnemyAI();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (hasSeenPlayer)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            playerDistance = Vector3.Distance(transform.position, playerController.transform.position);

            if(attackTime <= 0 && playerDistance <= attackRange)
            {
                attackTime = attackMaxTime;
                StartCoroutine(AttackCouroutine());
            }

            TowardsPlayer();
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    IEnumerator AttackCouroutine()
    {
        lightning.Stop();
        yield return new WaitForSeconds(1);
        Attack();
        attack.Play();
        yield return new WaitForSeconds(.5f);
        lightning.Play();
    }

    void Attack()
    {
        if(playerDistance <= attackRange)
        {
            playerScript.LoseHealth(spellAttackDamage);
            playerScript.LosePoise(spellAttackPoiseDamage);
        }
    }

    void TowardsPlayer()
    {
        playerDirection = playerController.transform.position - transform.position;
        playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z);
        attack.transform.rotation = Quaternion.LookRotation(playerDirection.normalized);
    }

    public override void StartDying()
    {
        enemyScript.Death();
        GameObject explosion = Instantiate(lightningExplosion);
        explosion.transform.position = transform.position + Vector3.up * 1.5f;
    }

    public override void StartStagger(float staggerDuration)
    {
        
    }
}
