using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceMageController : EnemyController
{
    [SerializeField] FireRing iceRing;
    PlayerMovement playerMovement;
    float tooClose = 3.5f;
    int fireBallDamage = 15;
    int fireBallPoiseDamage = 15;
    int fireRingDamage = 30;
    float fireRingPoiseDamage = 100;

    public override void Start()
    {
        base.Start();
        playerMovement = playerScript.GetComponent<PlayerMovement>();
        spellAttackDamage = fireBallDamage;
        spellAttackPoiseDamage = fireBallPoiseDamage;
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

            if (Vector3.Distance(transform.position, playerScript.transform.position) < tooClose)
            {
                if (attackTime <= 0)
                {
                    frontAnimator.Play("Blast");
                    backAnimator.Play("Blast");
                    attackTime = attackMaxTime;
                }
            }
            else if (Vector3.Distance(transform.position, playerScript.transform.position) < attackRange)
            {
                if (attackTime <= 0)
                {
                    frontAnimator.Play("CastSpell");
                    backAnimator.Play("CastSpell");
                    attackTime = attackMaxTime;
                }
            }

        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void BackUp()
    {
        Vector3 awayDirection = transform.position - playerScript.transform.position;
        if (navAgent.enabled)
        {
            navAgent.Move(awayDirection.normalized * Time.deltaTime * navAgent.speed / 2);
        }
    }

    public override void SpellAttack()
    {
        GameObject icicle;
        icicle = Instantiate(projectilePrefab);
        icicle.transform.position = playerScript.transform.position;
        Projectile projectile = icicle.GetComponentInChildren<Projectile>();
        projectile.enemyOfOrigin = enemyScript;
    }

    public override void SpecialAbility()
    {
        iceRing.Explode();

        if (Vector3.Distance(transform.position, playerScript.transform.position) < tooClose && playerScript.gameObject.layer == 3)
        {
            playerScript.LoseHealth(fireRingDamage,EnemyAttackType.MELEE, enemyScript);

            if (playerAbilities.shield) return;

            playerScript.LosePoise(fireRingPoiseDamage);
            Rigidbody playerRB = playerScript.gameObject.GetComponent<Rigidbody>();
            Vector3 awayVector = playerScript.transform.position - transform.position;
            PlayerAnimation playerAnimation = playerScript.gameObject.GetComponent<PlayerAnimation>();
            playerAnimation.attacking = false;
            playerRB.velocity = Vector3.zero;
            StartCoroutine(playerMovement.KnockBack(0.4f));
            playerRB.AddForce(awayVector.normalized * 7, ForceMode.VelocityChange);
        }
    }
}
