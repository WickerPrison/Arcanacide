using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceGolemController : EnemyController
{
    [SerializeField] GameObject iceRipplePrefab;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform trackingPoint;
    FacePlayerSlow facePlayer;
    float minWalkAngle = 70;

    public override void Start()
    {
        base.Start();
        facePlayer = GetComponent<FacePlayerSlow>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (hasSeenPlayer)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                AngleMeasurement();
                navAgent.SetDestination(playerController.transform.position);
            } 

            if (Vector3.Distance(transform.position, playerController.transform.position) <= attackRange)
            {
                if (attackTime <= 0)
                {
                    int randN = Random.Range(0, 2);
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
                        Kick();
                    }
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void Smash()
    {
        facePlayer.FacePlayerFast();
        directionLock = true;
        frontAnimator.Play("Smash");
        attacking = true;
        attackTime = attackMaxTime;
    }

    void MultiSmash()
    {
        facePlayer.FacePlayerFast();
        frontAnimator.Play("MultiSmash");
        attacking = true;
        attackTime = attackMaxTime + 1;
    }

    void Kick()
    {
        facePlayer.FacePlayerFast();
        directionLock = true;
        frontAnimator.Play("Kick");
        attacking = true;
        attackTime = attackMaxTime;
    }

    public override void AttackHit(int smearSpeed)
    {
        parryWindow = false;

        if (!canHitPlayer)
        {
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            playerScript.LoseHealth(hitDamage);
            playerScript.LosePoise(hitPoiseDamage);
        }
        else if (playerController.gameObject.layer == 8)
        {
            playerController.PathOfTheSword();
        }
    }

    public override void SpecialAbility()
    {
        GameObject iceRipple = Instantiate(iceRipplePrefab);
        iceRipple.transform.position = attackPoint.transform.position;
    }

    void AngleMeasurement()
    {
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
}
