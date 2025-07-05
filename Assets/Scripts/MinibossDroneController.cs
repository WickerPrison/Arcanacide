using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DroneState
{
    IDLE, FLYING
}

public class MinibossDroneController : MonoBehaviour
{
    public int droneId;
    [SerializeField] GameObject plasmaBallPrefab;
    [SerializeField] Transform frontFirePoint;
    [SerializeField] Transform backFirePoint;
    [SerializeField] AnimationCurve toDestinationCurve;
    [SerializeField] AnimationCurve[] hoverPattern;
    EnemyScript enemyScript;
    MinibossEvents minibossEvents;
    DroneState droneState = DroneState.IDLE;
    Vector3 focusPoint;
    PlayerScript playerScript;
    FaceDirection faceDirection;
    float plasmaShotsToPosTime = 0.5f;
    WaitForSeconds plasmaShotFireDelay = new WaitForSeconds(0.15f);
    int sign;
    Vector3 toPlayer;
    Vector3 perp;
    float randOffset;

    private void Awake()
    {
        minibossEvents = FindObjectOfType<MinibossEvents>();
    }

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        faceDirection = GetComponent<FaceDirection>();
        sign = droneId == 0 ? 1 : -1;
        randOffset = Random.Range(0f, 1f);
        if(minibossEvents != null)
        {
            enemyScript = minibossEvents.GetComponent<EnemyScript>();
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
                HoverPosition();
                break;
        }
    }

    void HoverPosition()
    {
        Vector3 hoverPos = enemyScript.transform.position + toPlayer * -0.5f + perp * 1.5f * sign;
        hoverPos += Vector3.up * (hoverPattern[droneId].Evaluate(Time.time * 0.3f + randOffset) * 0.2f + 1.5f);
        transform.position = hoverPos;
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
        float timer = plasmaShotsToPosTime;
        Vector3 startPos = transform.position;
        Vector3 destination = enemyScript.transform.position + toPlayer * 2 + perp * 2 * sign + Vector3.up * 1.7f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float progress = toDestinationCurve.Evaluate(timer / plasmaShotsToPosTime);
            transform.position = Vector3.Lerp(destination, startPos, progress);
            yield return null;
        }

        int count = 7;
        while(count > 0)
        {
            count--;
            FirePlasmaShot();
            yield return plasmaShotFireDelay;
        }

        timer = plasmaShotsToPosTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float progress = toDestinationCurve.Evaluate(timer / plasmaShotsToPosTime);
            transform.position = Vector3.Lerp(startPos, destination, progress);
            yield return null;
        }
        droneState = DroneState.IDLE;
    }

    public void FirePlasmaShot()
    {
        HomingProjectile shot = Instantiate(plasmaBallPrefab).GetComponent<HomingProjectile>();
        if (faceDirection.facingFront)
        {
            shot.transform.position = frontFirePoint.transform.position;
        }
        else
        {
            shot.transform.position = backFirePoint.transform.position;
        }
        shot.target = playerScript.transform;
        shot.enemyOfOrigin = enemyScript;
        shot.transform.LookAt(playerScript.transform);
    }

    private void OnEnable()
    {
        if (minibossEvents == null) return;
        minibossEvents.onStartPlasmaShots += MinibossEvents_onStartPlasmaShots;
    }

    private void OnDisable()
    {
        if (minibossEvents == null) return;
        minibossEvents.onStartPlasmaShots -= MinibossEvents_onStartPlasmaShots; 
    }
}
