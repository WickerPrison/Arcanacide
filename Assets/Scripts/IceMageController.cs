using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceMageController : EnemyController
{
    [SerializeField] FireRing iceRing;
    float tooClose = 3.5f;
    int fireBallDamage = 15;
    int fireBallPoiseDamage = 15;
    int fireRingDamage = 30;
    float fireRingPoiseDamage = 100;

    public override void Start()
    {
        base.Start();
        spellAttackDamage = fireBallDamage;
        spellAttackPoiseDamage = fireBallPoiseDamage;
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

            if (Vector3.Distance(transform.position, playerController.transform.position) < tooClose)
            {
                if (attackTime <= 0)
                {
                    frontAnimator.Play("IceBlast");
                    backAnimator.Play("IceBlast");
                    attackTime = attackMaxTime;
                }
            }
            else if (Vector3.Distance(transform.position, playerController.transform.position) < attackRange)
            {
                if (attackTime <= 0)
                {
                    frontAnimator.Play("CastSpell");
                    backAnimator.Play("CastSpell");
                    attackTime = attackMaxTime;
                }
            }

        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void BackUp()
    {
        Vector3 awayDirection = transform.position - playerController.transform.position;
        if (navAgent.enabled)
        {
            navAgent.Move(awayDirection.normalized * Time.deltaTime * navAgent.speed / 2);
        }
    }

    public override void SpellAttack()
    {
        GameObject projectile;
        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = playerController.transform.position;
    }

    public override void SpecialAbility()
    {
        iceRing.Explode();
        if (playerScript.shield)
        {
            return;
        }

        if (Vector3.Distance(transform.position, playerController.transform.position) < tooClose && playerController.gameObject.layer == 3)
        {
            playerScript.LoseHealth(fireRingDamage);
            playerScript.LosePoise(fireRingPoiseDamage);
            Rigidbody playerRB = playerScript.gameObject.GetComponent<Rigidbody>();
            Vector3 awayVector = playerController.transform.position - transform.position;
            playerController.knockback = true;
            PlayerAnimation playerAnimation = playerController.gameObject.GetComponent<PlayerAnimation>();
            playerAnimation.attacking = false;
            playerRB.velocity = Vector3.zero;
            playerRB.AddForce(awayVector.normalized * 7, ForceMode.VelocityChange);
        }
    }
}
