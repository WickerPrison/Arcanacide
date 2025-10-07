using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ElementalistController : EnemyController
{
    [SerializeField] Transform frontAttackPoint;
    [SerializeField] Transform backAttackPoint;
    [SerializeField] GameObject iceRipplePrefab;
    [SerializeField] GameObject chaosOrbPrefab;
    [SerializeField] Transform chaosHeadFront;
    [SerializeField] Transform chaosHeadBack;
    [SerializeField] GameObject bubblesPrefab;
    StepWithAttack stepWithAttack;
    float meleeRange = 4;
    int chaosOrbNum = 30;
    float angleDiff = 15;
    WaitForSeconds chaosOrbDelay = new WaitForSeconds(0.1f);
    float plantLineNum = 6;
    Vector3 chaosOrbVert = new Vector3(0, 0, 0);
    AttackArcGenerator attackArc;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
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

            if (playerDistance < meleeRange && attackTime <= 0)
            {
                state = EnemyState.ATTACKING;
                PlayAnimation("SwordAttack");
                attackTime = attackMaxTime;
            }
            else if (playerDistance < attackRange && attackTime <= 0)
            {
                state = EnemyState.ATTACKING;
                attackTime = attackMaxTime;
                int randInt = Random.Range(0, 4);
                switch (randInt)
                {
                    case 0:
                        PlayAnimation("CastSpell");
                        break;
                    case 1:
                        PlayAnimation("IceStomp");
                        break;
                    case 2:
                        PlayAnimation("ChaosHead");
                        break;
                    case 3:
                        PlayAnimation("Bubbles");
                        break;
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public void PlayAnimation(string animationName)
    {
        frontAnimator.Play(animationName);
        backAnimator.Play(animationName);
    }

    public override void SpellAttack()
    {
        GameObject projectile;
        HomingProjectile projectileScript;
        projectile = Instantiate(projectilePrefab);
        projectileScript = projectile.GetComponent<HomingProjectile>();
        if (facingFront)
        {
            projectile.transform.position = frontAttackPoint.position;
        }
        else
        {
            projectile.transform.position = backAttackPoint.position;
        }
        projectile.transform.LookAt(playerScript.transform.position);
        projectileScript.target = playerScript.transform;
        projectileScript.poiseDamage = spellAttackPoiseDamage;
        projectileScript.spellDamage = spellAttackDamage;
        projectileScript.enemyOfOrigin = enemyScript;
        enemySound.OtherSounds(0, 1);
    }

    public void SwingSword(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        stepWithAttack.Step(0.15f);
        enemySound.SwordSwoosh();
    }

    public void SwooshShock()
    {
        if (!canHitPlayer)
        {
            enemySound.OtherSounds(2, 1);
            return;
        }

        if (playerScript.gameObject.layer == 3)
        {
            enemySound.OtherSounds(1, 1);
            playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerScript.gameObject.layer == 8)
        {
            enemySound.OtherSounds(2, 1);
            playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
        }
    }

    public void IceStomp()
    {
        enemySound.OtherSounds(0, 1);
        IceRipple iceRipple = Instantiate(iceRipplePrefab).GetComponent<IceRipple>();
        iceRipple.transform.position = transform.position + new Vector3(0, 1, 0);
        iceRipple.damage = 14;
        iceRipple.poiseDamage = 10;
        iceRipple.enemyOfOrigin = enemyScript;
    }

    public IEnumerator ChaosHead()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        float angle = 0;

        for(int i = 0; i < chaosOrbNum; i++)
        {
            Projectile chaosOrb = Instantiate(chaosOrbPrefab).GetComponent<Projectile>();

            if (facingFront)
                chaosOrb.transform.position = chaosHeadFront.position;
            else
                chaosOrb.transform.position = chaosHeadBack.position;

            chaosOrb.direction = Utils.RotateDirection(direction, angle) + chaosOrbVert;
            chaosOrb.speed = 6;
            chaosOrb.enemyOfOrigin = enemyScript;
            angle += angleDiff;

            BiasProjectileHeight projectileHeight = chaosOrb.GetComponent<BiasProjectileHeight>();
            projectileHeight.speed = 1;

            enemySound.EnemySpell();
            yield return chaosOrbDelay;
        }

        frontAnimator.Play("ChaosHeadEnd");
        backAnimator.Play("ChaosHeadEnd");
    }

    public void Bubbles()
    {
        for(int i = 0; i < plantLineNum; i++)
        {
            AssistantBeam bubbles = Instantiate(bubblesPrefab).GetComponent<AssistantBeam>();
        }
    }

    public override void StartStagger(float staggerDuration)
    {
        attackArc.HideAttackArc();
        base.StartStagger(staggerDuration);
    }

    public override void StartDying()
    {
        chaosHeadFront.GetComponent<ParticleSystem>().Stop();
        chaosHeadBack.GetComponent<ParticleSystem>().Stop();
        attackArc.HideAttackArc();
        base.StartDying();
    }
}
