using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectricAlly : MonoBehaviour, IHaveLightningRings
{
    [SerializeField] EventReference electricImpact;
    //[SerializeField] Color transparent;
    //[SerializeField] Color solid;
    [SerializeField] float radius;
    float poiseDamage = 20;
    [System.NonSerialized] public bool isShielded = false;
    SpriteRenderer shield;
    PlayerScript playerScript;
    EnemyEvents enemyEvents;
    EnemyScript enemyScript;
    [SerializeField] Color auditRed;
    Material lightningMat;


    public event EventHandler<float> onSetRadius;
    public event EventHandler<Transform> onSetTarget;
    public event EventHandler<bool> onShowRings;
    public event EventHandler<Color> onSetColor;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        shield = GetComponent<SpriteRenderer>();
        enemyScript = GetComponentInParent<EnemyScript>();
        onSetRadius?.Invoke(this, radius);
        onShowRings?.Invoke(this, false);
    }

    public void ShieldOnOff(bool onOrOff)
    {
        enemyScript.invincible = onOrOff;
        //shield.enabled = onOrOff;
        onShowRings?.Invoke(this, onOrOff);
        isShielded = onOrOff;
    }

    private void OnHitWhileInvincible(object sender, System.EventArgs e)
    {
        if (!isShielded) return;

        RuntimeManager.PlayOneShot(electricImpact);
        playerScript.LosePoise(poiseDamage);
        playerScript.StartStagger(1);
        StartCoroutine(Block());
    }

    IEnumerator Block()
    {
        //shield.color = solid;
        onSetColor?.Invoke(this, Color.white);
        shield.material.SetFloat("_EdgeDecay", 0);
        yield return new WaitForSeconds(0.3f);
        //shield.color = transparent;
        onSetColor?.Invoke(this, auditRed);
        shield.material.SetFloat("_EdgeDecay", 0.6f);
    }

    private void OnEnable()
    {
        enemyEvents.OnHitWhileInvincible += OnHitWhileInvincible;
    }


    private void OnDisable()
    {
        enemyEvents.OnHitWhileInvincible -= OnHitWhileInvincible;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.3f, radius);
    }
}
