using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossV2Controller : EnemyController, IEndDialogue
{
    MinibossAbilities abilities;
    [SerializeField] MapData mapData;

    public override void Start()
    {
        base.Start();
        enemyScript.nonStaggerableStates.Add(EnemyState.SPECIAL);
        abilities = GetComponent<MinibossAbilities>();
        if (mapData.miniboss2Killed)
        {
            enemyEvents.HideBossHealthbar();
            gm.enemies.Remove(enemyScript);
            //Is actually destroyed in MinibossLateScript
        }
        else
        {
            state = EnemyState.DISABLED;
            gm.awareEnemies += 1;
        }
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (navAgent.enabled == true)
        {
            navAgent.SetDestination(abilities.navMeshDestination.position);
        }

        if (state == EnemyState.IDLE)
        {
            if (attackTime <= 0)
            {
                attackTime = attackMaxTime;
                float randFloat;
                if (playerDistance > 4)
                {
                    randFloat = Random.Range(0, 1f);

                    if (randFloat > 0.8f && playerScript.transform.position.magnitude < 9f)
                    {
                        abilities.Circle(CircleType.LASER);
                    }
                    else if (randFloat > 0.5f)
                    {
                        abilities.ChestLaser(2);
                    }
                    else if(randFloat > 0.2f)
                    {
                        abilities.PlasmaShots();
                    }
                    else
                    {
                        abilities.StartTeslaHarpoon();
                    }
                }
                else
                {
                    randFloat = Random.Range(0f, 1f);

                    if (randFloat > 0.8f && playerScript.transform.position.magnitude < 9f)
                    {
                        abilities.Circle(CircleType.LASER);
                    }
                    else if (randFloat > 0.7f)
                    {
                        abilities.MeleeBlade();
                    }
                    else if (randFloat > 0.6f)
                    {
                        abilities.MissileAttack(MissilePattern.RADIAL);
                    }
                    else if(randFloat > 0.5f)
                    {
                        abilities.StartTeslaHarpoon();
                    }
                    else if(randFloat > 0.2f)
                    {
                        abilities.DashAway(() => abilities.PlasmaShots());
                    }
                    else
                    {
                        abilities.DashAway(() => abilities.ChestLaser(2));
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

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        abilities.StartStagger();
    }

    public override void EndStagger()
    {
        base.EndStagger();
    }

    public void EndDialogue()
    {
        if (state == EnemyState.DYING)
        {
            frontAnimator.Play("FlyAway");
            backAnimator.Play("FlyAway");
        }
        else
        {
            state = EnemyState.IDLE;
            GlobalEvents.instance.MinibossEndDialogue();
        }
    }

    public override void StartDying()
    {
        GetComponent<FacePlayer>().ManualFace();
        abilities.StartStagger();
        enemyEvents.StartDying();
        state = EnemyState.DYING;
        enemyScript.invincible = true;
        enemyScript.health = 1;
        frontAnimator.Play("FallToKneel");
        backAnimator.Play("FallToKneel");
    }

    public override void Death()
    {
        base.Death();
        mapData.miniboss2Killed = true;
        GlobalEvents.instance.MiniBossKilled();
    }
}
