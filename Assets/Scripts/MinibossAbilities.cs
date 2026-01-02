using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public enum MissilePattern
{
    FRONT, RADIAL
}

public enum CircleType
{
    SHOOT, LASER
}

public enum LaserState
{
    START, SWEEP, PAUSE, END, OFF
}

public enum MinibossSpecialState
{
    NONE, CIRCLE, LASER, JUMP
}

public class MinibossAbilities : MonoBehaviour
{
    [SerializeField] float normalStopDistance;
    float dashingStopDistance = 1.5f;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform frontMissileSpawnPoint;
    [SerializeField] Transform backMissileSpawnPoint;
    public Ellipse ellipse;
    FacePlayer facePlayer;
    NavMeshAgent navMeshAgent;
    EnemyController enemyController;
    EnemyScript enemyScript;
    EnemySound enemySound;
    PlayerScript playerScript;
    CameraFollow cameraScript;
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
    [SerializeField] float radsToComplete = 2;
    public float timeToCircle;
    float startingRads;
    float ellipseRads;
    [SerializeField] GameObject plasmaBallPrefab;
    [SerializeField] Transform frontShotOrigin;
    [SerializeField] Transform backShotOrigin;
    [SerializeField] float plasmaCooldown;
    float plasmaTimer;
    [SerializeField] LaserBeam beam;
    [SerializeField] Transform beamOrigin;
    [SerializeField] Transform dashTarget;
    Vector3 initialBeamDirection;
    Vector3 finalBeamDirection;
    float beamVertAngle = 90;
    [System.NonSerialized] public Transform navMeshDestination;
    AttackArcGenerator attackArc;
    MinibossEvents minibossEvents;
    [System.NonSerialized] public MissilePattern missilePattern;
    CircleType circleType;
    [SerializeField] float teslaTime;
    [SerializeField] float teslaDelay;
    [SerializeField] GameObject teslaHarpoonPrefab;
    [SerializeField] AnimationCurve descendCurve;
    [SerializeField] float descendTime;
    float descendTimer;
    HarpoonManager harpoonManager;
    [System.NonSerialized] public List<Transform> fleePoints = new List<Transform>();

    LaserState laserState = LaserState.OFF;
    float laserTimer;
    [SerializeField] float pauseTime;
    [SerializeField] float sweepTime;
    float sweepHalfWidth = 65;
    int sweeps;
    Vector3 away = new Vector3(100, 100, 100);

    [SerializeField] SpriteRenderer landingIndicator;
    float jumpSpeed = 5f;
    int jumpDamage = 30;
    float jumpPoiseDamage = 60f;
    [SerializeField] float landTime;
    MinibossSpecialState specialState = MinibossSpecialState.NONE;
    WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

    public SpinPoints spinPoints;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyScript = GetComponent<EnemyScript>();
        enemySound = GetComponentInChildren<EnemySound>();
        minibossEvents = GetComponent<MinibossEvents>();
        playerScript = enemyController.playerScript;
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        facePlayer = GetComponent<FacePlayer>();
        beam.gameObject.SetActive(false);
        beam.transform.parent = null;
        dashTarget.transform.parent = null;
        navMeshDestination = playerScript.transform;
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        harpoonManager = GetComponent<HarpoonManager>();
        if (landingIndicator != null)
        {
            landingIndicator.transform.SetParent(null);
            landingIndicator.transform.position = transform.position;
            landingIndicator.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if(enemyController.state == EnemyState.SPECIAL)
        {
            switch (specialState)
            {
                case MinibossSpecialState.CIRCLE:
                    CircleFixedUpdate();
                    break;
                case MinibossSpecialState.LASER:
                    LaserSweep();
                    break;
                case MinibossSpecialState.JUMP:
                    JumpFixedUpdate();
                    break;
            }
        }
    }

    void CircleFixedUpdate()
    {
        if (onCircle)
        {
            switch (circleType)
            {
                case CircleType.SHOOT:
                    FollowCircleShoot();
                    break;
                case CircleType.LASER:
                    FollowCircleLaser();
                    break;
            }
        }
        else
        {
            GetToTheCircle();
        }
    }

    void JumpFixedUpdate()
    {
        Vector3 direction = Vector3.Normalize(playerScript.transform.position - landingIndicator.transform.position);

        if (Vector3.Distance(playerScript.transform.position, landingIndicator.transform.position) > jumpSpeed * Time.fixedDeltaTime)
        {
            landingIndicator.transform.position += direction.normalized * jumpSpeed * Time.fixedDeltaTime;
        }
        else
        {
            landingIndicator.transform.position = playerScript.transform.position;
        }
    }

