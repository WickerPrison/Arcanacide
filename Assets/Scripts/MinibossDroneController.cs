using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DroneState
{
    IDLE, FLYING, LASER, CIRCLE, CHARGE
}

public class MinibossDroneController : MonoBehaviour
{
    public int droneId;
    [SerializeField] GameObject plasmaBallPrefab;
    [SerializeField] Transform frontFirePoint;
    [SerializeField] Transform backFirePoint;
    [SerializeField] AnimationCurve toDestinationCurve;
    [SerializeField] AnimationCurve[] hoverPattern;
    Ellipse ellipse;
    EnemyScript enemyScript;
    EnemySound enemySound;
    MinibossAbilities abilities;
    MinibossEvents minibossEvents;
    DroneState droneState = DroneState.IDLE;
    Vector3 focusPoint;
    PlayerScript playerScript;
    FaceDirection faceDirection;
    float plasmaShotsToPosTime = 0.5f;
    float recallDroneTime = 0.5f;
    WaitForSeconds plasmaShotFireDelay = new WaitForSeconds(0.15f);
    int sign;
    Vector3 toPlayer;
    Vector3 perp;
    Vector3 targetPos;
    float randOffset;
    [SerializeField] float speed;
    [SerializeField] GameObject beam;
    //Vector3 initialBeamDirection;
    //Vector3 finalBeamDirection;
    float beamVertAngle = 90;
    LaserState laserState = LaserState.OFF;
    float laserTimer;
    [SerializeField] float[] pauseTime;
    [SerializeField] float sweepTime;
    float sweepHalfWidth = 100;
    int sweeps;
    float ellipseRads;
    [SerializeField] float plasmaCooldown;
    float plasmaTimer;
    [SerializeField] float chargeWindupTime;
    [SerializeField] float chargeTime;
    [SerializeField] int chargeDamage;
    [SerializeField] float chargePoiseDmage;
    float chargeTimer;
    bool chargeHitPlayer = false;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    [SerializeField] AnimationCurve chargeCurve;
    Collider chargeHitbox;
    WaitForSeconds chargePause = new WaitForSeconds(0.2f);
    Vector3 offset;
    public event EventHandler<(Vector3, Vector3)> onStartCharge;
    public event EventHandler onEndCharge;

