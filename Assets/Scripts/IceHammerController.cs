using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHammerController : EnemyController
{
    AttackArcGenerator attackArc;
    FacePlayer facePlayer;
    [SerializeField] float radius;
    Collider enemyCollider;
    Vector3 jumpDestination;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        facePlayer = GetComponent<FacePlayer>();
        enemyCollider = GetComponent<Collider>();
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
                    HammerSmash();
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public void HammerSmash()
    {
        frontAnimator.Play("DoubleAttack");
        backAnimator.Play("DoubleAttack");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
    }
    public void AreaSmash()
    {
        bool playerInRange = Vector3.Distance(playerScript.transform.position, facePlayer.attackPoint.position) <= radius;
        if (playerInRange)
        {
            playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
        }
    }


    public void JumpSmash()
    {
        frontAnimator.Play("JumpSmash");
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;
    }

    public void StartJump()
    {
        enemyCollider.enabled = false;
        jumpDestination = playerScript.transform.position;
    }

    public override void AttackHit(int smearSpeed)
    {
        enemySound.OtherSounds(0, 2);
        base.AttackHit(smearSpeed);
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
    }
}
