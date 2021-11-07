using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossController : EnemyController
{
    [SerializeField] GameObject fireBlastFront;
    [SerializeField] GameObject fireBlastBack;
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] GameObject bonfirePrefab;
    public int fireBlastDamage = 30;
    public int strafeLeftOrRight = 1;

    Collider fireBlastCollider;
    ParticleSystem fireBlastFrontParticles;
    ParticleSystem fireBlastBackParticles;
    float fireBlastRange = 3f;
    float fireBlastDuration = 0.5f;
    float fireBlastTime;
    float fireBallCD;
    float fireBallMaxCD = 4;
    float attackCD;
    float attackMaxCD = 2;
    float fireTrailMaxTime = 0.2f;
    float fireTrailTime;
    float bonfireMaxTime = 5;
    float bonfireTime;
    float tooClose = 3f;
    float strafeSpeed = 5;

    public override void Start()
    {
        base.Start();
        fireBlastCollider = fireBlastFront.GetComponent<Collider>();
        fireBlastFrontParticles = fireBlastFront.GetComponent<ParticleSystem>();
        fireBlastBackParticles = fireBlastBack.GetComponent<ParticleSystem>();
        bonfireTime = bonfireMaxTime;
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
                if (navAgent.enabled)
                {
                    if(Vector3.Distance(transform.position, playerController.transform.position) > tooClose)
                    {
                        Strafe();
                    }
                }

                if (attackCD <= 0)
                {
                    if(Vector3.Distance(transform.position, playerController.transform.position) < fireBlastRange)
                    {
                        frontAnimator.SetTrigger("FireBlast");
                    }
                    else if (fireBallCD <= 0)
                    {
                        frontAnimator.SetTrigger("SpellAttack");
                        fireBallCD = fireBallMaxCD;
                    }
                    attackCD = attackMaxCD;
                }
            }

        }

        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }

        if(fireBallCD > 0)
        {
            fireBallCD -= Time.deltaTime;
        }

        if(fireBlastTime > 0)
        {
            fireBlastTime -= Time.deltaTime;
            if(fireBlastTime <= 0)
            {
                EndFireBlast();
            }
        }

        if(fireTrailTime < 0)
        {
            FireTrail();
            fireTrailTime = fireTrailMaxTime;
        }
        else
        {
            fireTrailTime -= Time.deltaTime;
        }

        bonfireTime -= Time.deltaTime;
        if(bonfireTime <= 0)
        {
            GameObject bonfire = Instantiate(bonfirePrefab);
            bonfire.transform.position = playerController.transform.position;
            bonfire.transform.position -= new Vector3(0, playerController.transform.position.y, 0);
            bonfireTime = bonfireMaxTime;
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

    public void FireTrail()
    {
        GameObject fireTrail;
        fireTrail = Instantiate(fireTrailPrefab);
        fireTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Strafe()
    {
        Vector3 playerToBoss = transform.position - playerController.transform.position;
        playerToBoss *= strafeLeftOrRight;
        Vector3 strafeDirection = Vector3.Cross(transform.position, playerToBoss);
        navAgent.Move(strafeDirection.normalized * Time.deltaTime * strafeSpeed);
    }
}
