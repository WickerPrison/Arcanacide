using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireSwordsmanController : EnemyController
{
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] GameObject fireArcPrefab;
    [SerializeField] ChargeIndicator indicator;
    AttackArcGenerator attackArc;
    StepWithAttack stepWithAttack;
    LayerMask layerMask;
    Vector3 chargeDestination;
    float chargeRange = 6;
    float chargeSpeed = 7;
    float fireTrailMaxTime = 0.01f;
    float fireTrailTime;
    float chargeDistance = 1000;
    bool charging = false;
    float previousChargeDistance = 0;
    float chargeIndicatorWidth = 3.5f;
    Vector3 away = new Vector3(100, 100, 100);
    FacePlayer facePlayer;

    public override void Start()
    {
        base.Start();
        indicator.transform.parent = null;
        layerMask = LayerMask.GetMask("Default", "Player", "IFrames");
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        stepWithAttack = GetComponent<StepWithAttack>();
        facePlayer = GetComponent<FacePlayer>();
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

            if (Vector3.Distance(transform.position, playerScript.transform.position) <= attackRange)
            {
                if (attackTime <= 0)
                {
                    Attack();
                }
            }
            else if(Vector3.Distance(transform.position, playerScript.transform.position) <= chargeRange)
            {
                if(attackTime <= 0)
                {
                    attackTime = attackMaxTime;
                    state = EnemyState.SPECIAL;
                    LayChargeIndicator();
                    frontAnimator.Play("ChargeWarmup");
                    backAnimator.Play("ChargeWarmup");
                }
            }
        }
        
        if (state == EnemyState.SPECIAL)
        {
            LayChargeIndicator();
        }
        else if(!charging)
        {
            HideChargeIndicator();
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

    void LayChargeIndicator()
    {
        Vector3 playerDirection = playerScript.transform.position - transform.position;
        RaycastHit hit;
        Physics.Raycast(transform.position, playerDirection, out hit, chargeDistance, layerMask, QueryTriggerInteraction.Ignore);

        indicator.transform.position = Vector3.zero;
        indicator.initialPosition = transform.position;
        indicator.finalPosition = transform.position + playerDirection.normalized * (hit.distance + 2);
        indicator.indicatorWidth = chargeIndicatorWidth;
        indicator.initialNormal = playerDirection;
        indicator.finalNormal = -playerDirection;
        indicator.ReStart();

        facePlayer.ManualFace();
    }

    void HideChargeIndicator()
    {
        indicator.transform.position = away;
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
        HideChargeIndicator();
        charging = false;
    }

    void FireArc()
    {
        GameObject fireArc;
        fireArc = Instantiate(fireArcPrefab);
        fireArc.transform.position = transform.position;
        FireWave fireWaveScript;
        fireWaveScript = fireArc.GetComponent<FireWave>();
        fireWaveScript.target = playerScript.transform.position;
    }

    public override void SpecialAbility()
    {
        navAgent.enabled = true;
        directionLock = true;
        chargeDestination = playerScript.transform.position;
        charging = true;
        state = EnemyState.ATTACKING;
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

    public override void StartDying()
    {
        base.StartDying();
        HideChargeIndicator();
    }
}
