using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Flamethrower : EnemyController
{
    AttackArcGenerator attackArc;
    [SerializeField] ParticleSystem frontFlameVFX;
    [SerializeField] ParticleSystem backFlameVFX;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject explosionPrefab;
    public float selfDestructRange;
    [SerializeField] int selfDestructDamage;
    [SerializeField] float selfDestructPoiseDamage;
    FacePlayer facePlayer;
    [System.NonSerialized] public bool isShooting = false;
    float damageBuildup = 0;
    [SerializeField] float flamethrowerDamage;
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
                Artillery();
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public void Attack()
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
        ArcProjectile fireBall = Instantiate(fireballPrefab).GetComponent<ArcProjectile>();
        if (facingFront)
        {
            fireBall.transform.position = frontFlameVFX.transform.position;
        }
        else
        {
            fireBall.transform.position = backFlameVFX.transform.position;
        }
        fireBall.endPoint = new Vector3(playerScript.transform.position.x, 0, playerScript.transform.position.z);
        fireBall.enemyOfOrigin = enemyScript;
    }

    public override void StartStagger(float staggerDuration)
    {
        base.StartStagger(staggerDuration);
        SpecialAbilityOff();
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
        attackArc.HideAttackArc();
        isShooting = false;
    }

    public override void StartDying()
    {
        SpecialAbilityOff();
        enemyEvents.TriggerVfx("PropaneTank");
        base.StartDying();
    }

    private void SelfDestruct(object sender, System.EventArgs e)
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position + new Vector3(0, 1, 0);
        if(Vector3.Distance(playerScript.transform.position, transform.position) <= selfDestructRange)
        {
            playerScript.LoseHealth(selfDestructDamage, EnemyAttackType.NONPARRIABLE, null);
            playerScript.LosePoise(selfDestructPoiseDamage);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        enemyEvents.OnDeath += SelfDestruct;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        enemyEvents.OnDeath -= SelfDestruct;
    }
}