    private void Awake()
    {
        minibossEvents = FindObjectOfType<MinibossEvents>();
    }

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        faceDirection = GetComponent<FaceDirection>();
        chargeHitbox = GetComponent<Collider>();
        chargeHitbox.enabled = false;
        offset = Vector3.up * 1.75f;
        sign = droneId == 0 ? 1 : -1;
        randOffset = UnityEngine.Random.Range(0f, 1f);
        beam.SetActive(false);
        if(minibossEvents != null)
        {
            enemyScript = minibossEvents.GetComponent<EnemyScript>();
            enemySound = enemyScript.GetComponentInChildren<EnemySound>();
            abilities = minibossEvents.GetComponent<MinibossAbilities>();
            ellipse = abilities.ellipse;
        }
    }

    void Update()
    {
        toPlayer = Vector3.Normalize(playerScript.transform.position - enemyScript.transform.position);
        perp = Vector3.Cross(Vector3.up, toPlayer).normalized;

        switch (droneState)
        {
            case DroneState.IDLE:
                focusPoint = playerScript.transform.position;
                faceDirection.FaceTowards(focusPoint);
                targetPos = HoverPosition();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (droneState)
        {
            case DroneState.IDLE:
                if(Vector3.Distance(targetPos, transform.position) > speed * Time.fixedDeltaTime)
                {
                    Vector3 direction = Vector3.Normalize(targetPos - transform.position);
                    transform.position += speed * Time.fixedDeltaTime * direction;
                }
                else
                {
                    transform.position = targetPos;
                }
                break;
            case DroneState.LASER:
                LaserSweep();
                break;
            case DroneState.CIRCLE:
                ellipseRads += Time.fixedDeltaTime / abilities.timeToCircle * 2 * Mathf.PI;
                transform.position = ellipse.GetPosition(ellipseRads) + Vector3.up * 1.75f;
                faceDirection.FaceTowards(playerScript.transform.position);
                if (plasmaTimer > 0)
                {
                    plasmaTimer -= Time.fixedDeltaTime;
                    if (plasmaTimer <= 0)
                    {
                        plasmaTimer = plasmaCooldown;
                        FirePlasmaShot();
                    }
                }
                break;
        }
    }

    public Vector3 HoverPosition()
    {
        Vector3 hoverPos = RelativePosition(-0.5f, 1.5f, 0);
        hoverPos += Vector3.up * (hoverPattern[droneId].Evaluate(Time.time * 0.3f + randOffset) * 0.2f + 1.5f);
        return hoverPos;
    }

    Vector3 RelativePosition(float toPlayerMag, float perpMag, float vert = 1.7f)
    {
        return enemyScript.transform.position + toPlayer * toPlayerMag + perp * perpMag * sign + Vector3.up * vert;
    }

    Vector3 FirePoint()
    {
        if (faceDirection.facingFront)
        {
            return frontFirePoint.transform.position;
        }
        else
        {
            return backFirePoint.transform.position;
        }
    }

    IEnumerator ToPosition(Vector3 startPos, Vector3 destination, float maxTime, Action callback, bool facePlayer = false)
    {
        float timer = maxTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float progress = toDestinationCurve.Evaluate(timer / maxTime);
            transform.position = Vector3.Lerp(destination, startPos, progress);
            if (facePlayer)
            {
                faceDirection.FaceTowards(playerScript.transform.position);
            }
            else
            {
                faceDirection.FaceTowards(destination);
            }
            yield return null;
        }
        callback();
    }

    private void MinibossEvents_onStartPlasmaShots(object sender, System.EventArgs e)
    {
        StartCoroutine(PlasmaShots());
    }

    public void FirePlasmaShots()
    {
        StartCoroutine(PlasmaShots()); 
    }

    IEnumerator PlasmaShots()
    {
        droneState = DroneState.FLYING;
        Vector3 startPos = transform.position;
        Vector3 destination = RelativePosition(2, 2);
        yield return StartCoroutine(ToPosition(startPos, destination, plasmaShotsToPosTime, () => { }, true));

        int count = 7;
        while(count > 0)
        {
            count--;
            FirePlasmaShot();
            yield return plasmaShotFireDelay;
        }

        yield return StartCoroutine(ToPosition(destination, startPos, plasmaShotsToPosTime, () => { }));
        droneState = DroneState.IDLE;
    }

    public void FirePlasmaShot()
    {
        HomingProjectile shot = Instantiate(plasmaBallPrefab).GetComponent<HomingProjectile>();
        shot.transform.position = FirePoint();
        shot.target = playerScript.transform;
        shot.enemyOfOrigin = enemyScript;
        shot.transform.LookAt(playerScript.transform);
    }

    private void MinibossEvents_onStartDroneLaser(object sender, System.EventArgs e)
    {
        droneState = DroneState.FLYING;
        Vector3 startPos = transform.position;
        Vector3 destination = RelativePosition(1, 5);
        StartCoroutine(ToPosition(startPos, destination, plasmaShotsToPosTime, StartLaser));
    }

    void SetBeamPosition(Vector3 direction)
    {
        Vector3 beamOrigin = transform.position + direction.normalized * 0.3f;
        beam.transform.position = beamOrigin + direction.normalized * beam.transform.localScale.y;
        beam.transform.LookAt(beamOrigin);
        beam.transform.localEulerAngles = new Vector3(
            beamVertAngle,
            beam.transform.localEulerAngles.y,
            beam.transform.localEulerAngles.z);
        faceDirection.FaceTowards(beam.transform.position);
    }

    public void StartLaser()
    {
        //initialBeamDirection = Utils.RotateDirection(toPlayer, -sweepHalfWidth * sign);
        //finalBeamDirection = Utils.RotateDirection(toPlayer, sweepHalfWidth * sign);
        beam.SetActive(true);
        //SetBeamPosition(initialBeamDirection.normalized);
        SetBeamPosition(Utils.RotateDirection(toPlayer, -sweepHalfWidth * sign));
        droneState = DroneState.LASER;
        laserState = LaserState.START;
        laserTimer = pauseTime[droneId];
        sweeps = 3;
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
                float angle = Mathf.Lerp(-sweepHalfWidth, sweepHalfWidth, t);
                SetBeamPosition(Utils.RotateDirection(toPlayer, angle * sign));
                if (laserTimer <= 0)
                {
                    laserTimer = pauseTime[droneId];
                    sweeps -= 1;
                    if (sweeps == 0) laserState = LaserState.END;
                    else laserState = LaserState.PAUSE;
                }
                break;
            case LaserState.PAUSE:
                laserTimer -= Time.fixedDeltaTime;
                if (laserTimer <= 0)
                {
                    laserTimer = sweepTime;
                    //Vector3 temp = initialBeamDirection;
                    //initialBeamDirection = finalBeamDirection;
                    //finalBeamDirection = temp;
                    sign *= -1;
                    laserState = LaserState.SWEEP;
                }
                break;
            case LaserState.END:
                laserTimer -= Time.fixedDeltaTime;
                if (laserTimer <= 0)
                {
                    beam.SetActive(false);
                    laserState = LaserState.OFF;
                }
                break;
        }
    }

    private void MinibossEvents_onStartDroneCharge(object sender, EventArgs e)
    {
        StartCoroutine(ChargePositioning());
    }

    public void StartCharge()
    {
        StartCoroutine(ChargePositioning());
    }

    IEnumerator ChargePositioning()
    {
        if(droneId == 1)
        {
            yield return new WaitForSeconds(2);
        }

        Vector3 destination = playerScript.transform.position - toPlayer * 3 + perp * 2 * sign + offset;

        yield return StartCoroutine(ToPosition(transform.position, destination, 0.5f, () =>
         {
             StartCoroutine(Charge());
         }));
    }

    IEnumerator Charge()
    {
        chargeHitPlayer = false;
        droneState = DroneState.CHARGE;
        Vector3 startPos = transform.position;
        Vector3 direction = Vector3.Normalize(playerScript.transform.position + offset - transform.position);
        Vector3 destination = transform.position - direction * 1.5f;
        chargeTimer = chargeWindupTime;
        float progress;
        while (chargeTimer > 0)
        {
            chargeTimer -= Time.fixedDeltaTime;
            progress = 1 - chargeTimer / chargeWindupTime;
            transform.position = Vector3.LerpUnclamped(startPos, destination, chargeCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }

        yield return chargePause;

        chargeTimer = chargeTime;
        chargeHitbox.enabled = true;
        startPos = transform.position;
        destination = transform.position + Vector3.Normalize(playerScript.transform.position + offset - transform.position) * 15f;
        onStartCharge?.Invoke(this, (startPos, destination));
        while (chargeTimer > 0)
        {
            chargeTimer -= Time.fixedDeltaTime;
            progress = 1 - chargeTimer / chargeTime;
            transform.position = Vector3.LerpUnclamped(startPos, destination, chargeCurve.Evaluate(progress));
            yield return waitForFixedUpdate;
        }
        onEndCharge?.Invoke(this, EventArgs.Empty);
        chargeHitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (chargeHitPlayer) return;
        if (other.CompareTag("Player"))
        {
            chargeHitPlayer = true;
            playerScript.HitPlayer(() =>
            {
                enemySound.SwordImpact();
                playerScript.LoseHealth(chargeDamage, EnemyAttackType.NONPARRIABLE, enemyScript);
                playerScript.LosePoise(chargePoiseDmage);
            }, () =>
            {
                playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE, enemyScript);
            });
        }
    }

    private void MinibossEvents_onRecallDrones(object sender, EventArgs e)
    {
        StartCoroutine(ToPosition(transform.position, HoverPosition(), recallDroneTime, () => { droneState = DroneState.IDLE; }));
    }

    private void MinibossEvents_onStartCircle(object sender, (float, float) vals)
    {
        float minibossStartRads = vals.Item1;
        float getToCircleTime = vals.Item2;
        ellipseRads = minibossStartRads + 0.66666f * Mathf.PI * (droneId + 1);
        Vector3 circlePos = ellipse.GetPosition(ellipseRads) + Vector3.up * 1.75f; 
        StartCoroutine(ToPosition(transform.position, circlePos, getToCircleTime, () => 
        {
            droneState = DroneState.CIRCLE;
            plasmaTimer = plasmaCooldown;
        }));
    }

    private void OnEnable()
    {
        minibossEvents.onStartPlasmaShots += MinibossEvents_onStartPlasmaShots;
        minibossEvents.onStartDroneLaser += MinibossEvents_onStartDroneLaser;
        minibossEvents.onRecallDrones += MinibossEvents_onRecallDrones;
        minibossEvents.onStartCircle += MinibossEvents_onStartCircle;
        minibossEvents.onStartDroneCharge += MinibossEvents_onStartDroneCharge;
    }

    private void OnDisable()
    {
        minibossEvents.onStartPlasmaShots -= MinibossEvents_onStartPlasmaShots;
        minibossEvents.onStartDroneLaser -= MinibossEvents_onStartDroneLaser;
        minibossEvents.onRecallDrones -= MinibossEvents_onRecallDrones;
        minibossEvents.onStartCircle -= MinibossEvents_onStartCircle;
        minibossEvents.onStartDroneCharge -= MinibossEvents_onStartDroneCharge;
    }
}
