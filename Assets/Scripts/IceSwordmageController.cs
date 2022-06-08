using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceSwordmageController : EnemyController
{
    [SerializeField] ParticleSystem poof;
    AttackArcGenerator attackArc;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();
        if (hasSeenPlayer)
        {
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if(attackTime <= 0)
            {
                if(Vector3.Distance(transform.position, playerController.transform.position) <= attackRange)
                {
                    int randNum = Random.Range(0, 2);
                    if(randNum == 0)
                    {
                        Attack();
                    }
                    else
                    {
                        Poof();
                    }
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void Attack()
    {
        frontAnimator.Play("Attack");
        attackTime = attackMaxTime;
    }

    void Poof()
    {
        frontAnimator.Play("Poof");
        attackTime = attackMaxTime;
    }

    public override void AdditionalAttackEffects()
    {
        if (!playerScript.shield)
        {
            playerScript.LoseStamina(20);
        }
    }

    public override void SpecialAbility()
    {
        enemySound.OtherSounds(0, 1);
        poof.Play();

        if (!canHitPlayer)
        {
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            playerScript.LosePoise(100);
            AdditionalAttackEffects();
        }
        else if (playerController.gameObject.layer == 8)
        {
            playerController.PathOfTheSword();
        }
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
    }
}
