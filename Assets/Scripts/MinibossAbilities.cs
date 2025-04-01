using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MinibossAbilities : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform missileSpawnPoint;
    public Ellipse ellipse;
    FacePlayer facePlayer;
    NavMeshAgent navMeshAgent;
    EnemyController enemyController;
    EnemyScript enemyScript;
    PlayerScript playerScript;
    float range = 3.5f;
    float spread = 3.5f;
    WaitForSeconds salvoDelay = new WaitForSeconds(0.3f);
    float defaultSpeed = 2.85f;
    float dashSpeed = 20f;
    float defaultAccel = 8;
    float dashAccel = 45;
    float dashDistance = 10;
    bool onCircle = false;
    Vector3 circleStart;
    [SerializeField] float getToCircleSpeed;
    [SerializeField] float timeToCircle;
    float startingRads;
    float ellipseRads;
    [SerializeField] GameObject plasmaBallPrefab;
    [SerializeField] Transform shotOrigin;
    [SerializeField] float plasmaCooldown;
    float plasmaTimer;
    [SerializeField] GameObject beam;
    [SerializeField] Transform beamOrigin;
    [SerializeField] Transform dashTarget;
    Vector3 initialBeamDirection;
    Vector3 finalBeamDirection;
    float beamVertAngle = 87;
    [System.NonSerialized] public Transform navMeshDestination;
    AttackArcGenerator attackArc;
    MinibossEvents minibossEvents;

    enum LaserState 
    {
        START, SWEEP, PAUSE, END, OFF        
    }
    LaserState laserState = LaserState.OFF;
    float laserTimer;
    [SerializeField] float pauseTime;
    [SerializeField] float sweepTime;
    float sweepHalfWidth = 65;
    int sweeps;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyScript = GetComponent<EnemyScript>();
        minibossEvents = GetComponent<MinibossEvents>();
        playerScript = enemyController.playerScript;
        navMeshAgent = GetComponent<NavMeshAgent>();
        facePlayer = GetComponent<FacePlayer>();
        beam.SetActive(false);
        beam.transform.parent = null;
        dashTarget.transform.parent = null;
        navMeshDestination = playerScript.transform;
        attackArc = GetComponentInChildren<AttackArcGenerator>();
    }

    private void FixedUpdate()
    {
        if(enemyController.state == EnemyState.SPECIAL)
        {
            if (onCircle)
            {
                FollowCircle();
            }
            else
            {
                GetToTheCircle();
            }
        }

        LaserSweep();
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
                    0, 
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

    public void DashAway(Action nextAction)
    {
        enemyController.state = EnemyState.ATTACKING;
        Vector3 direction = transform.position - playerScript.transform.position;
        NavMeshHit hit;
        bool foundDest = false;
        int sign = UnityEngine.Random.Range(0, 2) * 2 - 1;
        while (!foundDest)
        {
            foundDest = NavMesh.SamplePosition(
                playerScript.transform.position + direction.normalized * dashDistance, 
                out hit, 
                1, 
                NavMesh.AllAreas
            );

            if (!foundDest)
            {
                direction = Utils.RotateDirection(direction.normalized, sign * 15);
            }
            else
            {
                dashTarget.position = hit.position;
            }
        }
        navMeshDestination = dashTarget;
        facePlayer.SetDestination(dashTarget.position);
        enemyController.frontAnimator.Play("StartDash");
        enemyController.backAnimator.Play("StartDash");
        navMeshAgent.acceleration = dashAccel;
        navMeshAgent.speed = dashSpeed;
        StartCoroutine(Dashing(dashTarget, () =>
        {
            enemyController.frontAnimator.Play("EndDash");
            enemyController.backAnimator.Play("EndDash");
            minibossEvents.ThrustersOff();
            navMeshAgent.acceleration = defaultAccel;
            navMeshAgent.speed = defaultSpeed;
            navMeshAgent.velocity = Vector3.zero;
            navMeshDestination = playerScript.transform;
            facePlayer.ResetDestination();
            facePlayer.ManualFace();
            nextAction();
        }));
    }

    public void AttackDash(string endAnimation)
    {
        enemyController.state = EnemyState.ATTACKING;
        enemyController.directionLock = false;
        navMeshAgent.enabled = true;
        navMeshAgent.acceleration = dashAccel;
        navMeshAgent.speed = dashSpeed;
        StartCoroutine(Dashing(playerScript.transform, () =>
        {
            enemyController.frontAnimator.Play(endAnimation);
            enemyController.backAnimator.Play(endAnimation);
            navMeshAgent.acceleration = defaultAccel;
            navMeshAgent.speed = defaultSpeed;
            navMeshAgent.velocity = Vector3.zero;
        }));
    }

    IEnumerator Dashing(Transform target, Action callback)
    {
        while (Vector3.Distance(target.position, transform.position) > 2)
        {
            yield return null;
        }

        callback();
    }

    public void Circle()
    {
        enemyController.frontAnimator.Play("StartDash");
        enemyController.backAnimator.Play("StartDash");
        navMeshAgent.enabled = false;
        onCircle = false;
        (circleStart, startingRads) = ellipse.GetStartingPosition(transform.position);
        ellipseRads = startingRads;
        enemyController.state = EnemyState.SPECIAL;
    }

    void GetToTheCircle()
    {
        Vector3 direction = (circleStart - transform.position).normalized;
        facePlayer.SetDestination(direction);
        transform.Translate(Time.fixedDeltaTime * getToCircleSpeed * (circleStart - transform.position).normalized);
        if (Vector3.Distance(transform.position, circleStart) < Time.deltaTime *getToCircleSpeed)
        {
            onCircle = true;
            facePlayer.ResetDestination();
            plasmaTimer = plasmaCooldown;
            enemyController.frontAnimator.Play("ShootDash");
            enemyController.backAnimator.Play("ShootDash");
        }
    }

    public void FollowCircle()
    {
        facePlayer.ManualFace();
        ellipseRads += Time.fixedDeltaTime / timeToCircle * 2 * Mathf.PI;
        transform.position = ellipse.GetPosition(ellipseRads);

        if(plasmaTimer > 0)
        {
            plasmaTimer -= Time.fixedDeltaTime;
            if(plasmaTimer <= 0)
            {
                plasmaTimer = plasmaCooldown;
                FirePlasmaShot();
            }
        }

        if(ellipseRads >= startingRads + 2 * MathF.PI)
        {
            navMeshAgent.enabled = true;
            enemyController.state = EnemyState.IDLE;
            enemyController.frontAnimator.Play("ShootDashEnd");        
            enemyController.backAnimator.Play("ShootDashEnd");        
        }
    }

    public void FirePlasmaShot()
    {
        HomingProjectile shot = Instantiate(plasmaBallPrefab).GetComponent<HomingProjectile>();
        shot.transform.position = shotOrigin.transform.position;
        shot.target = playerScript.transform;
        shot.enemyOfOrigin = enemyScript;
        shot.transform.LookAt(playerScript.transform);
    }

    public void ChestLaser(int sweepsCount)
    {
        if(Mathf.Abs(transform.position.z - playerScript.transform.position.z) > 6
           || Mathf.Abs(transform.position.x - playerScript.transform.position.x) > 8)
        {
            MissileAttack();
            return;
        }
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("ChestLaserStart");
        enemyController.backAnimator.Play("ChestLaserStart");
        initialBeamDirection = Utils.RotateDirection(facePlayer.faceDirection.normalized, -sweepHalfWidth);
        finalBeamDirection = Utils.RotateDirection(facePlayer.faceDirection.normalized, sweepHalfWidth);
        facePlayer.SetDestination(transform.position + initialBeamDirection);
        facePlayer.ManualFace();
        sweeps = sweepsCount;
    }

    public void StartLaser()
    {
        beam.SetActive(true);
        SetBeamPosition(initialBeamDirection.normalized);
        laserState = LaserState.START;
        laserTimer = pauseTime;
    }

    void LaserSweep()
    {
        switch (laserState)
        {
            case LaserState.START:
                laserTimer -= Time.fixedDeltaTime;
                if (laserTimer <= 0)
                {
                    laserTimer = sweepTime;
                    laserState = LaserState.SWEEP;
                }
                break;
            case LaserState.SWEEP:
                laserTimer -= Time.fixedDeltaTime;
                float t = Mathf.SmoothStep(1, 0, laserTimer / sweepTime);
                SetBeamPosition(Vector3.Lerp(initialBeamDirection, finalBeamDirection, t).normalized);
                if (laserTimer <= 0)
                {
                    laserTimer = pauseTime;
                    sweeps -= 1;
                    if (sweeps == 0) laserState = LaserState.END;
                    else laserState = LaserState.PAUSE;
                }
                break;
            case LaserState.PAUSE:
                laserTimer -= Time.fixedDeltaTime;
                if(laserTimer <= 0)
                {
                    laserTimer = sweepTime;
                    Vector3 temp = initialBeamDirection;
                    initialBeamDirection = finalBeamDirection;
                    finalBeamDirection = temp;
                    laserState = LaserState.SWEEP;
                }
                break;
            case LaserState.END:
                laserTimer -= Time.fixedDeltaTime;
                if (laserTimer <= 0)
                {
                    beam.SetActive(false);
                    enemyController.frontAnimator.Play("ChestLaserEnd");
                    enemyController.backAnimator.Play("ChestLaserEnd");
                    facePlayer.ResetDestination();
                    laserState = LaserState.OFF;
                }
                break;
        }
    }

    void SetBeamPosition(Vector3 direction)
    {
        beam.transform.position = beamOrigin.position + direction * beam.transform.localScale.y;
        beam.transform.LookAt(beamOrigin.position);
        beam.transform.localEulerAngles = new Vector3(
            beamVertAngle,
            beam.transform.localEulerAngles.y,
            beam.transform.localEulerAngles.z);
        facePlayer.SetDestination(beam.transform.position);
        facePlayer.ManualFace();
    }

    public void StartStagger()
    {
        facePlayer.ResetDestination();
        beam.SetActive(false);
        laserState = LaserState.OFF;
        laserTimer = 0;
        onCircle = false;
        navMeshAgent.acceleration = defaultAccel;
        navMeshAgent.speed = defaultSpeed;
        navMeshAgent.velocity = Vector3.zero;
        navMeshDestination = playerScript.transform;
        attackArc.HideAttackArc();
        StopAllCoroutines();
    }
}
