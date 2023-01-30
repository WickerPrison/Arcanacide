using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightningThrower : EnemyController
{
    AttackArcGenerator attackArc;
    [SerializeField] ParticleSystem electricityVFX;
    [SerializeField] GameObject lightningBallPrefab;
    FacePlayer facePlayer;
    bool isShocking = false;
    float shockDamageBuildup;
    float lightningThrowerDamage = 20;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        facePlayer = GetComponent<FacePlayer>();
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
                    Attack();
                }
            }
            else if(attackTime <= 0)
            {
                Artillery();
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void Attack()
    {
        attacking = true;
        attackTime = attackMaxTime;

        frontAnimator.Play("Attack");
        backAnimator.Play("Attack");
    }

    void Artillery()
    {
        attacking = true;
        attackTime = attackMaxTime;

        frontAnimator.Play("Artillery");
        backAnimator.Play("Artillery");
    }

    public override void SpellAttack()
    {
        enemySound.OtherSounds(0, 1);
        ArcProjectile lightningBall = Instantiate(lightningBallPrefab).GetComponent<ArcProjectile>();
        lightningBall.transform.position = electricityVFX.transform.position;
        lightningBall.endPoint = new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z);
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
        SpecialAbilityOff();
    }

    public override void SpecialAbility()
    {
        Vector3 direction = new Vector3(facePlayer.playerDirection.x, -90, facePlayer.playerDirection.z);
        electricityVFX.transform.rotation = Quaternion.LookRotation(direction.normalized);
        electricityVFX.Play();
        enemySound.SFX.Play();
        isShocking = true;
    }

    public override void SpecialAbilityOff()
    {
        electricityVFX.Stop();
        enemySound.SFX.Stop();
        isShocking = false;
    }


    private void FixedUpdate()
    {
        if (isShocking && attackArc.CanHitPlayer())
        {
            shockDamageBuildup += Time.fixedDeltaTime * lightningThrowerDamage;
            while (shockDamageBuildup > 1)
            {
                shockDamageBuildup -= 1;
                playerScript.LoseHealth(1);
            }
            playerScript.StartStagger(Time.fixedDeltaTime * 1.1f);

        }
    }
}
