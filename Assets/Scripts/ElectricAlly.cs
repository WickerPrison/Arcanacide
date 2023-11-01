using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAlly : MonoBehaviour
{
    [SerializeField] EventReference electricImpact;
    [SerializeField] Color transparent;
    [SerializeField] Color solid;
    float poiseDamage = 20;
    [System.NonSerialized] public bool isShielded = false;
    SpriteRenderer shield;
    PlayerScript playerScript;
    EnemyEvents enemyEvents;
    EnemyScript enemyScript;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        shield = GetComponent<SpriteRenderer>();
        enemyScript = GetComponentInParent<EnemyScript>();
    }

    public void ShieldOnOff(bool onOrOff)
    {
        enemyScript.invincible = onOrOff;
        shield.enabled = onOrOff;
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
        shield.color = solid;
        shield.material.SetFloat("_EdgeDecay", 0);
        yield return new WaitForSeconds(0.3f);
        shield.color = transparent;
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

}
