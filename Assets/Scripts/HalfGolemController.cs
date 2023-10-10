using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HalfGolemController : EnemyController
{
    CameraFollow cameraScript;
    FacePlayer facePlayer;
    float smashRange = 3;
    [System.NonSerialized] public int remainingIce = 3;
    public event System.EventHandler onIceBreak;
    [SerializeField] ParticleSystem poof;
    Renderer poofRenderer;
    AttackArcGenerator attackArc;
    StepWithAttack stepWithAttack;
    float unfrozenAttackMaxTime = 3;
    [SerializeField] EventReference draggingEvent;
    [SerializeField] float draggingVolume;

    public override void Start()
    {
        base.Start();
        //navAgent.enabled = false;
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        facePlayer = GetComponent<FacePlayer>();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        stepWithAttack = GetComponent<StepWithAttack>();
        poofRenderer = poof.GetComponent<Renderer>();
        enemySound.Play(draggingEvent, draggingVolume);
        enemySound.SetPaused(true);
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (state == EnemyState.IDLE)
        {
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }

            if (remainingIce > 0)
            {
                FrozenAI();
            }
            else
            {
                UnfrozenAI();
            }
        }
    }

    void FrozenAI()
    {
        //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
        if (navAgent.enabled == true)
        {
            navAgent.SetDestination(playerScript.transform.position);
        }

        if (Vector3.Distance(playerScript.transform.position, transform.position) < smashRange)
        {
            if (attackTime <= 0)
            {
                attackTime = attackMaxTime;
                state = EnemyState.ATTACKING;
                navAgent.speed = 0;
                frontAnimator.Play("Attack" + remainingIce.ToString());
                backAnimator.Play("Attack" + remainingIce.ToString());
            }
        }
        else
        {
            frontAnimator.Play("Walk");
            backAnimator.Play("Walk");
        }
    }

    void UnfrozenAI()
    {
        if (navAgent.enabled)
        {
            navAgent.SetDestination(playerScript.transform.position);
        }

        if (attackTime <= 0 && Vector3.Distance(playerScript.transform.position, transform.position) < smashRange)
        {
            attackTime = unfrozenAttackMaxTime;
            state = EnemyState.ATTACKING;
            frontAnimator.Play("DoubleAttack");
            backAnimator.Play("DoubleAttack");
        }
    }

    public override void AttackHit(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        if(remainingIce == 0)
        {
            stepWithAttack.Step(0.15f);
        }
        enemySound.SwordSwoosh();
        parryWindow = false;

        if (!canHitPlayer)
        {
            return;
        }

        if (playerScript.gameObject.layer == 3)
        {
            enemySound.SwordImpact();
            playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
        }
        else if (playerScript.gameObject.layer == 8)
        {
            playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
        }
    }

    public override void SpellAttack()
    {
        enemySound.OtherSounds(0, 1);
        if(facePlayer.faceDirection.z < 0 && facePlayer.faceDirection.x < 0)
        {
            poofRenderer.sortingOrder = 10;
        }
        else
        {
            poofRenderer.sortingOrder = 0;
        }
        poof.Play();

        if (!attackArc.CanHitPlayer()) 
        {
            return;
        }


        if (playerScript.gameObject.layer == 3)
        {
            playerScript.LoseStamina(60);
        }
        else if (playerScript.gameObject.layer == 8)
        {
            playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE);
        }
    }

    public override void SpecialAbility()
    {
        StartCoroutine(cameraScript.ScreenShake(.1f, .1f));
        enemySound.OtherSounds(1, 1);
        parryWindow = false;

        if (!attackArc.CanHitPlayer()) return;


        if (playerScript.gameObject.layer == 3)
        {
           playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
           playerScript.LosePoise(hitPoiseDamage);
        }
        else if (playerScript.gameObject.layer == 8)
        {
            playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
        }
    }
    public override void OnTakeDamage(object sender, EventArgs e)
    {
        base.OnTakeDamage(sender, e);
        if (remainingIce > 0)
        {
            enemySound.OtherSounds(2, 1);
            onIceBreak?.Invoke(this, EventArgs.Empty);
            remainingIce--;
            if (remainingIce == 0)
            {
                frontAnimator.SetBool("IsFrozen", false);
                backAnimator.SetBool("IsFrozen", false);
                navAgent.enabled = true;
                navAgent.speed = 5;
                attackTime = unfrozenAttackMaxTime;
            }
        }
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
    }

    public override void EndStagger()
    {
        base.EndStagger();
        if(remainingIce > 0)
        {
            navAgent.speed = 0;
        }
    }
}