    public void MissileAttack(MissilePattern pattern)
    {
        missilePattern = pattern;
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("Missiles");
        enemyController.backAnimator.Play("Missiles");
    }
    
    public void SingleMissile(Vector3 target, float timeToHit)
    {
        ArcProjectile missile = Instantiate(missilePrefab).GetComponent<ArcProjectile>();
        missile.transform.position = enemyController.facingFront ? frontMissileSpawnPoint.position : backMissileSpawnPoint.position;
        missile.endPoint = target;
        missile.enemyOfOrigin = enemyScript;
        missile.timeToHit = timeToHit;
    }

    public void FireMissiles(MissilePattern pattern)
    {
        switch (pattern)
        {
            case MissilePattern.FRONT:
                StartCoroutine(FrontMissileCoroutine());
                break;
            case MissilePattern.RADIAL:
                StartCoroutine(RadialMissileCoroutine());
                break;
        }
    }

    IEnumerator FrontMissileCoroutine()
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

    IEnumerator RadialMissileCoroutine()
    {
        int[] layerCount = { 5, 8, 12 };
        Vector3 playerDirection = Vector3.Normalize(playerScript.transform.position - transform.position);
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < layerCount[i]; j++)
            {
                float angle = (float)j / layerCount[i] * 360;
                Vector3 direction = Utils.RotateDirection(playerDirection, angle);
                Vector3 target = transform.position + direction.normalized * range * (i + 1);
                target += new Vector3(
                    UnityEngine.Random.Range(-1f, 1f),
                    0,
                    UnityEngine.Random.Range(-1f, 1f));
                SingleMissile(target, 0.75f + UnityEngine.Random.Range(0, 0.5f));
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

    public void DashAway(Action nextAction, Vector3 destination = new Vector3())
    {
        enemyController.state = EnemyState.ATTACKING;
        Vector3 direction = transform.position - playerScript.transform.position;
        NavMeshHit hit;
        bool foundDest = false;
        int sign = UnityEngine.Random.Range(0, 2) * 2 - 1;
        if(destination != Vector3.zero)
        {
            dashTarget.position = destination;
            foundDest = true;
        }
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
        navMeshAgent.stoppingDistance = 0;
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
            navMeshAgent.stoppingDistance = normalStopDistance;
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
        navMeshAgent.stoppingDistance = dashingStopDistance;
        StartCoroutine(Dashing(playerScript.transform, () =>
        {
            enemyController.frontAnimator.Play(endAnimation);
            enemyController.backAnimator.Play(endAnimation);
            navMeshAgent.acceleration = defaultAccel;
            navMeshAgent.speed = defaultSpeed;
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.stoppingDistance = normalStopDistance;
        }));
    }

    public void DashAttackHit(int smearSpeed)
    {
        enemyController.AttackHit(0);
        enemyController.smearScript.AlternateSmears(smearSpeed, 0);
    }

    IEnumerator Dashing(Transform target, Action callback)
    {
        while (Vector3.Distance(target.position, transform.position) > 2)
        {
            yield return null;
        }

        callback();
    }

    public void Circle(CircleType type)
    {
        circleType = type;
        enemyController.frontAnimator.Play("StartDash");
        enemyController.backAnimator.Play("StartDash");
        navMeshAgent.enabled = false;
        onCircle = false;
        (circleStart, startingRads) = ellipse.GetStartingPosition(transform.position);
        float distance = Vector3.Distance(transform.position, circleStart);
        minibossEvents.StartCircle(startingRads, distance / getToCircleSpeed); 
        ellipseRads = startingRads;
        enemyController.state = EnemyState.SPECIAL;
        specialState = MinibossSpecialState.CIRCLE;
    }

    void GetToTheCircle()
    {
        Vector3 direction = (circleStart - transform.position).normalized;
        facePlayer.SetDestination(direction);
        transform.Translate(Time.fixedDeltaTime * getToCircleSpeed * (circleStart - transform.position).normalized);
        if (Vector3.Distance(transform.position, circleStart) < Time.deltaTime * getToCircleSpeed)
        {
            onCircle = true;
            facePlayer.ResetDestination();
            switch (circleType)
            {
                case CircleType.SHOOT:
                    plasmaTimer = plasmaCooldown;
                    enemyController.frontAnimator.Play("ShootDash");
                    enemyController.backAnimator.Play("ShootDash");
                    break;
                case CircleType.LASER:
                    enemyController.frontAnimator.Play("ChestLaser");
                    enemyController.backAnimator.Play("ChestLaser");
                    beam.gameObject.SetActive(true);
                    SetBeamPosition(-transform.position);
                    break;

            }
        }
    }

    public void FollowCircleShoot()
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

        if(ellipseRads >= startingRads + radsToComplete * MathF.PI)
        {
            navMeshAgent.enabled = true;
            enemyController.state = EnemyState.IDLE;
            specialState = MinibossSpecialState.NONE;
            enemyController.frontAnimator.Play("ShootDashEnd");        
            enemyController.backAnimator.Play("ShootDashEnd");
            minibossEvents.RecallDrones();
        }
    }

