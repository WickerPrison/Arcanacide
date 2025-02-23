using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceGolemController : EnemyController
{
    [SerializeField] GameObject iceRipplePrefab;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform trackingPoint;
    AttackArcGenerator attackArc;
    CameraFollow cameraScript;
    FacePlayerSlow facePlayer;
    float minWalkAngle = 70;
    float chargeRange = 7;
    int chargeDamage = 30;
    int chargePoiseDamage = 100;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        facePlayer = GetComponent<FacePlayerSlow>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (navAgent.enabled == true)
        {
            AngleMeasurement();
            navAgent.SetDestination(playerScript.transform.position);
        }

        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk

            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
                return;
            }

            if (Vector3.Distance(transform.position, playerScript.transform.position) <= attackRange)
            {
                int randN = Random.Range(0, 3);
                if (randN == 0)
                {
                    Smash();
                }
                else if (randN == 1)
                {
                    MultiSmash();
                }
                else if (randN == 2)
                {
                    IceRings();
                }
            }
            else if (Vector3.Distance(transform.position, playerScript.transform.position) <= chargeRange)
            {
                int randN = Random.Range(0, 2);
                if (randN == 0)
                {
                    ShoulderCharge();
                }
                else if (randN == 1)
                {
                    IceRings();
                }
            }
        }
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
    }

    void Smash()
    {
        facePlayer.FacePlayerFast();
        directionLock = true;
        frontAnimator.Play("Smash");
        backAnimator.Play("Smash");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
    }

    void MultiSmash()
    {
        facePlayer.FacePlayerFast();
        frontAnimator.Play("MultiSmash");
        backAnimator.Play("MultiSmash");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime + 1;
    }

    void ShoulderCharge()
    {
        facePlayer.FacePlayerFast();
        frontAnimator.Play("ShoulderCharge");
        backAnimator.Play("ShoulderCharge");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
    }

    void IceRings()
    {
        facePlayer.FacePlayerFast();
        frontAnimator.Play("IceRings");
        backAnimator.Play("IceRings");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
    }

    public override void AttackHit(int smearSpeed)
    {
        StartCoroutine(cameraScript.ScreenShake(.1f, .1f));
        enemySound.OtherSounds(1, 1);

        if (!canHitPlayer)
        {
            return;
        }

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

    public override void SpecialAbility()
    {
        enemySound.OtherSounds(0, 1);
        GameObject iceRipple = Instantiate(iceRipplePrefab);
        iceRipple.transform.position = transform.position + new Vector3(0, 1, 0);
        IceRipple iceRippleScript = iceRipple.GetComponent<IceRipple>();
        iceRippleScript.enemyOfOrigin = enemyScript;
    }

    void AngleMeasurement()
    {
        if (state == EnemyState.SPECIAL)
        {
            facePlayer.FacePlayerFast();
            return;
        }

        Vector3 trackingVector = trackingPoint.position - transform.position;
        Vector3 attackVector = attackPoint.position - transform.position;
        float turnAngle = Vector3.Angle(trackingVector, attackVector);
        if(turnAngle <= minWalkAngle)
        {
            navAgent.speed = 5;
        }
        else
        {
            navAgent.speed = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != EnemyState.SPECIAL)
        {
            return;
        }

        if (collision.transform.CompareTag("Player"))
        {
            enemySound.OtherSounds(2, 1);
            frontAnimator.Play("ChargeCollision");
            backAnimator.Play("ChargeCollision");
            if(collision.gameObject.layer == 3)
            {
                playerScript.LoseHealth(chargeDamage, EnemyAttackType.NONPARRIABLE, null);
                playerScript.LosePoise(chargePoiseDamage);
            }
            else if(collision.gameObject.layer == 8)
            {
                playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE);
            }
        }
    }
}
