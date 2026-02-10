using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossV4Controller : EnemyController, IEndDialogue
{
    MinibossAbilities abilities;
    [SerializeField] MapData mapData;

    public override void Start()
    {
        base.Start();
        enemyScript.nonStaggerableStates.Add(EnemyState.SPECIAL);
        abilities = GetComponent<MinibossAbilities>();
        if (mapData.miniboss4Killed)
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
                float randFloat = Random.Range(0, 1f);
                if (playerDistance > 4)
                {
                    if (randFloat > 0.8f && playerScript.transform.position.magnitude < 9f)
                    {
                        abilities.Circle(CircleType.SHOOT);
                    }
                    else if (randFloat > 0.6f)
                    {
                        abilities.MissileAttack(MissilePattern.RADIAL);
                    }
                    else if (randFloat > 0.4f)
                    {
                        abilities.DroneCharge();
                    }
                    else if (randFloat > 0.2f)
                    {
                        StartLasers();
                    }
                    else
                    {
                        abilities.StartTeslaHarpoon();
                    }
                }
                else
                {
                    if (randFloat > 0.8f && playerScript.transform.position.magnitude < 9f)
                    {
                        abilities.Circle(CircleType.SHOOT);
                    }
                    else if(randFloat > 0.7)
                    {
                        abilities.DashAway(() => abilities.MissileAttack(MissilePattern.FRONT));
                    }
                    else if (randFloat > 0.5f)
                    {
                        abilities.DashAway(() => abilities.DroneCharge());
                    }
                    else if (randFloat > 0.4f)
                    {
                        abilities.MeleeBlade();
                    }
                    else if (randFloat > 0.2f)
                    {
                        StartLasers();
                    }
                    else
                    {
                        abilities.StartTeslaHarpoon();
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

    public void StartLasers()
    {
        abilities.DashAway(abilities.DroneLasers, abilities.FleePointDestination());
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
            SpriteEffects spriteEffects = GetComponent<SpriteEffects>();
            StartCoroutine(spriteEffects.Dissolve());
            MinibossEvents minibossEvents = GetComponent<MinibossEvents>();
            minibossEvents.Dissolve();
            StartCoroutine(DeathTimer());
        }
        else
        {
            state = EnemyState.IDLE;
            GlobalEvents.instance.MinibossEndDialogue();
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(1);
        enemyScript.Death();
        Death();
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
        mapData.miniboss4Killed = true;
        GlobalEvents.instance.MiniBossKilled();
        GlobalEvents.instance.WhistleblowerKilled();
    }
}
