using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyVFX : MonoBehaviour
{
    [SerializeField] Transform frontPoint;
    [SerializeField] Transform backPoint;
    [SerializeField] bool testingOn = false;
    [SerializeField] bool testingFaceFront;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem magicImpact;
    [SerializeField] ParticleSystem impactVFX;
    [SerializeField] ParticleSystem dot;
    [SerializeField] ParticleSystem hitVFX;
    EnemyEvents enemyEvents;
    EnemyController enemyController;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    private void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void Update()
    {
        if (enemyController.facingFront)
        {
            transform.position = frontPoint.position;
            transform.rotation = frontPoint.rotation;
        }
        else
        {
            transform.position = backPoint.position;
            transform.rotation = backPoint.rotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!testingOn) return;

        if (testingFaceFront)
        {
            transform.position = frontPoint.position;
            transform.rotation = frontPoint.rotation;
        }
        else
        {
            transform.position = backPoint.position;
            transform.rotation = backPoint.rotation;
        }
    }

    private void OnTakeDamage(object sender, System.EventArgs e)
    {
        if (!hitVFX.isEmitting)
        {
            hitVFX.Play();
        }
    }

    private void OnAttackImpact(object sender, System.EventArgs e)
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary._spellsword) && playerData.mana > emblemLibrary.spellswordManaCost)
        {
            magicImpact.Play();
        }
        else
        {
            impactVFX.Play();
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