    public void FollowCircleLaser()
    {
        facePlayer.ManualFace();
        ellipseRads += Time.fixedDeltaTime / timeToCircle * 2 * Mathf.PI;
        transform.position = ellipse.GetPosition(ellipseRads);

        SetBeamPosition(-transform.position);

        if (ellipseRads >= startingRads + 2 * MathF.PI)
        {
            navMeshAgent.enabled = true;
            enemyController.state = EnemyState.IDLE;
            specialState = MinibossSpecialState.NONE;
            enemyController.frontAnimator.Play("ChestLaserEnd");
            enemyController.backAnimator.Play("ChestLaserEnd");
            minibossEvents.ThrustersOff();
            beam.gameObject.SetActive(false);
        }
    }

    public void PlasmaShots()
    {
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("PlasmaShots");
        enemyController.backAnimator.Play("PlasmaShots");
        minibossEvents.StartPlasmaShots();
    }

    public void FireMultiplePlasmaShots(int count, float delay)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        StartCoroutine(MultipleShots(count, waitForSeconds));
    }

    IEnumerator MultipleShots(int count, WaitForSeconds delay)
    {
        while(count > 0)
        {
            FirePlasmaShot();
            count -= 1;
            yield return delay;
        }
    }

    public HomingProjectile FirePlasmaShot()
    {
        HomingProjectile shot = Instantiate(plasmaBallPrefab).GetComponent<HomingProjectile>();
        shot.transform.position = enemyController.facingFront ? frontShotOrigin.position : backShotOrigin.position;
        shot.target = playerScript.transform;
        shot.enemyOfOrigin = enemyScript;
        shot.transform.LookAt(playerScript.transform);
        return shot;
    }

    public void ChestLaser(int sweepsCount)
    {
        if(Mathf.Abs(transform.position.z - playerScript.transform.position.z) > 6
           || Mathf.Abs(transform.position.x - playerScript.transform.position.x) > 8)
        {
            MissileAttack(MissilePattern.FRONT);
            return;
        }
        enemyController.state = EnemyState.SPECIAL;
        specialState = MinibossSpecialState.LASER;
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
        beam.gameObject.SetActive(true);
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
                    beam.gameObject.SetActive(false);
                    enemyController.frontAnimator.Play("ChestLaserEnd");
                    enemyController.backAnimator.Play("ChestLaserEnd");
                    specialState = MinibossSpecialState.NONE;
                    facePlayer.ResetDestination();
                    laserState = LaserState.OFF;
                }
                break;
        }
    }

    public void StartTeslaHarpoon()
    {
        enemyController.state = EnemyState.ATTACKING;
        minibossEvents.TeslaHarpoons();
        enemyController.frontAnimator.Play("HarpoonTakeoff");
        enemyController.backAnimator.Play("Takeoff");
        enemyController.attackTime = teslaTime + 5;
    }

    public void HarpoonTakeoff()
    {
        enemyScript.invincible = true;
        StartCoroutine(TeslaHarpoons());
    }

    IEnumerator TeslaHarpoons()
    {
        yield return StartCoroutine(FlyUp());

        float rainTimer = teslaTime;
        float delayTimer = teslaDelay;
        while(rainTimer > 0)
        {
            rainTimer -= Time.deltaTime;
            delayTimer -= Time.deltaTime;

            if(delayTimer <= 0)
            {
                SpawnTeslaHarpoon();
                delayTimer = teslaDelay;
            }

            yield return null;
        }

        enemyScript.invincible = false;
        enemyController.frontAnimator.Play("Descend");
        enemyController.backAnimator.Play("Descend"); 
        descendTimer = 0;
        NavMeshHit hit;
        Vector3 randVect = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, UnityEngine.Random.Range(-5f, 5f));
        NavMesh.SamplePosition(playerScript.transform.position + randVect, out hit, 5f, NavMesh.AllAreas);
        transform.position = new Vector3(hit.position.x, 15, hit.position.z);
        while(descendTimer <= descendTime)
        {
            descendTimer += Time.deltaTime;
            float yPos = descendCurve.Evaluate(descendTimer / descendTime) * 15;
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            yield return null;
        }
        enemyController.attackTime = enemyController.attackMaxTime;
    }

    IEnumerator FlyUp()
    {
        while (transform.position.y < 15)
        {
            transform.position += new Vector3(0, 25 * Time.deltaTime, 0);
            yield return null;
        }
    }

    void SpawnTeslaHarpoon()
    {
        TeslaHarpoonProjectile teslaHarpoon = Instantiate(teslaHarpoonPrefab).GetComponentInChildren<TeslaHarpoonProjectile>();
        teslaHarpoon.SetupHarpoon(enemyScript);
    }

    public void DroneLasers()
    {
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("DroneTakeoff");
        enemyController.backAnimator.Play("Takeoff");
        minibossEvents.StartDroneLaser();
    }

    public void DroneTakeoff()
    {
        StartCoroutine(DroneLaserFlying());
    }

    IEnumerator DroneLaserFlying()
    {
        landingIndicator.transform.position = transform.position;
        yield return StartCoroutine(FlyUp());
        enemyController.state = EnemyState.SPECIAL;
        specialState = MinibossSpecialState.JUMP;
        landingIndicator.enabled = true;
        yield return new WaitForSeconds(8f);
        specialState = MinibossSpecialState.NONE;
        enemyController.frontAnimator.Play("Falling");
        enemyController.backAnimator.Play("Falling");

        transform.position = landingIndicator.transform.position + Vector3.up * 15;
        float landTimer = landTime;
        Vector3 startPos = transform.position;
        while(landTimer > 0)
        {
            landTimer -= Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(landingIndicator.transform.position, startPos, landTimer / landTime);
            yield return fixedUpdate;
        }
        enemyController.frontAnimator.Play("Land");
        enemyController.backAnimator.Play("Land");
    }

    public void LandImpact()
    {
        landingIndicator.enabled = false;
        enemySound.OtherSounds(0, 1f);
        StartCoroutine(cameraScript.ScreenShake(.1f, .3f));
        minibossEvents.TriggerVfx("land");
        minibossEvents.ThrustersOff();
        if(enemyController.playerDistance < 3.5f)
        {
            playerScript.HitPlayer(() =>
            {
                enemySound.SwordImpact();
                playerScript.LoseHealth(jumpDamage, EnemyAttackType.MELEE, enemyScript);
                playerScript.LosePoise(jumpPoiseDamage);
            }, () =>
            {
                playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
            });
        }
    }

    public void DroneCharge()
    {
        minibossEvents.StartDroneCharge();
        enemyController.attackTime += 5f;
    }

    public Vector3 FleePointDestination()
    {
        Vector3 destination = fleePoints[0].position;
        float distance = Vector3.Distance(transform.position, fleePoints[0].position);
        for (int i = 1; i < fleePoints.Count; i++)
        {
            float newDist = Vector3.Distance(transform.position, fleePoints[i].position);
            if (newDist > distance)
            {
                distance = newDist;
                destination = fleePoints[i].position;
            }
        }
        return destination;
    }

    void SetBeamPosition(Vector3 direction)
    {
        beam.transform.position = beamOrigin.position + direction.normalized * (beam.transform.localScale.y + 0.2f);
        if(beam.transform.position.z > beamOrigin.position.z)
        {
            beam.SetSortingOrder(-1);
        }
        else
        {
            beam.SetSortingOrder(0);
        }
        beam.transform.LookAt(beamOrigin.position);
        beam.transform.localEulerAngles = new Vector3(
            beamVertAngle,
            beam.transform.localEulerAngles.y,
            beam.transform.localEulerAngles.z);
        facePlayer.SetDestination(beam.transform.position);
        facePlayer.ManualFace();
    }

    public void StartSpin()
    {
        minibossEvents.TeslaHarpoons();
    }

    public void StartStagger()
    {
        facePlayer.ResetDestination();
        beam.gameObject.SetActive(false);
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

    public bool AttackArcIsHidden()
    {
        return attackArc.AttackArcIsHidden();
    }
}
