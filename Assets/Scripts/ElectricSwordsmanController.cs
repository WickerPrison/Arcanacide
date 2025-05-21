using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectricSwordsmanController : EnemyController
{
    StepWithAttack stepWithAttack;
    FacePlayer facePlayer;
    public event EventHandler onCloseRing;
    public LightningRings rings;
    AttackArcGenerator attackArc;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
        facePlayer = GetComponent<FacePlayer>();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
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

                    StartRings();
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public void Attack()
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
        Vector3 attackPointDir = Vector3.Normalize(facePlayer.attackPoint.position - transform.position);
        projectile.transform.position = facePlayer.attackPoint.position + Vector3.up * 1.3f + attackPointDir * 1.5f;
        projectile.direction = Vector3.Normalize(facePlayer.attackPoint.position - transform.position);
        projectile.enemyOfOrigin = enemyScript;
    }

    public void StartRings()
    {
        frontAnimator.Play("Rings");
        backAnimator.Play("Rings");
        state = EnemyState.ATTACKING;
    }

    public override void SpecialAbility()
    {
        rings.StartRings();
    }

    public override void SpecialAbilityOff()
    {
        rings.CloseRings();
        attackTime = attackMaxTime;
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
    }
}
