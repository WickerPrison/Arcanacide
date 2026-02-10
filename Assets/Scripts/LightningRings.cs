using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

public enum LightningRingsState
{
    DISABLED, FOLLOW, PAUSE, CLOSING
}

public class LightningRings : MonoBehaviour, IHaveLightningRings
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
    [SerializeField] Material lightningMat;
    EnemyEvents enemyEvents;
    EnemySound enemySound;
    [SerializeField] EventReference lightningSound;
    [SerializeField] EventReference playerImpactSFX;
    [SerializeField] float impactSFXvolume;
    [System.NonSerialized] public LightningRingsState state;

    public event EventHandler<float> onSetRadius;
    public event EventHandler<Transform> onSetTarget;
    public event EventHandler<bool> onShowRings;
    public event EventHandler<Color> onSetColor;

    private void Awake()
    {
        enemyOfOrigin = GetComponentInParent<EnemyScript>();
        electricSwordsman = enemyOfOrigin.GetComponent<ElectricSwordsmanController>();
        enemyEvents = enemyOfOrigin.GetComponent<EnemyEvents>();
    }

    private void Start()
    {
        enemySound = enemyOfOrigin.GetComponentInChildren<EnemySound>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        target = playerScript.transform;
        onSetTarget?.Invoke(this, target);
        transform.SetParent(null);

        DisableRings();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case LightningRingsState.DISABLED:
                return;
            case LightningRingsState.FOLLOW:
                transform.position = target.position;
                break;
            case LightningRingsState.CLOSING:
                closingTimer -= Time.fixedDeltaTime;
                radius = Mathf.Lerp(0, maxRadius, closingTimer / closeMaxTime);
                onSetRadius?.Invoke(this, radius);
                if (closingTimer <= 0)
                {
                    DisableRings();
                }
                break;
        }

        if (canHitPlayer)
        {
            float dist = Vector3.Distance(transform.position, playerScript.transform.position);
            if(dist > radius - halfWidth && dist < radius + halfWidth)
            {
                if(playerScript.gameObject.layer == 3)
                {
                    playerScript.LoseHealth(electricSwordsman.spellAttackDamage, EnemyAttackType.PROJECTILE, enemyOfOrigin);
                    playerScript.LosePoise(electricSwordsman.spellAttackPoiseDamage);
                    FmodUtils.PlayOneShot(playerImpactSFX, impactSFXvolume, transform.position);
                    StartCoroutine(HitDelay());
                }
                else if (playerScript.gameObject.layer == 8)
                {
                    playerScript.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin, gameObject);
                }
            }
        }
    }

    public void StartRings()
    {
        state = LightningRingsState.FOLLOW;
        enemySound.Play(lightningSound, 0.6f);
        radius = maxRadius;
        onSetRadius?.Invoke(this, radius);
        onShowRings?.Invoke(this, true);
    }

    IEnumerator HitDelay()
    {
        canHitPlayer = false;
        yield return hitDelay;
        canHitPlayer = true;
    }

    public void CloseRings()
    {
        StartCoroutine(StartClosing());
    }

    IEnumerator StartClosing()
    {
        state = LightningRingsState.PAUSE;

        yield return new WaitForSeconds(0.3f);
        state = LightningRingsState.CLOSING;
        closingTimer = closeMaxTime;
    }

    public void DisableRings()
    {
        state = LightningRingsState.DISABLED;
        onShowRings?.Invoke(this, false);
        enemySound.Stop();
    }

    private void OnEnable()
    {
        enemyEvents.OnStartDying += EnemyEvents_OnStartDying;
        enemyEvents.OnStagger += EnemyEvents_OnStagger;
    }

    private void OnDisable()
    {
        enemyEvents.OnStartDying -= EnemyEvents_OnStartDying;
        enemyEvents.OnStagger -= EnemyEvents_OnStagger;
    }

    private void EnemyEvents_OnStagger(object sender, EventArgs e)
    {
        DisableRings();
    }

    private void EnemyEvents_OnStartDying(object sender, EventArgs e)
    {
        DisableRings();
    }
}
