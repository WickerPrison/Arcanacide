using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[System.Serializable]
public class ChaosMageController : EnemyController
{
    private enum BeamState
    {
        OFF, INDICATOR, ON
    }
    BeamState beamState = BeamState.OFF;
    [SerializeField] Transform frontAttackOrigin;
    [SerializeField] Transform backAttackOrigin;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform attackPoint;
    Vector3 attackOrigin;
    Vector3 targetDirection;
    Vector3 endPoint;
    LayerMask layerMask;
    bool hitPlayer;
    bool hitPlayerDelay;
    WaitForSeconds hitDelay = new WaitForSeconds(0.2f);
    WaitForSeconds beamDuration = new WaitForSeconds(2f);
    [SerializeField] GameObject beam;
    float beamLength;


    public override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Player", "Default");
        lineRenderer.enabled = false;
        beam.SetActive(false);
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

            if (playerDistance < attackRange && attackTime <= 0)
            {
                int randInt = Random.Range(0, 2);
                if (randInt == 0)
                {
                    attackTime = attackMaxTime;
                    frontAnimator.Play("CastSpell");
                    backAnimator.Play("CastSpell");
                }
                else
                {
                    attackTime = 1000;
                    frontAnimator.Play("StartBeam");
                    backAnimator.Play("StartBeam");
                }
            }
        }

        if(beamState == BeamState.INDICATOR)
        {
            PerformRaycast();
            lineRenderer.SetPosition(0, attackOrigin);
            lineRenderer.SetPosition(1, endPoint + Vector3.up);
        }
        else if(beamState == BeamState.ON)
        {
            PerformRaycast();
            Vector3 beamDirection = endPoint + Vector3.up - attackOrigin;
            beam.transform.position = attackOrigin + beamDirection.normalized * beamLength / 2;
            Quaternion direction = Quaternion.LookRotation(beamDirection, Vector3.up);
            beam.transform.rotation = direction;
            beam.transform.localScale = new Vector3(0.3f, 0.3f, beamLength * .8f);
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public override void SpellAttack()
    {
        GameObject projectile;
        Projectile projectileScript;
        projectile = Instantiate(projectilePrefab);
        projectileScript = projectile.GetComponent<Projectile>();
        GetDirection();
        projectile.transform.position = attackOrigin; 
        float randomAngle = Random.Range(-20, 20);
        projectileScript.direction = RotateDirection(targetDirection, randomAngle);
        projectileScript.poiseDamage = spellAttackPoiseDamage;
        projectileScript.spellDamage = spellAttackDamage;
        projectileScript.enemyOfOrigin = enemyScript;
        //enemySound.OtherSounds(0, 1);
    }

    public override void SpecialAbility()
    {
        lineRenderer.enabled = true;
        beamState = BeamState.INDICATOR;
    }

    public override void SpecialAbilityOff()
    {
        lineRenderer.enabled = false;
        beam.SetActive(true);
        beamState = BeamState.ON;
        StartCoroutine(BeamDuration());
    }

    IEnumerator BeamDuration()
    {
        yield return beamDuration;
        beamState = BeamState.OFF;
        beam.SetActive(false);
        frontAnimator.Play("EndBeam");
        backAnimator.Play("EndBeam");
        attackTime = attackMaxTime;
    }

    void PerformRaycast()
    {
        GetDirection();
        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up, targetDirection, out hit, attackRange, layerMask, QueryTriggerInteraction.Ignore);
   
        if (hit.collider == null)
        {
            endPoint = transform.position + Vector3.up + targetDirection.normalized * attackRange;
            beamLength = attackRange;
            return;
        }
        else
        {
            endPoint = hit.point;
            beamLength = hit.distance;
        }

        hitPlayer = hit.collider.CompareTag("Player");
        if (hitPlayer)
        {
            if (hitPlayerDelay || beamState != BeamState.ON)
            {
                hitPlayer = false;
            }
            else
            {
                DealDamage();
            }
        }
    }

    void DealDamage()
    {
        StartCoroutine(HitPlayerDelay());
        playerScript.LoseHealth(3,EnemyAttackType.NONPARRIABLE, enemyScript);
        playerScript.LosePoise(3);
    }

    IEnumerator HitPlayerDelay()
    {
        hitPlayerDelay = true;
        yield return hitDelay;
        hitPlayerDelay = false;
    }

    void GetDirection()
    {
        Vector3 target = attackPoint.position;
        if (facingFront)
        {
            attackOrigin = frontAttackOrigin.position;
        }
        else
        {
            attackOrigin = backAttackOrigin.position;
        }
        targetDirection = (target - transform.position).normalized;
    }

    Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
    }
}
