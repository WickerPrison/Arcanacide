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
    float unfrozenAttackMaxTime = 1.5f;
    [SerializeField] EventReference draggingEvent;
    [SerializeField] float draggingVolume;
    [SerializeField] StalagmiteHolder stalagmiteHolder;
    [SerializeField] SpriteRenderer jumpIndicator;
    [SerializeField] float jumpSpeed;
    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpRadius;
    [SerializeField] int jumpDamage;
    [SerializeField] float jumpPoiseDamage;
    [SerializeField] GameObject iceRipplePrefab;
    Collider enemyCollider;

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
        stalagmiteHolder.gameObject.SetActive(true);
        enemyCollider = GetComponent<Collider>();
        jumpIndicator.transform.SetParent(null);
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

        if (attackTime <= 0)
        {
            if(Vector3.Distance(playerScript.transform.position, transform.position) < smashRange)
            {
                DoubleAttack();
            }
            else
            {
                JumpAttack();
            }
        }
    }

    public void DoubleAttack()
    {
        attackTime = unfrozenAttackMaxTime;
        state = EnemyState.ATTACKING;
        frontAnimator.Play("DoubleAttack");
        backAnimator.Play("DoubleAttack");
    }

    public override void AttackHit(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        if(remainingIce == 0)
        {
            stepWithAttack.Step(0.15f);
        }
        enemySound.SwordSwoosh();

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

    public void Stomp()
    {
        StartCoroutine(cameraScript.ScreenShake(.2f, .2f));
        enemySound.OtherSounds(1, 1);
        stalagmiteHolder.TriggerWave();
    }

    public void JumpAttack()
    {
        state = EnemyState.ATTACKING;
        frontAnimator.Play("Jump");
        backAnimator.Play("Jump");
        attackTime = attackMaxTime;
    }

    public void StartJump()
    {
        state = EnemyState.ATTACKING;
        enemyCollider.enabled = false;
        jumpIndicator.enabled = true;
        jumpIndicator.transform.position = playerScript.transform.position;
        Vector3 offset = Vector3.Normalize(jumpIndicator.transform.position - transform.position) * 0.2f;
        StartCoroutine(Jump(transform.position, jumpIndicator.transform.position - offset + .083333f * Vector3.up));
    }

    IEnumerator Jump(Vector3 startPos, Vector3 endPos)
    {
        float jumpTime = Mathf.Max(Vector3.Distance(startPos, endPos) / jumpSpeed, 0.4f);
        float jumpTimer = 0;
        float progress = jumpTimer / jumpTime;
        float landTrigger = 1 - 0.1f / jumpTime;
        landTrigger = Mathf.Max(landTrigger, 0.75f);
        while (progress < landTrigger)
        {
            jumpTimer += Time.deltaTime;
            progress = jumpTimer / jumpTime;
            transform.position = GetJumpPosition(startPos, endPos, progress);
            yield return null;
        }
        frontAnimator.Play("Land");
        backAnimator.Play("Land");
        while (progress < 1)
        {
            jumpTimer += Time.deltaTime;
            progress = jumpTimer / jumpTime;
            transform.position = GetJumpPosition(startPos, endPos, progress);
            yield return null;
        }
    }

    Vector3 GetJumpPosition(Vector3 startPos, Vector3 jumpDestination, float progress)
    {
        return Vector3.Lerp(startPos, jumpDestination, progress) + Vector3.up * heightCurve.Evaluate(progress) * jumpHeight;
    }

    public void JumpHit()
    {
        enemyCollider.enabled = true;
        jumpIndicator.enabled = false;
        bool playerInRange = Vector3.Distance(playerScript.transform.position, transform.position) <= jumpRadius;
        if (playerInRange)
        {
            playerScript.HitPlayer(() =>
            {
                playerScript.LoseHealth(jumpDamage, EnemyAttackType.MELEE, enemyScript);
                playerScript.LosePoise(jumpPoiseDamage);
                enemySound.OtherSounds(0, 1f);
            }, () =>
            {
                playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
            });
        }
        GlobalEvents.instance.ScreenShake(0.15f, 0.3f);
        enemySound.OtherSounds(1, 1f);
        enemyEvents.TriggerVfx("JumpSmash");

        GameObject iceRipple = Instantiate(iceRipplePrefab);
        iceRipple.transform.position = transform.position + new Vector3(0, 1, 0);
        IceRipple iceRippleScript = iceRipple.GetComponent<IceRipple>();
        iceRippleScript.enemyOfOrigin = enemyScript;
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
        jumpIndicator.enabled = false;
    }

    public override void EndStagger()
    {
        base.EndStagger();
        if(remainingIce > 0)
        {
            navAgent.speed = 0;
        }
    }

    public override void StartDying()
    {
        base.StartDying();
        attackArc.HideAttackArc();
        jumpIndicator.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, jumpRadius);
    }
}
