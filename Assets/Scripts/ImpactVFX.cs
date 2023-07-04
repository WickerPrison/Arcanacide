using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFX : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem magicImpact;
    [SerializeField] ParticleSystem dot;
    [SerializeField] ParticleSystem hitVFX;
    EnemyEvents enemyEvents;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    private void OnTakeDamage(object sender, System.EventArgs e)
    {
        hitVFX.Play();
    }

    private void OnAttackImpact(object sender, System.EventArgs e)
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary._spellsword) && playerData.mana > emblemLibrary.spellswordManaCost)
        {
            magicImpact.Play();
        }
    }

    private void OnStartDOT(object sender, System.EventArgs e)
    {
        dot.Play();
    }

    private void OnStopDOT(object sender, System.EventArgs e)
    {
        dot.Stop();
    }

    private void OnEnable()
    {
        enemyEvents.OnTakeDamage += OnTakeDamage;
        enemyEvents.OnAttackImpact += OnAttackImpact;
        enemyEvents.OnStartDOT += OnStartDOT;
        enemyEvents.OnStopDOT += OnStopDOT;
    }

    private void OnDisable()
    {
        enemyEvents.OnTakeDamage -= OnTakeDamage;
        enemyEvents.OnAttackImpact -= OnAttackImpact;
        enemyEvents.OnStartDOT -= OnStartDOT;
        enemyEvents.OnStopDOT -= OnStopDOT;
    }
}
