using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossController : EnemyController
{
    [SerializeField] GameObject fireBlastFront;
    [SerializeField] GameObject fireBlastBack;
    public int fireBlastDamage = 30;

    Collider fireBlastCollider;
    ParticleSystem fireBlastFrontParticles;
    ParticleSystem fireBlastBackParticles;
    float fireBlastRange = 3f;
    float fireBlastDuration = 0.5f;
    float fireBlastTime;

    public override void Start()
    {
        base.Start();
        fireBlastCollider = fireBlastFront.GetComponent<Collider>();
        fireBlastFrontParticles = fireBlastFront.GetComponent<ParticleSystem>();
        fireBlastBackParticles = fireBlastBack.GetComponent<ParticleSystem>();
    }

    public override void EnemyAI()
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) <= detectRange)
        {
            hasSeenPlayer = true;
        }
        if (hasSeenPlayer)
        {
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if (Vector3.Distance(transform.position, playerController.transform.position) < spellRange)
            {
                if (attackTime <= 0)
                {
                    if(Vector3.Distance(transform.position, playerController.transform.position) < fireBlastRange)
                    {
                        frontAnimator.SetTrigger("FireBlast");
                    }
                    else
                    {
                        frontAnimator.SetTrigger("SpellAttack");
                    }
                    attackTime = attackMaxTime;
                }
            }

        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }

        if(fireBlastTime > 0)
        {
            fireBlastTime -= Time.deltaTime;
            if(fireBlastTime <= 0)
            {
                EndFireBlast();
            }
        }
    }

    public void StartFireBlast()
    {
        fireBlastCollider.enabled = true;
        fireBlastBackParticles.Play();
        fireBlastFrontParticles.Play();
        fireBlastTime = fireBlastDuration;
    }

    void EndFireBlast()
    {
        fireBlastCollider.enabled = false;
        fireBlastBackParticles.Stop();
        fireBlastFrontParticles.Stop();
    }
}
