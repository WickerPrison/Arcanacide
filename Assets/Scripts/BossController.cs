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
    [SerializeField] GameObject groundFirePrefab;
    [SerializeField] Transform frontAttackPoint;
    [SerializeField] Transform backAttackPoint;
    BossDialogue bossDialogue;
    FireRing fireRing;
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
    public bool canStagger = false;
    int goingUp = 1;
    public bool pauseTimer = false;
    int fireBallDamage = 20;
    int fireBallPoiseDamage = 15;
    public int phase = 1;
    int phaseTrigger = 300;
    int phaseCounter = 3;
    int fireRingDamage = 50;
    float fireRingPoiseDamage = 150;
    float fireRingRadius = 3.5f;

    public override void Start()
    {
        base.Start();
        bonfireCD = bonfireMaxCD;
        spellAttackPoiseDamage = fireBallPoiseDamage;
        spellAttackDamage = fireBallDamage;
        bossDialogue = GetComponent<BossDialogue>();
        fireRing = GetComponentInChildren<FireRing>();
    }

    public override void Update()
    {
        base.Update();
        if(enemyScript.health < phaseTrigger)
        {
            phaseTrigger = 0;
            StartPhase2();
        }
    }

    void FixedUpdate()
    {
        UpAndDown();
    }

    public override void EnemyAI()
    {
        if (!hasSeenPlayer)
        {
            return;
        }

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

            if (attackCD <= 0)
            {
                if (Vector3.Distance(transform.position, playerController.transform.position) < tooClose)
                {
                    if(phase == 1)
                    {
                        runAwayTime = runAwayMaxTime;
                    }
                    else
                    {
                        attacking = true;
                        frontAnimator.Play("FireRing");
                        backAnimator.Play("FireRing");
                    }
                }
                else if (bonfireCD <= 0)
                {
                    attacking = true;
                    pauseTimer = true;
                    frontAnimator.Play("Bonfires");
                    backAnimator.Play("Bonfires");
                    bonfireCD = bonfireMaxCD;
                }
                else if (fireBallCD <= 0)
                {
                    attacking = true;
                    int num = Random.Range(1, phaseCounter);
                    if (num == 1)
                    {
                        frontAnimator.Play("FireBalls");
                        backAnimator.Play("FireBalls");
                    }
                    if (num == 2)
                    {
                        frontAnimator.Play("FireWave");
                        backAnimator.Play("FireWave");
                    }
                    if(num == 3)
                    {
                        frontAnimator.Play("GroundFire");
                        backAnimator.Play("GroundFire");
                    }
                    fireBallCD = fireBallMaxCD;
                }
                attackCD = attackMaxCD;
            }
        }

        if(runAwayTime > 0)
        {
            runAwayTime -= Time.deltaTime;
            RunAway();
        }

        if (!pauseTimer && attackCD > 0)
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

        if(fireTrailTime < 0)
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
            canStagger = false;
            bossDialogue.EndLookUpDialogue();
            StartStagger(3);
        }
    }

    public override void EndStagger()
    {
        base.EndStagger();
        canStagger = false;
        pauseTimer = false;
    }


    public void Bonfire()
    {
        float bonfireMinSummonRadius = 2;
        float bonfireSummonRadius = 7;
        int xDir = Random.Range(1, 3);
        int yDir = Random.Range(1, 3);
        float xPos = Random.Range(bonfireMinSummonRadius, bonfireSummonRadius);
        float zPos = Random.Range(bonfireMinSummonRadius, bonfireSummonRadius);
        Vector3 startPos = playerController.transform.position + new Vector3(xPos * Mathf.Pow(-1, xDir), -1, zPos * Mathf.Pow(-1, yDir));
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

    public void FireRing()
    {
        fireRing.Explode();
        if (Vector3.Distance(transform.position, playerController.transform.position) < fireRingRadius && playerController.gameObject.layer == 3)
        {
            playerScript.LoseHealth(fireRingDamage);
            playerScript.LosePoise(fireRingPoiseDamage);
            Rigidbody playerRB = playerScript.gameObject.GetComponent<Rigidbody>();
            Vector3 awayVector = playerController.transform.position - transform.position;
            playerController.knockback = true;
            PlayerAnimation playerAnimation = playerController.gameObject.GetComponent<PlayerAnimation>();
            playerAnimation.attacking = false;
            playerRB.velocity = Vector3.zero;
            playerRB.AddForce(awayVector.normalized * 7, ForceMode.VelocityChange);
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

    public void GroundFire()
    {
        GameObject groundFire = Instantiate(groundFirePrefab);
        groundFire.transform.position = transform.position;
        GroundFire groundFireScript = groundFire.GetComponent<GroundFire>();
        groundFireScript.target = playerController.transform;
    }

    public void FireBlast()
    {

    }

    void StartPhase2()
    {
        phase = 2;
        backAnimator.SetInteger("Phase", 2);
        frontAnimator.SetInteger("Phase", 2);
        phaseCounter = 4;
    }

    void Strafe()
    {
        Vector3 playerToBoss = transform.position - playerController.transform.position;
        playerToBoss *= strafeLeftOrRight;
        Vector3 strafeDirection = Vector3.Cross(transform.position, playerToBoss);
        navAgent.Move(strafeDirection.normalized * Time.deltaTime * strafeSpeed);
    }

    public override void SpellAttack()
    {
        GameObject projectile;
        HomingProjectile projectileScript;
        projectile = Instantiate(projectilePrefab);
        projectileScript = projectile.GetComponent<HomingProjectile>();
        if (facingFront)
        {
            projectile.transform.position = frontAttackPoint.position;
        }
        else
        {
            projectile.transform.position = backAttackPoint.position;
        }
        projectile.transform.LookAt(playerController.transform.position);
        projectileScript.target = playerController.transform;
        projectileScript.poiseDamage = spellAttackPoiseDamage;
        projectileScript.spellDamage = spellAttackDamage;
    }
}
