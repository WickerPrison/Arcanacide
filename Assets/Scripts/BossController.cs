using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class BossController : EnemyController
{
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] GameObject bonfirePrefab;
    [SerializeField] GameObject fireWavePrefab;
    public int strafeLeftOrRight = 1;

    float tooClose = 3f;
    float runAwayTime;
    float runAwayMaxTime = 1f;
    float runAwaySpeed = 8f;
    float fireBallCD;
    float fireBallMaxCD = 5;
    float attackCD;
    float attackMaxCD = 2;
    float fireTrailMaxTime = 0.2f;
    float fireTrailTime;
    float bonfireMaxCD = 15;
    float bonfireCD;
    float strafeSpeed = 3;
    float floatTimer;
    float floatMaxTime = 1;
    float floatSpeed = 0.3f;
    float staggerTimer = 0;
    float staggerMaxTime = 3;
    public bool canStagger = false;
    bool isStaggered = false;
    int goingUp = 1;

    public override void Start()
    {
        base.Start();
        bonfireCD = bonfireMaxCD;
    }

    public override void Update()
    {
        base.Update();
        frontAnimator.SetFloat("Stagger", staggerTimer);
    }

    void FixedUpdate()
    {
        UpAndDown();
    }

    public override void EnemyAI()
    {
        if (staggerTimer > 0)
        {
            staggerTimer -= Time.deltaTime;
            if(staggerTimer <= 0)
            {
                EndStagger();
            }
        }

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

            if (Vector3.Distance(transform.position, playerController.transform.position) < attackRange)
            {
                if (navAgent.enabled)
                {
                    if(Vector3.Distance(transform.position, playerController.transform.position) > tooClose)
                    {
                        Strafe();
                    }
                }

                if (attackCD <= 0 && !isStaggered)
                {
                    if(Vector3.Distance(transform.position, playerController.transform.position) < tooClose && !isStaggered)
                    {
                        runAwayTime = runAwayMaxTime;
                    }
                    else if(bonfireCD <= 0)
                    {
                        frontAnimator.SetTrigger("Bonfire");
                        bonfireCD = bonfireMaxCD;
                    }
                    else if (fireBallCD <= 0)
                    {
                        int num = Random.Range(1, 3);
                        if (num == 1)
                        {
                            frontAnimator.SetTrigger("SpellAttack");
                        }
                        if(num == 2)
                        {
                            frontAnimator.SetTrigger("FireWave");
                        }
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

        if(bonfireCD > 0)
        {
            bonfireCD -= Time.deltaTime;
        }

        if(fireTrailTime < 0 && !isStaggered)
        {
            FireTrail();
            fireTrailTime = fireTrailMaxTime;
        }
        else
        {
            fireTrailTime -= Time.deltaTime;
        }
    }

    void UpAndDown()
    {
        if(floatTimer > 0)
        {
            floatTimer -= Time.deltaTime;
            transform.Translate(Vector3.up * goingUp * Time.fixedDeltaTime * floatSpeed);
        }
        else
        {
            floatTimer = floatMaxTime;
            goingUp *= -1;
        }
    }

    public override void OnHit()
    {
        base.OnHit();
        if (canStagger)
        {
            frontAnimator.Play("Stagger");
            isStaggered = true;
            staggerTimer = staggerMaxTime;
        }
    }

    void EndStagger()
    {
        BossAnimationEvents animationEvents = frontAnimator.GetComponent<BossAnimationEvents>();
        animationEvents.CanNotStagger();
        animationEvents.EnableMovement();
        frontAnimator.Play("Idle");
        isStaggered = false;
    }


    public void Bonfire()
    {
        float bonfireSummonRadius = 7;
        float xPos = Random.Range(-bonfireSummonRadius, bonfireSummonRadius);
        float zPos = Random.Range(-bonfireSummonRadius, bonfireSummonRadius);
        Vector3 startPos = transform.position + new Vector3(xPos, -1, zPos);
        NavMeshHit hit;
        NavMesh.SamplePosition(startPos, out hit, bonfireSummonRadius + 1, NavMesh.AllAreas);
        GameObject bonfire = Instantiate(bonfirePrefab);
        bonfire.transform.position = hit.position;
        bonfireCD = bonfireMaxCD;
    }

    public void FireWave()
    {
        GameObject fireWave;
        fireWave = Instantiate(fireWavePrefab);
        fireWave.transform.position = transform.position;
        FireWave fireWaveScript;
        fireWaveScript = fireWave.GetComponent<FireWave>();
        fireWaveScript.target = playerController.transform.position;
    }

    void RunAway()
    {
        Vector3 awayDirection = transform.position - playerController.transform.position;
        if (navAgent.enabled)
        {
            navAgent.Move(awayDirection.normalized * Time.deltaTime * runAwaySpeed);
        }
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
