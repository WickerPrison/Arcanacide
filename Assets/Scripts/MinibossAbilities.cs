using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MinibossAbilities : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform missileSpawnPoint;
    [SerializeField] Ellipse ellipse;
    FacePlayer facePlayer;
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
    [SerializeField] MeshRenderer beam;
    [SerializeField] Transform beamOrigin;
    float beamAngle = 45;
    Vector3 initialBeamDirection;
    bool beamFiring = false;
    float beamVertAngle = 87;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyScript = GetComponent<EnemyScript>();
        playerScript = enemyController.playerScript;
        navMeshAgent = GetComponent<NavMeshAgent>();
        facePlayer = GetComponent<FacePlayer>();
        beam.enabled = false;
        beam.transform.parent = null;
    }

    private void Update()
    {
        if (!beamFiring) return;
        beamAngle += Time.deltaTime * 15;
        Vector3 beamDirection = Utils.RotateDirection(initialBeamDirection, beamAngle);
        beam.transform.position = beamOrigin.position + beamDirection * beam.transform.localScale.y;
        beam.transform.LookAt(beamOrigin.position);
        beam.transform.localEulerAngles = new Vector3(
                beamVertAngle,
                beam.transform.localEulerAngles.y,
                beam.transform.localEulerAngles.z);
        facePlayer.SetDestination(beam.transform.position);
        facePlayer.ManualFace();
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

    public void ChestLaser()
    {
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("ChestLaserStart");
        enemyController.backAnimator.Play("ChestLaserStart");
        Vector3 playerDirection = Vector3.Normalize(playerScript.transform.position - transform.position);
        Vector3 startDirection = Utils.RotateDirection(playerDirection, 45);
        initialBeamDirection = Utils.RotateDirection(facePlayer.faceDirection.normalized, beamAngle);
        facePlayer.SetDestination(startDirection);
        facePlayer.ManualFace();
    }

    public void StartLaser()
    {
        beam.enabled = true;
        beamFiring = true;
        beamAngle = 0;
        initialBeamDirection = Utils.RotateDirection(facePlayer.faceDirection.normalized, -45);
    }
}
