using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ElectricBossController : EnemyController
{
    [SerializeField] Transform[] firePoints;
    [SerializeField] GameObject hadokenPrefab;
    float playerDistance;
    StepWithAttack stepWithAttack;
    FacePlayer facePlayer;
    float fleeRadiusMin = 6;
    float fleeRadiusMax = 11;
    Vector3 fleePoint;
    bool canHadoken = true;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
        facePlayer = GetComponent<FacePlayer>();
        ChooseRandomPoint();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();
        if (hasSeenPlayer)
        {
            playerDistance = Vector3.Distance(playerController.transform.position, transform.position);
            if(attackTime > 0)
            {
                if (canHadoken && !attacking)
                {
                    StartCoroutine(HadokenTimer());
                    frontAnimator.Play("Hadoken");
                    backAnimator.Play("Hadoken");
                }

                if(Vector3.Distance(playerController.transform.position, fleePoint) < fleeRadiusMin)
                {
                    ChooseRandomPoint();
                }

                if (navAgent.enabled)
                {
                    navAgent.SetDestination(fleePoint);
                    if(navAgent.velocity.magnitude  > 0)
                    {
                        facePlayer.SetDestination(fleePoint);
                    }
                    else
                    {
                        facePlayer.ResetDestination();
                    }
                }
            }
            else if(playerDistance < 2.5)
            {
                ChooseRandomPoint();
                attackTime = attackMaxTime;
                frontAnimator.Play("Attack");
                backAnimator.Play("Attack");
                attacking = true;
            }
            else if(navAgent.enabled)
            {
                facePlayer.ResetDestination();
                navAgent.SetDestination(playerController.transform.position);
            }
        }

        if(attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void ChooseRandomPoint()
    {
        int xDir = Random.Range(1, 3);
        int yDir = Random.Range(1, 3);
        float xPos = Random.Range(fleeRadiusMin, fleeRadiusMax);
        float zPos = Random.Range(fleeRadiusMin, fleeRadiusMax);
        Vector3 startPos = playerController.transform.position + new Vector3(xPos * Mathf.Pow(-1, xDir), 0, zPos * Mathf.Pow(-1, yDir));
        NavMeshHit hit;
        NavMesh.SamplePosition(startPos, out hit, fleeRadiusMax + 1, NavMesh.AllAreas);
        fleePoint = hit.position;
    }

    public void SwingSword(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        stepWithAttack.Step(0.15f);
        enemySound.SwordSwoosh();
    }

    public void SwooshShock()
    {
        parryWindow = false;

        if (!canHitPlayer)
        {
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            playerScript.LoseHealth(hitDamage, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerController.gameObject.layer == 8)
        {
            playerController.PerfectDodge();
        }
    }

    public void Hadoken()
    {
        Hadoken hadoken = Instantiate(hadokenPrefab).GetComponent<Hadoken>();
        int frontOrBack = facingFront ? 0 : 1;
        hadoken.transform.position = firePoints[frontOrBack].position;
        hadoken.direction = playerController.transform.position + new Vector3(0, 1, 0) - firePoints[frontOrBack].position;
    }

    IEnumerator HadokenTimer()
    {
        canHadoken = false;
        yield return new WaitForSeconds(5);
        canHadoken = true;
    }
}
