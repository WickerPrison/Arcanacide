using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MinibossAbilities : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform missileSpawnPoint;
    [SerializeField] FollowPath followPath;
    FollowPoint followPoint;
    NavMeshAgent navMeshAgent;
    EnemyController enemyController;
    EnemyScript enemyScript;
    PlayerScript playerScript;
    float range = 3.5f;
    float spread = 3.5f;
    WaitForSeconds salvoDelay = new WaitForSeconds(0.3f);
    float defaultSpeed = 2.85f;
    float dashSpeed = 15f;
    float defaultAccel = 8;
    float dashAccel = 25;



    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyScript = GetComponent<EnemyScript>();
        playerScript = enemyController.playerScript;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void MissileAttack()
    {
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("Missiles");
        enemyController.backAnimator.Play("Missiles");
    }

    public void SingleMissile(Vector3 target, float timeToHit)
    {
        ArcProjectile missile = Instantiate(missilePrefab).GetComponent<ArcProjectile>();
        missile.transform.position = missileSpawnPoint.position;
        missile.endPoint = target;
        missile.enemyOfOrigin = enemyScript;
        missile.timeToHit = timeToHit;
    }

    public void FireMissiles()
    {
        StartCoroutine(MissileCoroutine());
    }

    IEnumerator MissileCoroutine()
    {
        Vector3 playerDirection = Vector3.Normalize(playerScript.transform.position - transform.position);
        for (int i = 1; i < 4; i++)
        {
            Vector3 forwardPosition = transform.position + playerDirection * i * range;
            for (int j = -2; j <= 2; j++)
            {
                Vector3 target = forwardPosition + Vector3.Cross(playerDirection, Vector3.up).normalized * j * spread;
                target += new Vector3(
                    UnityEngine.Random.Range(-1f, 1f), 
                    UnityEngine.Random.Range(-1f, 1f), 
                    UnityEngine.Random.Range(-1f, 1f));
                SingleMissile(target, 0.5f + (float)Mathf.Abs(j) / 4);
            }
            yield return salvoDelay;
        }
    }

    public void MeleeBlade()
    {
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("Blade1");
        enemyController.backAnimator.Play("Blade1");
    }

    public void Dash()
    {
        enemyController.frontAnimator.Play("StartDash");
        enemyController.backAnimator.Play("StartDash");
        navMeshAgent.acceleration = dashAccel;
        navMeshAgent.speed = dashSpeed;
        StartCoroutine(Dashing(() =>
        {
            enemyController.frontAnimator.Play("EndDash");
            enemyController.backAnimator.Play("EndDash");
            navMeshAgent.acceleration = defaultAccel;
            navMeshAgent.speed = defaultSpeed;
            navMeshAgent.velocity = Vector3.zero;
        }));
    }

    public void AttackDash(string endAnimation)
    {
        enemyController.directionLock = false;
        navMeshAgent.enabled = true;
        navMeshAgent.acceleration = dashAccel;
        navMeshAgent.speed = dashSpeed;
        StartCoroutine(Dashing(() =>
        {
            enemyController.frontAnimator.Play(endAnimation);
            enemyController.backAnimator.Play(endAnimation);
            navMeshAgent.acceleration = defaultAccel;
            navMeshAgent.speed = defaultSpeed;
            navMeshAgent.velocity = Vector3.zero;
        }));
    }

    IEnumerator Dashing(Action callback)
    {
        while (enemyController.playerDistance > 2)
        {
            yield return null;
        }

        callback();
    }

    public void Circle()
    {
        enemyController.frontAnimator.Play("StartDash");
        enemyController.backAnimator.Play("StartDash");
        navMeshAgent.acceleration = dashAccel;
        navMeshAgent.speed = dashSpeed;
        navMeshAgent.autoBraking = false;
        enemyController.state = EnemyState.SPECIAL;
        followPoint = followPath.GetClosestPoint(transform.position);
    }

    public void FollowCircle()
    {
        if (Vector3.Distance(transform.position, followPoint.transform.position) <= navMeshAgent.stoppingDistance)
        {
            followPoint = followPoint.next;
        }
        navMeshAgent.SetDestination(followPoint.transform.position);
    }
}
