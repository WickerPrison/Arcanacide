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
            navAgent.SetDestination(abilities.navMeshDestination.position);
        }

        if(state == EnemyState.IDLE)
        {
            if(attackTime <= 0)
            {
                attackTime = attackMaxTime;
                int randInt;
                if(playerDistance > 4)
                {
                    randInt = Random.Range(0, 3);
                    //randInt = 2;
                    switch (randInt)
                    {
                        case 0:
                            abilities.MissileAttack();
                            break;
                        case 1:
                            abilities.ChestLaser(2);
                            break;
                        case 2:
                            abilities.Circle();
                            break;
                    }
                }
                else
                {
                    randInt = Random.Range(0, 4);
                    //randInt = 1;
                    switch (randInt)
                    {
                        case 0:
                            abilities.MeleeBlade();
                            break;
                        case 1:
                            abilities.Circle();
                            break;
                        case 2:
                            abilities.DashAway(abilities.MissileAttack);
                            break;
                        case 3:
                            abilities.DashAway(() => abilities.ChestLaser(2));
                            break;
                    }
                }
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
