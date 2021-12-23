using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireSwordsmanController : EnemyController
{
    [SerializeField] ParticleSystem frontSmear;
    Vector3 frontSmearScale;
    Vector3 frontSmearRotation;
    Vector3 frontSmearPosition;
    Vector3 chargeDestination;
    int hitDamage = 30;
    float hitPoiseDamage = 15;
    float chargeRange = 6;
    float chargeSpeed = 10;

    public override void Start()
    {
        base.Start();
        frontSmearScale = frontSmear.transform.localScale;
        frontSmearRotation = new Vector3(90, -20, 0);
        frontSmearPosition = new Vector3(-0.17f, -0.3f, 0.17f);
    }

    public override void EnemyAI()
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) <= detectRange)
        {
            hasSeenPlayer = true;
        }

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
                    frontAnimator.Play("Attack");
                    attacking = true;
                    attackTime = attackMaxTime;
                }
            }
            else if(Vector3.Distance(transform.position, playerController.transform.position) <= chargeRange)
            {
                if(attackTime <= 0)
                {
                    attackTime = attackMaxTime;
                    frontAnimator.Play("ChargeWarmup");
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }

        if (navAgent.enabled)
        {
            AttackPoint();
        }
    }

    public override void Update()
    {
        base.Update();

        if (navAgent.enabled)
        {
            SmearDirection();
        }

        if (charging)
        {
            Vector3 chargeDirection = chargeDestination - transform.position;
            navAgent.Move(chargeDirection * Time.deltaTime * chargeSpeed);
            float chargeDistance = Vector3.Distance(chargeDestination, transform.position);
            frontAnimator.SetFloat("ChargeDistance", chargeDistance);
            if (chargeDistance < 1)
            {
                charging = false;
            }
        }
    }

    void AttackPoint()
    {
        Vector3 direction = playerController.transform.position - transform.position; 
        attackPoint.position = transform.position + direction.normalized;
    }

    public override void AttackHit(int smearSpeed)
    {
        ParticleSystem.ShapeModule frontSmearShape = frontSmear.shape;
        frontSmearShape.arcSpeed = smearSpeed;
        frontSmear.Play();
        float hitDistance = Vector3.Distance(attackPoint.position, playerController.transform.position);
        if(hitDistance <= 1.5 && playerController.gameObject.layer == 3)
        {
            if (!playerAnimation.attacking)
            {
                playerScript.LoseHealth(hitDamage);
                playerScript.LosePoise(hitPoiseDamage);
            }
        }
    }

    void SmearDirection()
    {
        if (playerController.transform.position.x > transform.position.x)
        {
            frontSmear.transform.localScale = frontSmearScale;
            frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, frontSmearRotation.y, frontSmearRotation.z);
            frontSmear.transform.localPosition = frontSmearPosition;
        }
        else
        {
            frontSmear.transform.localScale = new Vector3(-frontSmearScale.x, frontSmearScale.y, frontSmearScale.z);
            frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, -frontSmearRotation.y, frontSmearRotation.z);
            frontSmear.transform.localPosition = new Vector3(-frontSmearPosition.x, frontSmearPosition.y, frontSmearPosition.z);
        }
    }

    public override bool SwordClash()
    {
        float hitDistance = Vector3.Distance(attackPoint.position, playerController.transform.position);
        if (hitDistance <= 1.5 && playerAnimation.attacking)
        {
            return attacking;
        }
        else return false;
    }

    public override void SpecialAbility()
    {
        navAgent.enabled = true;
        chargeDestination = playerController.transform.position;
        charging = true;
    }
}
