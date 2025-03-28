using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossV1Controller : EnemyController, IEndDialogue
{
    MinibossAbilities abilities;
    [SerializeField] MapData mapData;
    

    public override void Start()
    {
        base.Start();
        enemyScript.nonStaggerableStates.Add(EnemyState.SPECIAL);
        abilities = GetComponent<MinibossAbilities>();
        if (mapData.miniboss1Killed)
        {
            enemyEvents.HideBossHealthbar();
            //musicManager.ChangeMusicState(MusicState.MAINLOOP);
            gm.enemies.Remove(enemyScript);
            Destroy(gameObject);
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

        if(state == EnemyState.IDLE)
        {
            if(attackTime <= 0)
            {
                attackTime = attackMaxTime;
                int randInt;
                if(playerDistance > 4)
                {
                    randInt = Random.Range(0, 3);
                    randInt = 2;
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
                    randInt = 1;
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
        if(state == EnemyState.DYING)
        {
            frontAnimator.Play("FlyAway");
            backAnimator.Play("FlyAway");
        }
        else
        {
            state = EnemyState.IDLE;
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
        mapData.miniboss1Killed = true;
        GlobalEvents.instance.MiniBossKilled();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GlobalEvents.instance.onTestButton += Instance_onTestButton;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GlobalEvents.instance.onTestButton -= Instance_onTestButton;
    }

    private void Instance_onTestButton(object sender, System.EventArgs e)
    {
        enemyScript.LosePoise(10000);
    }
}
