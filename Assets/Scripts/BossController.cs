using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class BossController : EnemyController, IEndDialogue
{
    [SerializeField] Transform exitDoor;
    [SerializeField] MapData mapData;
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] GameObject bonfirePrefab;
    [SerializeField] GameObject fireWavePrefab;
    [SerializeField] GameObject groundFirePrefab;
    [SerializeField] Transform frontAttackPoint;
    [SerializeField] Transform backAttackPoint;
    BossDialogue bossDialogue;
    FacePlayer facePlayer;
    FireRing fireRing;
    public int strafeLeftOrRight = 1;
    PlayerMovement playerMovement;
    InputManager im;
    MusicManager musicManager;

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
    float strafeSpeed = 1;
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
    [System.NonSerialized] public bool hasSurrendered = false;
    Vector3 attackOffset = new Vector3(0, 0.8f, 0);

    public override void Start()
    {
        base.Start();
        im = gm.GetComponent<InputManager>();
        facePlayer = GetComponent<FacePlayer>();
        playerMovement = playerScript.GetComponent<PlayerMovement>();
        bonfireCD = bonfireMaxCD;
        spellAttackPoiseDamage = fireBallPoiseDamage;
        spellAttackDamage = fireBallDamage;
        bossDialogue = GetComponent<BossDialogue>();
        fireRing = GetComponentInChildren<FireRing>();
        state = EnemyState.UNAWARE;
        musicManager = gm.GetComponentInChildren<MusicManager>();
        if (mapData.fireBossKilled)
        {
            GameObject bossHealthbar = enemyScript.healthbar.transform.parent.gameObject;
            bossHealthbar.SetActive(false);
            musicManager.ChangeMusicState(MusicState.MAINLOOP);
            gm.enemies.Remove(enemyScript);
            Destroy(gameObject);
        }
        else
        {
            gm.awareEnemies += 1;
        }
    }

    public override void Update()
    {
        base.Update();
        if(enemyScript.health < phaseTrigger)
        {
            musicManager.ChangeMusicState(MusicState.BOSSLOOPB);
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
        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);

        if (navAgent.enabled == true)
        {
            navAgent.SetDestination(playerScript.transform.position);
        }

        if (state == EnemyState.IDLE && playerDistance < attackRange)
        {
            if (navAgent.enabled)
            {
                if(playerDistance > tooClose) 
                {
                    Strafe();
                }
            }

            if (attackCD <= 0)
            {
                if (playerDistance < tooClose)
                {
                    if(phase == 1)
                    {
                        runAwayTime = runAwayMaxTime;
                    }
                    else
                    {
                        state = EnemyState.ATTACKING;
                        frontAnimator.Play("FireRing");
                        backAnimator.Play("FireRing");
                    }
                }
                else if (bonfireCD <= 0)
                {
                    state = EnemyState.ATTACKING;
                    pauseTimer = true;
                    frontAnimator.Play("Bonfires");
                    backAnimator.Play("Bonfires");
                    bonfireCD = bonfireMaxCD;
                }
                else if (fireBallCD <= 0)
                {
                    state = EnemyState.ATTACKING;
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
        else if(state == EnemyState.SPECIAL)
        {
            navAgent.enabled = true;
            navAgent.SetDestination(exitDoor.position);
            facePlayer.SetDestination(exitDoor.position);
            facePlayer.ManualFace();
            if(Vector3.Distance(exitDoor.position, transform.position) < 2)
            {
                enemyScript.Death();
                Death();
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

    public override void OnLosePoise(object sender, System.EventArgs e)
    {
        base.OnLosePoise(sender, e);
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
        Vector3 startPos = playerMovement.transform.position + new Vector3(xPos * Mathf.Pow(-1, xDir), 0, zPos * Mathf.Pow(-1, yDir));
        NavMeshHit hit;
        NavMesh.SamplePosition(startPos, out hit, bonfireSummonRadius + 1, NavMesh.AllAreas);
        GameObject bonfire = Instantiate(bonfirePrefab);
        bonfire.transform.position = hit.position;
        bonfireCD = bonfireMaxCD;
        bonfire.GetComponent<Bonfire>().enemyOfOrigin = enemyScript;
    }

    public void FireWave()
    {
        GameObject fireWave;
        fireWave = Instantiate(fireWavePrefab);
        fireWave.transform.position = transform.position + attackOffset;
        FireWave fireWaveScript;
        fireWaveScript = fireWave.GetComponent<FireWave>();
        fireWaveScript.target = playerScript.transform.position + attackOffset;
        fireWaveScript.enemyOfOrigin = enemyScript;
    }

    void RunAway()
    {
        Vector3 awayDirection = transform.position - playerScript.transform.position;
        if (navAgent.enabled)
        {
            navAgent.Move(awayDirection.normalized * Time.deltaTime * runAwaySpeed);
        }
    }

    public void FireRing()
    {
        fireRing.Explode();
        if (Vector3.Distance(transform.position, playerScript.transform.position) < fireRingRadius && playerScript.gameObject.layer == 3)
        {
            playerScript.LoseHealth(fireRingDamage,EnemyAttackType.MELEE, enemyScript);

            if (playerAbilities.shield) return;

            playerScript.LosePoise(fireRingPoiseDamage);
            Rigidbody playerRB = playerScript.gameObject.GetComponent<Rigidbody>();
            Vector3 awayVector = playerMovement.transform.position - transform.position;
            PlayerAnimation playerAnimation = playerMovement.gameObject.GetComponent<PlayerAnimation>();
            playerAnimation.attacking = false;
            playerRB.velocity = Vector3.zero;
            StartCoroutine(playerMovement.KnockBack(0.4f));
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
        groundFireScript.target = playerScript.transform;
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
        Vector3 playerToBoss = transform.position - playerScript.transform.position;
        playerToBoss *= strafeLeftOrRight;
        Vector3 strafeDirection = Vector3.Cross(Vector3.up, playerToBoss);
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
        projectile.transform.LookAt(playerScript.transform.position);
        projectileScript.target = playerScript.transform;
        projectileScript.poiseDamage = spellAttackPoiseDamage;
        projectileScript.spellDamage = spellAttackDamage;
        projectileScript.enemyOfOrigin = enemyScript;
        enemySound.OtherSounds(0, 1);
    }

    public override void StartDying()
    {
        enemyScript.invincible = true;
        enemyScript.health = 1;
        bossDialogue.EndLookUpDialogue();
        if (!hasSurrendered)
        {
            frontAnimator.Play("HandsUp");
            backAnimator.Play("HandsUp");
        }
    }

    public void EndDialogue()
    {
        if(state == EnemyState.UNAWARE)
        {
            state = EnemyState.IDLE;
            musicManager.ChangeMusicState(MusicState.BOSSLOOPA);
        }
        else
        {
            im.Dialogue();
            frontAnimator.Play("StandUp");
            backAnimator.Play("StandUp");
            navAgent.enabled = false;
            navAgent.stoppingDistance = 0;
        }
    }

    public override void Death()
    {
        base.Death();
        im.Gameplay();
        bossDialogue.EndLookUpDialogue();
        mapData.fireBossKilled = true;
        gm.awareEnemies -= 1;
        GameObject bossHealthbar = enemyScript.healthbar.transform.parent.gameObject;
        bossHealthbar.SetActive(false);
        GlobalEvents.instance.BossKilled();
    }
}
