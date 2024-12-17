using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Flamethrower : EnemyController
{
    AttackArcGenerator attackArc;
    [SerializeField] ParticleSystem frontFlameVFX;
    [SerializeField] ParticleSystem backFlameVFX;
    FacePlayer facePlayer;
    bool isShooting = false;
    float damageBuildup = 0;
    float flamethrowerDamage = 20f;
    float flameTimer = 0;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        facePlayer = GetComponent<FacePlayer>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (isShooting && attackArc.CanHitPlayer() && playerAbilities.gameObject.layer == 3)
        {
            damageBuildup += Time.deltaTime * flamethrowerDamage;
            flameTimer += Time.deltaTime;
            if(flameTimer > 0.15f)
            {
                playerScript.LoseHealth(Mathf.RoundToInt(damageBuildup), EnemyAttackType.NONPARRIABLE, null);
                damageBuildup = 0;
                flameTimer = 0;
            }
        }

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
            else if (attackTime <= 0)
            {
                //Artillery();
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void Attack()
    {
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;

        frontAnimator.Play("Attack");
        backAnimator.Play("Attack");
    }

    public override void SpecialAbility()
    {
        Vector3 direction = new Vector3(facePlayer.faceDirection.x, -90, facePlayer.faceDirection.z);
        if (facingFront)
        {
            frontFlameVFX.transform.rotation = Quaternion.LookRotation(direction.normalized);
            frontFlameVFX.Play();
        }
        else
        {
            backFlameVFX.transform.rotation = Quaternion.LookRotation(direction.normalized);
            backFlameVFX.Play();
        }
        //enemySound.Play(lightningSFX, lightningVolume);
        isShooting = true;
    }

    public override void SpecialAbilityOff()
    {
        frontFlameVFX.Stop();
        backFlameVFX.Stop();
        //enemySound.Stop();
        isShooting = false;
    }

    public override void StartDying()
    {
        SpecialAbilityOff();
        base.StartDying();
    }
}
