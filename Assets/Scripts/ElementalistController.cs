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
    [SerializeField] Transform chaosHead;
    [SerializeField] GameObject plantLinePrefab;
    StepWithAttack stepWithAttack;
    float meleeRange = 4;
    int chaosOrbNum = 30;
    float angleDiff = 15;
    WaitForSeconds chaosOrbDelay = new WaitForSeconds(0.1f);
    float plantLineNum = 6;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if (playerDistance < meleeRange && attackTime <= 0)
            {
                state = EnemyState.ATTACKING;
                frontAnimator.Play("SwordAttack");
                backAnimator.Play("SwordAttack");
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
                        frontAnimator.Play("CastSpell");
                        backAnimator.Play("CastSpell");
                        break;
                    case 1:
                        frontAnimator.Play("IceStomp");
                        backAnimator.Play("IceStomp");
                        break;
                    case 2:
                        frontAnimator.Play("ChaosHead");
                        backAnimator.Play("ChaosHead");
                        break;
                    case 3:
                        frontAnimator.Play("PlantAttack");
                        backAnimator.Play("PlantAttack");
                        break;
                }
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
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
        projectile.transform.LookAt(playerController.transform.position);
        projectileScript.target = playerController.transform;
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

        if (playerController.gameObject.layer == 3)
        {
            enemySound.OtherSounds(1, 1);
            playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerController.gameObject.layer == 8)
        {
            enemySound.OtherSounds(2, 1);
            playerController.PerfectDodge();
        }
    }

    public void IceStomp()
    {
        enemySound.OtherSounds(0, 1);
        GameObject iceRipple = Instantiate(iceRipplePrefab);
        iceRipple.transform.position = transform.position + new Vector3(0, 1, 0);
    }

    public IEnumerator ChaosHead()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        float angle = 0;

        for(int i = 0; i < chaosOrbNum; i++)
        {
            Projectile chaosOrb = Instantiate(chaosOrbPrefab).GetComponent<Projectile>();
            chaosOrb.transform.position = chaosHead.position;
            chaosOrb.direction = RotateDirection(direction, angle);
            chaosOrb.speed = 6;
            angle += angleDiff;
            yield return chaosOrbDelay;
        }

        frontAnimator.Play("ChaosHeadEnd");
        backAnimator.Play("ChaosHeadEnd");
    }

    public void PlantAttack()
    {
        for(int i = 0; i < plantLineNum; i++)
        {
            Vector3 startPos = transform.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            NavMeshHit hit;
            NavMesh.SamplePosition(startPos, out hit, 10, NavMesh.AllAreas);

            Instantiate(plantLinePrefab).transform.position = hit.position;
        }
    }

    Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
    }
}