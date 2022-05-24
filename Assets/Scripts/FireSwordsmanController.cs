using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireSwordsmanController : EnemyController
{
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] GameObject fireArcPrefab;
    Vector3 chargeDestination;
    float chargeRange = 6;
    float chargeSpeed = 7;
    float fireTrailMaxTime = 0.01f;
    float fireTrailTime;
    bool fireArc = false;

    public override void Start()
    {
        base.Start();
        hitDamage = 30;
        hitPoiseDamage = 15;
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (hasSeenPlayer)
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
                    fireArc = false;
                    Attack();
                }
            }
            else if(Vector3.Distance(transform.position, playerController.transform.position) <= chargeRange)
            {
                if(attackTime <= 0)
                {
                    attackTime = attackMaxTime;
                    //int num = Random.Range(0, 3);
                    int num = 2;
                    if(num < 2)
                    {
                        fireArc = true;
                        Attack();
                    }
                    else
                    {
                        directionLock = true;
                        frontAnimator.Play("ChargeWarmup");
                        backAnimator.Play("ChargeWarmup");
                    }
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
        attacking = true;
        attackTime = attackMaxTime;
    }

    public override void AttackHit(int smearSpeed)
    {
        base.AttackHit(smearSpeed);
        if (fireArc)
        {
            FireArc();
        }
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
}
