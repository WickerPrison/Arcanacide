using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[System.Serializable]
public class ChaosBossController : EnemyController, IEndDialogue
{
    [System.NonSerialized] public FacePlayer facePlayer;
    FinalBossEvents bossEvents;
    float fleeRadiusMin = 0;
    float fleeRadiusMax = 12;
    Vector3 fleePoint;
    [System.NonSerialized] public int phase = 1;
    float phaseTriggerPercent = 1.1f;
    float phaseTrigger;
    MusicManager musicManager;
    [SerializeField] AssistantController assistant;

    [SerializeField] FatmanSummon[] fatMenList;
    [System.NonSerialized] public Queue<FatmanSummon> fatMen = new Queue<FatmanSummon>();

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
        foreach(FatmanSummon fatMan in fatMenList)
        {
            fatMan.enemyScript = enemyScript;
            fatMan.bossController = this;
            fatMen.Enqueue(fatMan);
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
                attackTime = attackMaxTime;
                frontAnimator.Play("Combo");
                backAnimator.Play("Combo");
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

    public void FatManAttack(Vector3 position, Vector3 direction)
    {
        FatmanSummon fatMan = fatMen.Dequeue();
        fatMan.transform.position = position;
        fatMan.SetDirection(direction);
        fatMan.CallAnimation("Attack");
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
