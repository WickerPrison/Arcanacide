using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectricSwordsmanController : EnemyController
{
    StepWithAttack stepWithAttack;
    FacePlayer facePlayer;
    public event EventHandler onCloseRing;
    [SerializeField] GameObject electricRingPrefab;
    LightningRings rings;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
        facePlayer = GetComponent<FacePlayer>();
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

            if (attackTime <= 0)
            {
                float randfloat = UnityEngine.Random.Range(0f, 1f);
                if (randfloat < 0.8f)
                {
                    Attack();
                }
                else
                {
                    frontAnimator.Play("Rings");
                    backAnimator.Play("Rings");
                    state = EnemyState.ATTACKING;
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void Attack()
    {
        int num = UnityEngine.Random.Range(0, 3);
        if (num <= 1)
        {
            frontAnimator.SetBool("TripleAttack", false);
            backAnimator.SetBool("TripleAttack", false);
        }
        else
        {
            frontAnimator.SetBool("TripleAttack", true);
            backAnimator.SetBool("TripleAttack", true);
        }
        frontAnimator.Play("Attack");
        backAnimator.Play("Attack");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
    }

    public override void AttackHit(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        stepWithAttack.Step(0.15f);
        base.AttackHit(smearSpeed);

        EnemySlashProjectile projectile = Instantiate(projectilePrefab).GetComponent<EnemySlashProjectile>();
        projectile.transform.position = facePlayer.attackPoint.position + Vector3.up * 1.3f;
        projectile.direction = Vector3.Normalize(facePlayer.attackPoint.position - transform.position);
        projectile.spellDamage = spellAttackDamage;
        projectile.poiseDamage = spellAttackPoiseDamage;
        projectile.enemyOfOrigin = enemyScript;
    }

    public override void SpecialAbility()
    {
        rings = Instantiate(electricRingPrefab).GetComponent<LightningRings>();
        rings.target = playerScript.transform;
        rings.enemyOfOrigin = enemyScript;
        rings.electricSwordsman = this;
        rings.SetupEvents();
    }

    public override void SpecialAbilityOff()
    {
        onCloseRing?.Invoke(this, EventArgs.Empty);
        attackTime = attackMaxTime;
    }
}
