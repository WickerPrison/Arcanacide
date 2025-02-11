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

        if (navAgent.enabled == true)
        {
            navAgent.SetDestination(playerScript.transform.position);
        }

        if(state == EnemyState.IDLE)
        {
            if(attackTime <= 0)
            {
                attackTime = attackMaxTime;
                //abilities.MissileAttack();
                //abilities.Dash();
                //abilities.MeleeBlade();
                //abilities.Circle();
                abilities.ChestLaser();
            }
            else
            {
                attackTime -= Time.deltaTime;
            }
        }

        frontAnimator.SetFloat("PlayerDistance", playerDistance);
        backAnimator.SetFloat("PlayerDistance", playerDistance);
    }

    public override void AttackHit(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        base.AttackHit(smearSpeed);
    }
}
