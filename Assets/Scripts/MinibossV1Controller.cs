using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossV1Controller : EnemyController
{
    MinibossAbilities abilities;

    public override void Start()
    {
        base.Start();
        abilities = GetComponent<MinibossAbilities>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if(state == EnemyState.IDLE)
        {
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerScript.transform.position);
            }
        }

        if(attackTime <= 0)
        {
            attackTime = attackMaxTime;
            abilities.MissileAttack();
        }
        else
        {
            attackTime -= Time.deltaTime;
        }
    }
}
