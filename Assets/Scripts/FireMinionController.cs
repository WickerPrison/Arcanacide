using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireMinionController : EnemyController
{
    public override void Start()
    {
        base.Start();
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

    void Attack()
    {
        int num = Random.Range(0, 3);
        if(num <= 1)
        {
            frontAnimator.Play("Attack1");
            backAnimator.Play("Attack1");
        }
        else
        {
            frontAnimator.Play("DoubleAttack1");
            backAnimator.Play("DoubleAttack1");
        }
        attacking = true;
        attackTime = attackMaxTime;
    }
}
