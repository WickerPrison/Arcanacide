using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireSwordsmanController : EnemyController
{
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] GameObject fireArcPrefab;
    AttackArcGenerator attackArc;
    StepWithAttack stepWithAttack;
    Vector3 chargeDestination;
    float chargeRange = 6;
    float chargeSpeed = 7;
    float fireTrailMaxTime = 0.01f;
    float fireTrailTime;
    bool charging = false;
    float previousChargeDistance = 0;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        stepWithAttack = GetComponent<StepWithAttack>();
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

            if (Vector3.Distance(transform.position, playerController.transform.position) <= attackRange)
            {
                if (attackTime <= 0)
                {
                    Attack();
                }
            }
            else if(Vector3.Distance(transform.position, playerController.transform.position) <= chargeRange)
            {
                if(attackTime <= 0)
                {
                    attackTime = attackMaxTime;
                    directionLock = true;
                    frontAnimator.Play("ChargeWarmup");
                    backAnimator.Play("ChargeWarmup");
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public override void Update()
    {
        base.Update();

        if (charging && navAgent.enabled)
        {
            FireTrail();
            Vector3 chargeDirection = chargeDestination - transform.position;
            navAgent.Move(chargeDirection * Time.deltaTime * chargeSpeed);
            float chargeDistance = Vector3.Distance(chargeDestination, transform.position);
            frontAnimator.SetFloat("ChargeDistance", chargeDistance);
            backAnimator.SetFloat("ChargeDistance", chargeDistance);
            float amountMoved = Mathf.Abs(chargeDistance - previousChargeDistance);
            previousChargeDistance = chargeDistance;
            if(amountMoved <= 0.1f)
            {
                frontAnimator.Play("Attack4");
                backAnimator.Play("Attack4");
                chargeDistance = 0;
            }

            if (chargeDistance < 1)
            {
                navAgent.enabled = false;
                charging = false;
            }
        }
    }

    void Attack()
    {
        int num = Random.Range(0, 3);
        if(num <= 1)
        {
            frontAnimator.SetBool("TripleAttack", false);
            backAnimator.SetBool("TripleAttack", false);
        }
        else
        {
            frontAnimator.SetBool("TripleAttack", true);
            backAnimator.SetBool("TripleAttack", true);
        }
        frontAnimator.Play("Attack");
        backAnimator.Play("Attack");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
        charging = false;
    }

    void FireArc()
    {
        GameObject fireArc;
        fireArc = Instantiate(fireArcPrefab);
        fireArc.transform.position = transform.position;
        FireWave fireWaveScript;
        fireWaveScript = fireArc.GetComponent<FireWave>();
        fireWaveScript.target = playerController.transform.position;
    }

    public override void SpecialAbility()
    {
        navAgent.enabled = true;
        chargeDestination = playerController.transform.position;
        charging = true;
    }

    public void FireTrail()
    {
        if (fireTrailTime < 0)
        {
            GameObject fireTrail;
            fireTrail = Instantiate(fireTrailPrefab);
            fireTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);            
            fireTrailTime = fireTrailMaxTime;
        }
        else
        {
            fireTrailTime -= Time.deltaTime;
        }
    }

    public override void AttackHit(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        stepWithAttack.Step(0.15f);
        base.AttackHit(smearSpeed);
    }
}
