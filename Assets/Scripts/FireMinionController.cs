using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireMinionController : EnemyController
{
    AttackArcGenerator attackArc;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        hitDamage = 30;
        hitPoiseDamage = 15;
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

            if (Vector3.Distance(transform.position, playerController.transform.position) <= attackRange)
            {
                if (attackTime <= 0)
                {
                    Attack();
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public override void AttackHit(int smearSpeed)
    {
        enemySound.OtherSounds(0, 2);
        base.AttackHit(smearSpeed);
    }

    void Attack()
    {
        int num = Random.Range(0, 3);
        if(num <= 1)
        {
            frontAnimator.Play("Attack");
            backAnimator.Play("Attack");
        }
        else
        {
            frontAnimator.Play("DoubleAttack");
            backAnimator.Play("DoubleAttack");
        }
        attacking = true;
        attackTime = attackMaxTime;
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
    }
}
