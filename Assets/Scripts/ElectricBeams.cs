using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ElectricBeams : MonoBehaviour
{
    private enum BeamsState
    {
        OFF, INDICATOR, BOLTS
    }

    Bolts[] bolts;
    List<LightningBolt> indicatorLines  = new List<LightningBolt>();
    Vector3 away = Vector3.one * 100;
    AnimationCurve initialCurve;
    [SerializeField] AnimationCurve indicatorCurve;
    List<Vector3> endPoints = new List<Vector3>();
    LayerMask layerMask;
    float angle = 0;
    [System.NonSerialized] public float rotationSpeed = 50;
    BeamsState state = BeamsState.OFF;
    bool hitPlayer = false;
    bool hitPlayerDelay = false;
    WaitForSeconds hitDelay = new WaitForSeconds(0.2f);
    PlayerScript playerScript;
    EnemyScript enemyScript;
    [SerializeField] EventReference electricDamage;
    [System.NonSerialized] public float friendshipPower;
    public int beamDamage = 30;
    public float beamPoiseDamage = 30;


    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        enemyScript = GetComponentInParent<EnemyScript>();
        layerMask = LayerMask.GetMask("Default", "Player");
        bolts = GetComponentsInChildren<Bolts>();
        foreach(Bolts bolt in bolts)
        {
            bolt.SetPositions(away, away);
            indicatorLines.Add(bolt.GetComponentInChildren<LightningBolt>());
        }
        initialCurve = indicatorLines[0].lineRenderer.widthCurve;
    }

    private void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        if (angle > 360) angle -= 360;
        
        if(state == BeamsState.INDICATOR)
        {
            UpdateIndicators();
        }
        else if(state == BeamsState.BOLTS)
        {
            UpdateBolts();
        }

    }

    public void ActivateLightning()
    {
        foreach(LightningBolt indicator in indicatorLines)
        {
            indicator.noiseAmp = 0.5f;
            indicator.lineRenderer.widthCurve = initialCurve;
            indicator.frameDivider = 3;
        }
        state = BeamsState.BOLTS;
    }

    void UpdateBolts()
    {
        PerformRaycasts(angle);

        for(int i = 0; i < bolts.Length; i++)
        {
            bolts[i].SetPositions(transform.position, endPoints[i]);
        }
    }

    public void ActivateIndicators()
    {
        for(int i = 0; i < indicatorLines.Count; i++)
        {
            indicatorLines[i].noiseAmp = 0;
            indicatorLines[i].lineRenderer.widthCurve = indicatorCurve;
            indicatorLines[i].lineRenderer.widthMultiplier = 0.1f;
            indicatorLines[i].frameDivider = 0;
        }

        UpdateIndicators();
        state = BeamsState.INDICATOR;
    }

    void UpdateIndicators()
    {
        PerformRaycasts(angle);

        for (int i = 0; i < indicatorLines.Count; i++)
        {
            indicatorLines[i].SetPositions(transform.position, endPoints[i]);
        }
    }

    public void BeamsOff()
    {
        state = BeamsState.OFF;
        foreach(Bolts bolt in bolts)
        {
            bolt.SetPositions(away, away);
        }
    }

    void PerformRaycasts(float startAngle)
    {
        endPoints.Clear();
        float angle = startAngle;
        float angleDifference = 360 / indicatorLines.Count;

        for(int i = 0; i < indicatorLines.Count;i++)
        {
            Vector3 direction = Utils.RotateDirection(Vector3.right, startAngle + angleDifference * i);
            RaycastHit hit;
            Physics.Raycast(transform.position, direction.normalized, out hit, 50, layerMask, QueryTriggerInteraction.Ignore);
            endPoints.Add(hit.point);
            if (hit.collider.CompareTag("Player")) hitPlayer = true;
        }

        if (hitPlayer)
        {
            if (hitPlayerDelay || state != BeamsState.BOLTS)
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
        RuntimeManager.PlayOneShot(electricDamage);
        playerScript.LoseHealth(Mathf.RoundToInt(friendshipPower * beamDamage), EnemyAttackType.NONPARRIABLE, enemyScript);
        playerScript.LosePoise(beamPoiseDamage);
        playerScript.StartStagger(0.5f);
    }

    IEnumerator HitPlayerDelay()
    {
        hitPlayerDelay = true;
        yield return hitDelay;
        hitPlayerDelay = false;
    }
}
