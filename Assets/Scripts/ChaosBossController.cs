using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChaosBossController : EnemyController, IEndDialogue
{
    [System.NonSerialized] public FacePlayer facePlayer;
    FinalBossEvents bossEvents;
    float fleeRadiusMin = 0;
    float fleeRadiusMax = 12;
    Vector3 fleePoint;
    float meleeRange = 3f;
    [System.NonSerialized] public int phase = 1;
    float phaseTriggerPercent = 0.1f;
    float phaseTrigger;
    MusicManager musicManager;
    [SerializeField] AssistantController assistant;

    [SerializeField] FatmanSummon[] fatMenList;
    [System.NonSerialized] public Queue<FatmanSummon> fatMen = new Queue<FatmanSummon>();

    [SerializeField] KnightSummon[] knightSummons;
    [System.NonSerialized] public Queue<KnightSummon> knights = new Queue<KnightSummon>();

    public override void Awake()
    {
        base.Awake();
        bossEvents = GetComponent<FinalBossEvents>();
    }

    public override void Start()
    {
        base.Start();
        musicManager = gm.GetComponent<MusicManager>();
        facePlayer = GetComponent<FacePlayer>();
        facePlayer.SetDestination(new Vector3(7, 0, -9));
        phaseTrigger = enemyScript.maxHealth * phaseTriggerPercent;
        gm.awareEnemies += 1;
        attackTime = attackMaxTime;
        foreach(FatmanSummon fatMan in fatMenList)
        {
            fatMan.enemyScript = enemyScript;
            fatMan.bossController = this;
            fatMen.Enqueue(fatMan);
        }
        foreach(KnightSummon knight in knightSummons)
        {
            knight.bossController = this;
            knight.enemyScript = enemyScript;
            knights.Enqueue(knight);
        }
    }

    public override void EnemyAI()
    {
        if (state == EnemyState.UNAWARE) return;

        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);

        if (phase == 1 && enemyScript.health < phaseTrigger)
        {
            phase = 2;
        }

        if(state == EnemyState.IDLE)
        {
            if(navAgent.enabled == true)
            {
                navAgent.SetDestination(playerScript.transform.position);
            }

            if(attackTime <= 0)
            {
                if(phase == 1)
                {
                    Phase1Attacks();
                }

            }
        }

        if (state == EnemyState.SPECIAL)
        {
            facePlayer.SetDestination(fleePoint);
            float distance = Vector3.Distance(transform.position, fleePoint);
            if (distance <= navAgent.stoppingDistance)
            {
                state = EnemyState.IDLE;
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void Phase1Attacks()
    {
        int randInt;
        if(playerDistance < meleeRange)
        {
            randInt = UnityEngine.Random.Range(1, 5);
        }
        else
        {
            randInt = UnityEngine.Random.Range(0, 4);
        }
        attackTime = 5;
        randInt = 2;
        switch (randInt)
        {
            case 0:
                assistant.CallAnimation("Beams");
                break;
            case 1:
                assistant.CallAnimation("ThrowBombs");
                break;
            case 2:
                assistant.CallAnimation("Bolts");
                break;
            case 3:
                state = EnemyState.ATTACKING;
                frontAnimator.Play("Knights");
                backAnimator.Play("Knights");
                
                break;
            case 4:
                state = EnemyState.ATTACKING;
                frontAnimator.Play("Combo");
                backAnimator.Play("Combo");
                break;
        }
    }

    public void FatManAttack(Vector3 position, Vector3 direction)
    {
        FatmanSummon fatMan = fatMen.Dequeue();
        fatMan.transform.position = position;
        fatMan.SetDirection(direction);
        fatMan.CallAnimation("Attack");
    }

    public void SummonKnight()
    {
        KnightSummon knight = knights.Dequeue();
        knight.GetSummoned();
    }

    public void SetAttackTime(float newTime = -1)
    {
        if (newTime >= 0)
        {
            attackTime = newTime;
        }
        else attackTime = attackMaxTime;
    }

    public void EndDialogue()
    {
        frontAnimator.Play("Idle");
        backAnimator.Play("Idle");
        bossEvents.EndDialogue();
        musicManager.ChangeMusicState(MusicState.BOSSMUSIC);
        state = EnemyState.IDLE;
    }

    public override void StartDying()
    {
        enemyScript.invincible = true;
        enemyScript.health = 1;
        bossEvents.FreezeAssistant();
        GlobalEvents.instance.BossKilled();
        GetComponent<FinalDialogue>().StartConversation();
        frontAnimator.Play("StartDying");
        backAnimator.Play("StartDying");   
    }

    public override void OnEnable()
    {
        base.OnEnable();
        bossEvents.standUp += standUp;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        bossEvents.standUp -= standUp;
    }

    private void standUp(object sender, System.EventArgs e)
    {
        backAnimator.Play("StandUp");
    }
}
