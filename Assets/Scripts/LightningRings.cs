using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System;

public class LightningRings : MonoBehaviour
{
    public float maxRadius;
    float radius;
    [SerializeField] float halfWidth;
    public Transform target;
    bool canHitPlayer = true;
    PlayerScript playerScript;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;
    [System.NonSerialized] public ElectricSwordsmanController electricSwordsman;
    WaitForSeconds hitDelay = new WaitForSeconds(0.4f);
    float closeMaxTime = 0.5f;
    float closingTimer;
    bool closing;
    [SerializeField] Material lightningMat;
    EnemyEvents enemyEvents;

    public event EventHandler<float> onSetRadius;
    public event EventHandler<Transform> onSetTarget;

    private void OnValidate()
    {
        onSetRadius?.Invoke(this, radius);
        onSetTarget?.Invoke(this, target);
    }

    //private void OnDrawGizmos()
    //{
    //    Handles.DrawWireDisc(target.position, Vector3.up, radius + halfWidth);
    //    Handles.DrawWireDisc(target.position, Vector3.up, radius - halfWidth);
    //}

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        radius = maxRadius;
        onSetRadius?.Invoke(this, radius);
        onSetTarget?.Invoke(this, target);
    }

    private void FixedUpdate()
    {
        transform.position = target.position;

        if (canHitPlayer)
        {
            float dist = Vector3.Distance(target.position, playerScript.transform.position);
            if(dist > radius - halfWidth && dist < radius + halfWidth)
            {
                if(playerScript.gameObject.layer == 3)
                {
                    playerScript.LoseHealth(15, EnemyAttackType.PROJECTILE, enemyOfOrigin);
                    playerScript.LosePoise(100);
                    StartCoroutine(HitDelay());
                }
                else if (playerScript.gameObject.layer == 8)
                {
                    playerScript.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin, gameObject);
                }
            }
        }

        if (closing)
        {
            closingTimer -= Time.fixedDeltaTime;
            radius = Mathf.Lerp(0, maxRadius, closingTimer / closeMaxTime);
            onSetRadius?.Invoke(this, radius);
            if(closingTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator HitDelay()
    {
        canHitPlayer = false;
        yield return hitDelay;
        canHitPlayer = true;
    }

    IEnumerator StartClosing()
    {
        //Color originalColor = lightningMat.color;
        //lightningMat.color = Color.white;
        //lightningMat.SetColor("_EmissionColor", Color.white);

        target = transform;
        onSetTarget?.Invoke(this, target);

        yield return new WaitForSeconds(0.3f);
        //lightningMat.color = originalColor;
        //lightningMat.SetColor("_EmissionColor", originalColor);
        closing = true;
        closingTimer = closeMaxTime;
    }

    void InterruptRing()
    {
        Destroy(gameObject);
    }

    public void SetupEvents()
    {
        electricSwordsman.onCloseRing += ElectricSwordsman_onCloseRing;
        enemyEvents = electricSwordsman.GetComponent<EnemyEvents>();
        enemyEvents.OnStartDying += EnemyEvents_OnStartDying;
        enemyEvents.OnStagger += EnemyEvents_OnStagger;
    }

    private void OnDisable()
    {
        electricSwordsman.onCloseRing -= ElectricSwordsman_onCloseRing;
        enemyEvents.OnStartDying -= EnemyEvents_OnStartDying;
        enemyEvents.OnStagger -= EnemyEvents_OnStagger;
    }

    private void ElectricSwordsman_onCloseRing(object sender, EventArgs e)
    {
        StartCoroutine(StartClosing());
    }

    private void EnemyEvents_OnStagger(object sender, EventArgs e)
    {
        InterruptRing();
    }

    private void EnemyEvents_OnStartDying(object sender, EventArgs e)
    {
        InterruptRing();
    }
}
