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
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    float meleeRange = 3f;
    [System.NonSerialized] public int phase = 1;
    float phaseTriggerPercent = 0.6f;
    float phaseTrigger;
    MusicManager musicManager;
    [SerializeField] AssistantController assistant;

    [SerializeField] FatmanSummon[] fatMenList;
    [System.NonSerialized] public Queue<FatmanSummon> fatMen = new Queue<FatmanSummon>();

    [SerializeField] KnightSummon[] knightSummons;
    [System.NonSerialized] public Queue<KnightSummon> knights = new Queue<KnightSummon>();

    [SerializeField] IceSniperSummon[] iceSniperSummons;
    [System.NonSerialized] public Queue<IceSniperSummon> snipers = new Queue<IceSniperSummon>();

    public event EventHandler<int> onFireWaves;
    WaitForSeconds waveDelay = new WaitForSeconds(1f);
    WaitForSeconds finalWaveDelay = new WaitForSeconds(3f);

    public int orbDamage;

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
        //facePlayer.SetDestination(new Vector3(3, 0, -2));
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
        foreach(IceSniperSummon sniper in iceSniperSummons)
        {
            sniper.enemyScript = enemyScript;
            sniper.bossController = this;
            snipers.Enqueue(sniper);
        }
        enemyScript.nonStaggerableStates.Add(EnemyState.SPECIAL);
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
                if (phase == 1)
                {
                    Phase1Attacks();
                }
                else Phase2Attacks();
            }
        }

        if (state == EnemyState.SPECIAL)
        {
            facePlayer.SetDestination(fleePoint);
            navAgent.SetDestination(Vector3.zero);
            float distance = Vector3.Distance(transform.position, fleePoint);
            if (distance <= navAgent.stoppingDistance)
            {
                FireWaves();
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void Phase1Attacks()
    {
        attackTime = 100;
        float randFloat;
        if(playerDistance < meleeRange)
        {
            randFloat = UnityEngine.Random.Range(0.4f, 1.4f);
        }
        else
        {
            randFloat = UnityEngine.Random.Range(0f, 1.2f);
        }
        switch (randFloat)
        {
            case <= 0.2f: assistant.CallAnimation("Beams"); break;
            case <= 0.4f: StartSummonSnipers(); break;
            case <= 0.6f: IceRings(); break;
            case <= 0.8f: ThrowBombs(); break;
            case <= 1.0f: StartKnightsAttack(); break;
            case <= 1.2f: StartFireWaves(); break;
            case <= 1.4f: Combo(); break;
            //default: Bolts(); break;
        }
    }

    void Phase2Attacks()
    {
        int randInt;
        if (playerDistance < meleeRange)
        {
            randInt = UnityEngine.Random.Range(1, 5);
        }
        else
        {
            randInt = UnityEngine.Random.Range(0, 4);
        }
        attackTime = 3;
        switch (randInt)
        {
            case 0:
                assistant.CallAnimation("Beams");
                StartCoroutine(DelayAttack(0.4f, () =>
                {
                    state = EnemyState.ATTACKING;
                    frontAnimator.Play("Knights");
                    backAnimator.Play("Knights");
                }));
                break;
            case 1:
                assistant.CallAnimation("ThrowBombs");
                break;
            case 2:
                assistant.CallAnimation("Bolts");
                attackTime = 5;
                break;
            case 3:
                StartKnightsAttack();
                break;
            case 4:
                state = EnemyState.ATTACKING;
                StartCoroutine(DelayAttack(0.4f, () =>
                {
                    frontAnimator.Play("Combo");
                    backAnimator.Play("Combo");
                    assistant.CallAnimation("ThrowBombs");
                }));
                break;
        }
    }

    public void ThrowBombs()
    {
        assistant.CallAnimation("ThrowBombs");
    }

    public void Combo()
    {
        state = EnemyState.ATTACKING;
        frontAnimator.Play("Combo");
        backAnimator.Play("Combo");
    }

    public void IceRings()
    {
        assistant.CallAnimation("IceRings");
    }

    public void Bolts()
    {
        assistant.CallAnimation("Bolts");
    }

    public void StartFireWaves()
    {
        if(transform.position.magnitude <= 0.5f)
        {
            FireWaves();
            return;
        }
        fleePoint = Vector3.zero;
        state = EnemyState.SPECIAL;
        navAgent.speed = runSpeed;
        navAgent.stoppingDistance = 0.5f;
        frontAnimator.Play("Run");
        backAnimator.Play("Run");
    }

    public void FireWaves()
    {
        navAgent.stoppingDistance = 4;
        navAgent.speed = walkSpeed;
        state = EnemyState.IDLE;
        attackTime = 10;
        frontAnimator.Play("FireWaves");
        backAnimator.Play("FireWaves");
    }

    public IEnumerator WavePattern()
    {
        onFireWaves?.Invoke(this, 1);
        yield return waveDelay;
        onFireWaves?.Invoke(this, 2);
        yield return waveDelay;
        onFireWaves?.Invoke(this, 3);
        yield return waveDelay;
        onFireWaves?.Invoke(this, 4);
        if(phase == 2)
        {
            yield return waveDelay;
            onFireWaves?.Invoke(this, 0);
        }
        yield return finalWaveDelay;
        attackTime = attackMaxTime;
    }

    public void StartKnightsAttack()
    {
        state = EnemyState.ATTACKING;
        frontAnimator.Play("Knights");
        backAnimator.Play("Knights");
    }

    IEnumerator DelayAttack(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
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

    public void StartSummonSnipers()
    {
        state = EnemyState.ATTACKING;
        frontAnimator.Play("SummonSnipers");
        backAnimator.Play("SummonSnipers");
    }

    public void SummonSniper(int whichSide)
    {
        int selectSide = facePlayer.faceDirectionID switch
        {
            0 or 2 => whichSide * -1,
            1 or 3 => whichSide,
            _ => 0,
        };
        IceSniperSummon sniper = snipers.Dequeue();
        Vector3 direction = facePlayer.attackPoint.position - transform.position;
        sniper.GetSummoned(selectSide, direction.normalized);
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
        facePlayer.ResetDestination();
        bossEvents.EndDialogue();
        musicManager.ChangeMusicState(MusicState.BOSSMUSIC);
        state = EnemyState.IDLE;
    }

    public override void StartDying()
    {
        state = EnemyState.DYING;
        navAgent.enabled = false;
        enemyScript.invincible = true;
        enemyScript.health = 1;
        bossEvents.FreezeAssistant();
        GlobalEvents.instance.BossKilled();
        SteamAchievements.UnlockAchievement(Achievement.KILL_CEO);
        GetComponent<FinalDialogue>().StartConversation();
        frontAnimator.Play("StartDying");
        backAnimator.Play("StartDying");   
    }

    public override void EndStagger()
    {
        base.EndStagger();
        attackTime = attackMaxTime;
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
