using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ChaosKnightController : EnemyController
{
    [SerializeField] SpriteRenderer indicatorCircle;
    [SerializeField] ParticleSystem streak;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform jumpPoint;
    WaitForSeconds jumpTime = new WaitForSeconds(4f);
    Vector3 facingDirection;
    Vector3 playerDirection;
    float backAngle;
    float jumpSpeed = 50;
    int jumpDamage = 50;
    float jumpPoiseDamage = 100;
    float indicatorCircleSpeed;
    Vector3 indicatorDirection;
    Vector3 jumpPointDirection;
    StepWithAttack stepWithAttack;
    CameraFollow cameraScript;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
        jumpPoint.parent = null;
        attackTime = attackMaxTime;
        indicatorCircle.enabled = false;
        indicatorCircle.transform.parent = null;
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    private void FixedUpdate()
    {
        if (state != EnemyState.SPECIAL) return;

        indicatorDirection = playerController.transform.position - indicatorCircle.transform.position;

        if(Vector3.Distance(playerController.transform.position, indicatorCircle.transform.position) > indicatorCircleSpeed * Time.deltaTime)
        {
            indicatorCircle.transform.position += indicatorDirection.normalized * indicatorCircleSpeed * Time.fixedDeltaTime;
        }

        jumpPointDirection = jumpPoint.position - transform.position;
        transform.position += jumpPointDirection.normalized * jumpSpeed * Time.fixedDeltaTime;
        if(indicatorCircleSpeed == 0 && Vector3.Distance(transform.position, jumpPoint.position) < jumpSpeed * Time.deltaTime)
        {
            transform.position = jumpPoint.position;
            frontAnimator.Play("Land");
            backAnimator.Play("Land");
        }
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if(attackTime <= 0)
            {
                if(playerDistance < 4)
                {
                    Attack();
                }
                else
                {
                    Jump();
                }
            }
            else
            {
                attackTime -= Time.deltaTime;
            }

            PlayerBehind();
        }
        else
        {
            enemyScript.blockAttack = false;
        }
    }

    void Attack()
    {
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
        frontAnimator.Play("Attack");
        backAnimator.Play("Attack");
    }

    void Jump()
    {
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
        frontAnimator.Play("Jump");
        backAnimator.Play("Jump");
    }

    public override void SpecialAbility()
    {
        state = EnemyState.SPECIAL;
        indicatorCircle.transform.position = transform.position;
        indicatorCircle.enabled = true;
        indicatorCircleSpeed = 7;
        navAgent.enabled = false;
        streak.Play();
        jumpPoint.position = transform.position + Vector3.up * 20;
        StartCoroutine(HangTime());
    }

    public override void SpecialAbilityOff()
    {
        indicatorCircle.enabled = false;
        streak.Stop();
        streak.Clear();
        StartCoroutine(cameraScript.ScreenShake(.1f, .3f));
        enemySound.OtherSounds(0, 1);
        if(Vector3.Distance(jumpPoint.position, playerController.transform.position) <= 3.5f)
        {
            if (playerController.gameObject.layer == 3)
            {
                enemySound.SwordImpact();
                playerScript.LoseHealth(jumpDamage, EnemyAttackType.MELEE, enemyScript);
                playerScript.LosePoise(jumpPoiseDamage);
            }
            else if (playerController.gameObject.layer == 8)
            {
                playerController.PerfectDodge();
            }
        }
    }

    IEnumerator HangTime()
    {
        yield return jumpTime;
        indicatorCircleSpeed = 0;
        transform.position = indicatorCircle.transform.position + Vector3.up * 15;
        jumpPoint.position = indicatorCircle.transform.position;
    }

    public override void AttackHit(int smearSpeed)
    {
        //smearScript.particleSmear(smearSpeed);
        stepWithAttack.Step(0.15f);
        base.AttackHit(smearSpeed);
    }

    void PlayerBehind()
    {
        playerDirection = transform.position - playerController.transform.position;
        facingDirection = attackPoint.position - transform.position;
        float angle = Vector3.Angle(facingDirection, playerDirection);
        if(angle < backAngle)
        {
            enemyScript.blockAttack = false;
        }
        else
        {
            enemyScript.blockAttack = true;
        }
    }
}