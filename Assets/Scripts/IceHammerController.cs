using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHammerController : EnemyController
{
    AttackArcGenerator attackArc;
    FacePlayer facePlayer;
    [SerializeField] float radius;
    [System.NonSerialized] public Collider enemyCollider;
    [SerializeField] float jumpSpeed;
    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] float jumpHeight;
    [SerializeField] SpriteRenderer jumpIndicator;
    [SerializeField] EventReference smashSound;
    [SerializeField] EventReference iceImpact;
    public int jumps;
    [SerializeField] StalagmiteHolder lineStalagmites;
    [SerializeField] StalagmiteHolder circleStalagmites;
    [SerializeField] Transform[] hammerTargets;
    float longAttackRange = 15f;

    public override void Start()
    {
        base.Start();
        jumpIndicator.transform.SetParent(null);
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        facePlayer = GetComponent<FacePlayer>();
        enemyCollider = GetComponent<Collider>();
        lineStalagmites.gameObject.SetActive(true);
        circleStalagmites.gameObject.SetActive(true);
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerScript.transform.position);
            }


            if(attackTime <= 0)
            {
                attackTime = attackMaxTime;
                float randFloat = Random.Range(0f, 1f);
                if (playerDistance <= attackRange)
                {
                    HammerSmash();
                }
                else
                {
                    if(randFloat > 0.5f || playerDistance > longAttackRange)
                    {
                        JumpSmash();
                    }
                    else
                    {
                        StartStomp();
                    }
                }
            }


            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }
        }
    }

    public void HammerSmash()
    {
        frontAnimator.Play("DoubleAttack");
        backAnimator.Play("DoubleAttack");
        state = EnemyState.ATTACKING;
    }

    public void HammerSmashImpact()
    {
        bool playerInRange = Vector3.Distance(playerScript.transform.position, jumpIndicator.transform.position) <= radius;
        if (playerInRange)
        {
            playerScript.HitPlayer(() =>
            {
                playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
                playerScript.LosePoise(hitPoiseDamage);
                FmodUtils.PlayOneShot(iceImpact, 1f);
            }, () =>
            {
                playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
            });
        }
        GlobalEvents.instance.ScreenShake(0.15f, 0.3f);
        FmodUtils.PlayOneShot(smashSound, 1f);
        enemyEvents.TriggerVfx("HammerSmash");
    }

    public void JumpSmashImpact()
    {
        bool playerInRange = Vector3.Distance(playerScript.transform.position, jumpIndicator.transform.position) <= radius;
        if (playerInRange)
        {
            playerScript.HitPlayer(() =>
            {
                playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
                playerScript.LosePoise(hitPoiseDamage);
                FmodUtils.PlayOneShot(iceImpact, 1f);
            }, () =>
            {
                playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
            });
        }
        GlobalEvents.instance.ScreenShake(0.15f, 0.3f);
        FmodUtils.PlayOneShot(smashSound, 1f);
        enemyEvents.TriggerVfx("JumpSmash");
    }

    public void JumpSmash()
    {
        frontAnimator.Play("JumpSmash");
        backAnimator.Play("JumpSmash");
        state = EnemyState.ATTACKING;
        jumps = 3;
    }

    public void StartJump()
    {
        enemyCollider.enabled = false;
        jumpIndicator.enabled = true;
        jumpIndicator.transform.position = playerScript.transform.position;
        Vector3 offset = Vector3.Normalize(transform.position - jumpIndicator.transform.position) * 1.9f;
        StartCoroutine(Jump(transform.position, playerScript.transform.position + offset)); 
    }

    IEnumerator Jump(Vector3 startPos, Vector3 jumpDestination)
    {
        float jumpTime = Mathf.Max(Vector3.Distance(startPos, jumpDestination) / jumpSpeed, 0.4f);
        float jumpTimer = 0;
        float progress = jumpTimer / jumpTime;
        float landTrigger = 1 - 0.1f / jumpTime;
        landTrigger = Mathf.Max(landTrigger, 0.75f);
        while (progress < landTrigger)
        {
            jumpTimer += Time.deltaTime;
            progress = jumpTimer / jumpTime;
            transform.position = GetJumpPosition(startPos, jumpDestination, progress);
            yield return null;
        }
        frontAnimator.Play("JumpLand");
        backAnimator.Play("JumpLand");
        while(progress < 1)
        {
            jumpTimer += Time.deltaTime;
            progress = jumpTimer / jumpTime;
            transform.position = GetJumpPosition(startPos, jumpDestination, progress);
            yield return null;
        }
    }

    Vector3 GetJumpPosition(Vector3 startPos, Vector3 jumpDestination, float progress)
    {
        return Vector3.Lerp(startPos, jumpDestination, progress) + Vector3.up * heightCurve.Evaluate(progress) * jumpHeight;
    }

    public void StartStomp()
    {
        frontAnimator.Play("Stomp");
        backAnimator.Play("Stomp");
        state = EnemyState.ATTACKING;
    }

    public void Stomp()
    {
        lineStalagmites.TriggerWave();
        GlobalEvents.instance.ScreenShake(0.2f, 0.1f);
        FmodUtils.PlayOneShot(smashSound, 0.5f);
    }

    public void ButtSlam()
    {
        circleStalagmites.transform.localPosition = Vector3.zero;
        circleStalagmites.TriggerWave();
        GlobalEvents.instance.ScreenShake(0.2f, 0.3f);
        FmodUtils.PlayOneShot(smashSound, 1);
    }

    public void HammerIcicles()
    {
        Vector3 targetPos = facingFront ? hammerTargets[0].position : hammerTargets[1].position;
        circleStalagmites.transform.position = new Vector3(targetPos.x, 0, targetPos.z);
        circleStalagmites.TriggerWave();
        GlobalEvents.instance.ScreenShake(0.2f, 0.3f);
        FmodUtils.PlayOneShot(smashSound, 1);
    }

    public override void AttackHit(int smearSpeed)
    {
        enemySound.OtherSounds(0, 2);
        base.AttackHit(smearSpeed);
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
        CancelJump();
    }

    public override void StartDying()
    {
        base.StartDying();
        attackArc.HideAttackArc();
        CancelJump();
    }

    void CancelJump()
    {
        StopAllCoroutines();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z );
        enemyCollider.enabled = true;
    }
}
