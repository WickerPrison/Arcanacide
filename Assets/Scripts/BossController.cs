using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossController : EnemyController
{
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] GameObject bonfirePrefab;
    public int strafeLeftOrRight = 1;

    float tooClose = 3f;
    float runAwayTime;
    float runAwayMaxTime = 1f;
    float runAwaySpeed = 8f;
    float fireBallCD;
    float fireBallMaxCD = 4;
    float attackCD;
    float attackMaxCD = 2;
    float fireTrailMaxTime = 0.2f;
    float fireTrailTime;
    float bonfireMaxTime = 5;
    float bonfireTime;
    float strafeSpeed = 5;

    public override void Start()
    {
        base.Start();
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
                    if(Vector3.Distance(transform.position, playerController.transform.position) < tooClose)
                    {
                        runAwayTime = runAwayMaxTime;
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

        if(runAwayTime > 0)
        {
            runAwayTime -= Time.deltaTime;
            RunAway();
        }

        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }

        if(fireBallCD > 0)
        {
            fireBallCD -= Time.deltaTime;
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

        /*
        bonfireTime -= Time.deltaTime;
        if(bonfireTime <= 0)
        {
            GameObject bonfire = Instantiate(bonfirePrefab);
            bonfire.transform.position = playerController.transform.position;
            bonfire.transform.position -= new Vector3(0, 1, 0);
            bonfireTime = bonfireMaxTime;
        }
        */
    }


    void RunAway()
    {
        Vector3 awayDirection = transform.position - playerController.transform.position;
        navAgent.Move(awayDirection.normalized * Time.deltaTime * runAwaySpeed);
    }

    public void FireTrail()
    {
        GameObject fireTrail;
        fireTrail = Instantiate(fireTrailPrefab);
        fireTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        if(runAwayTime > 0)
        {
            fireTrail.transform.localScale = Vector3.Scale(fireTrail.transform.localScale, new Vector3(2f, 1f, 2f));
        }
    }

    void Strafe()
    {
        Vector3 playerToBoss = transform.position - playerController.transform.position;
        playerToBoss *= strafeLeftOrRight;
        Vector3 strafeDirection = Vector3.Cross(transform.position, playerToBoss);
        navAgent.Move(strafeDirection.normalized * Time.deltaTime * strafeSpeed);
    }
}
