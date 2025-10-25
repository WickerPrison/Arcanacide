using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class ChaosZombie : EnemyController
{
    [SerializeField] Transform[] handPustule;
    AttackArcGenerator attackArc;
    ChaosSporesScript sporesScript;
    float sporesDuration = 6;
    float meleeRange = 3f;

    public override void Start()
    {
        base.Start();
        sporesScript = playerScript.GetComponentInChildren<ChaosSporesScript>();
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

            if(playerDistance < meleeRange && attackTime < attackMaxTime / 2)
            {
                attackTime = attackMaxTime;
                state = EnemyState.ATTACKING;
                frontAnimator.Play("ClawSwipe");
                backAnimator.Play("ClawSwipe");
            }
            else if(playerDistance < attackRange && attackTime <= 0)
            {
                attackTime = attackMaxTime;
                state = EnemyState.ATTACKING;
                frontAnimator.Play("Attack");
                backAnimator.Play("Attack");
            }

        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public override void SpellAttack()
    {
        PustuleScript pustule = Instantiate(projectilePrefab).GetComponent<PustuleScript>();

        if (facingFront)
            pustule.transform.position = handPustule[0].transform.position;
        else
            pustule.transform.position = handPustule[1].transform.position;


        pustule.endPoint = playerScript.transform.position;
        pustule.enemyScript = enemyScript;
    }

    public override void AdditionalAttackEffects()
    {
        base.AdditionalAttackEffects();
        sporesScript.StartChaosSpores(sporesDuration);
    }

    public override void StartStagger(float staggerDuration)
    {
        attackArc.HideAttackArc();
        base.StartStagger(staggerDuration);
    }
}
