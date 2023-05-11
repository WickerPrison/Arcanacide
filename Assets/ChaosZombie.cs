using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class ChaosZombie : EnemyController
{
    [SerializeField] Transform handPustule;

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

            if(attackTime <= 0)
            {
                attackTime = attackMaxTime;
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
        pustule.transform.position = handPustule.transform.position;
        pustule.endPoint = playerController.transform.position;
    }
}
