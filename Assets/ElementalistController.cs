using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementalistController : EnemyController
{
    [SerializeField] Transform frontAttackPoint;
    [SerializeField] Transform backAttackPoint;
    StepWithAttack stepWithAttack;
    float meleeRange = 4;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if (playerDistance < meleeRange && attackTime <= 0)
            {
                frontAnimator.Play("SwordAttack");
                backAnimator.Play("SwordAttack");
                attackTime = attackMaxTime;
            }
            else if (playerDistance < attackRange && attackTime <= 0)
            {
                frontAnimator.Play("CastSpell");
                backAnimator.Play("CastSpell");
                attackTime = attackMaxTime;
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public override void SpellAttack()
    {
        GameObject projectile;
        HomingProjectile projectileScript;
        projectile = Instantiate(projectilePrefab);
        projectileScript = projectile.GetComponent<HomingProjectile>();
        if (facingFront)
        {
            projectile.transform.position = frontAttackPoint.position;
        }
        else
        {
            projectile.transform.position = backAttackPoint.position;
        }
        projectile.transform.LookAt(playerController.transform.position);
        projectileScript.target = playerController.transform;
        projectileScript.poiseDamage = spellAttackPoiseDamage;
        projectileScript.spellDamage = spellAttackDamage;
        projectileScript.enemyOfOrigin = enemyScript;
        enemySound.OtherSounds(0, 1);
    }

    public void SwingSword(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        stepWithAttack.Step(0.15f);
        enemySound.SwordSwoosh();
    }

    public void SwooshShock()
    {
        if (!canHitPlayer)
        {
            enemySound.OtherSounds(2, 1);
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            enemySound.OtherSounds(1, 1);
            playerScript.LoseHealth(hitDamage, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerController.gameObject.layer == 8)
        {
            enemySound.OtherSounds(2, 1);
            playerController.PerfectDodge();
        }
    }
}
