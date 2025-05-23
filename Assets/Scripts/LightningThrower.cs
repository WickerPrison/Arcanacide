using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightningThrower : EnemyController
{
    AttackArcGenerator attackArc;
    [SerializeField] ParticleSystem frontElectricityVFX;
    [SerializeField] ParticleSystem backElectricityVFX;
    [SerializeField] GameObject lightningBallPrefab;
    FacePlayer facePlayer;
    bool isShocking = false;
    float shockDamageBuildup;
    [SerializeField] float lightningThrowerDamage;
    float shockTimer = 0;
    [SerializeField] EventReference lightningSFX;
    [SerializeField] float lightningVolume;

    public override void Start()
    {
        base.Start();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        facePlayer = GetComponent<FacePlayer>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (isShocking && attackArc.CanHitPlayer() && playerAbilities.gameObject.layer == 3)
        {
            shockDamageBuildup += Time.deltaTime * lightningThrowerDamage;
            shockTimer += Time.deltaTime;
            if(shockTimer > 0.15f)
            {
                playerScript.LoseHealth(Mathf.RoundToInt(shockDamageBuildup), EnemyAttackType.NONPARRIABLE, null);
                shockDamageBuildup = 0;
                shockTimer = 0;
                playerScript.StartStagger(0.2f);

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
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;

        frontAnimator.Play("Attack");
        backAnimator.Play("Attack");
    }

    void Artillery()
    {
        state = EnemyState.ATTACKING;
        attackTime = attackMaxTime;

        frontAnimator.Play("Artillery");
        backAnimator.Play("Artillery");
    }

    public override void SpellAttack()
    {
        enemySound.OtherSounds(0, 1);
        ArcProjectile lightningBall = Instantiate(lightningBallPrefab).GetComponent<ArcProjectile>();
        if (facingFront)
        {
            lightningBall.transform.position = frontElectricityVFX.transform.position;
        }
        else
        {
            lightningBall.transform.position = backElectricityVFX.transform.position;
        }
        lightningBall.endPoint = new Vector3(playerScript.transform.position.x, 0, playerScript.transform.position.z);
        lightningBall.enemyOfOrigin = enemyScript;
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        attackArc.HideAttackArc();
        SpecialAbilityOff();
    }

    public override void SpecialAbility()
    {
        Vector3 direction = new Vector3(facePlayer.faceDirection.x, -90, facePlayer.faceDirection.z);
        if (facingFront)
        {
            frontElectricityVFX.transform.rotation = Quaternion.LookRotation(direction.normalized);
            frontElectricityVFX.Play();
        }
        else
        {
            backElectricityVFX.transform.rotation = Quaternion.LookRotation(direction.normalized);
            backElectricityVFX.Play();
        }
        enemySound.Play(lightningSFX, lightningVolume);
        isShocking = true;
    }

    public override void SpecialAbilityOff()
    {
        frontElectricityVFX.Stop();
        backElectricityVFX.Stop();
        enemySound.Stop();
        isShocking = false;
    }

    public override void StartDying()
    {
        SpecialAbilityOff();
        base.StartDying();
    }
}
