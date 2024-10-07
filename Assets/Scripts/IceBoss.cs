using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class IceBoss : EnemyController, IEndDialogue
{
    public MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject dialoguePrefab;
    Dialogue postDialogue;
    [SerializeField] Material healthbarMaterial;

    int smashDamage = 30;
    float smashPoiseDamage = 30;

    int ringBlastDamage = 40;
    float ringBlastPoiseDamage = 50;

    [SerializeField] GameObject iciclePrefab;
    [SerializeField] float[] limbTransitions;
    [SerializeField] string[] limbAnimationNames;
    [System.NonSerialized] public int currentLimb = 0;
    IceBossBeamCrystal beamCrystal;
    [SerializeField] BossIceBreath iceBreath;
    [SerializeField] AttackArcGenerator attackArc;
    public IceBossAnimationEvents animationEvents;
    CameraFollow cameraScript;

    float tooFarAway = 8;
    float meleeRange = 3;

    float icicleTimer;
    float icicleMaxTime = 1;
    RaycastHit hit;

    LayerMask layerMask;
    [SerializeField] LineRenderer line;
    Vector3 offset = new Vector3(0, 2, 0);
    Gradient gradient;
    [SerializeField] Color blueColor;
    float alpha = 1;
    float aimValue;
    float maxAimValue = 1.5f;
    float gradientOffset = .4f;
    Vector3 away = new Vector3(100, 100, 100);
    [System.NonSerialized] public bool justTransformed = false;
    bool fullyTransformed = false;
    MusicManager musicManager;
    int healthPercent;

    bool icicleDelay = true;

    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    public override void Awake()
    {
        if (mapData.iceBossKilled)
        {
            enemyScript = GetComponent<EnemyScript>();
            enemyScript.enabled = false;
        }

        base.Awake();
    }

    public override void Start()
    {
        postDialogue = GetComponent<Dialogue>();
        beamCrystal = GetComponentInChildren<IceBossBeamCrystal>();
        healthbarMaterial.SetFloat("_IceThreshold", 1);

        line.SetPosition(0, away);
        line.SetPosition(1, away);

        base.Start();

        musicManager = gm.GetComponentInChildren<MusicManager>();

        if (mapData.iceBossKilled)
        {
            SetupDeathPose();
            GameObject bossHealthbar = enemyScript.healthbar.transform.parent.gameObject;
            bossHealthbar.SetActive(false);
            musicManager.ChangeMusicState(MusicState.MAINLOOP);
            GameObject dialogueTrigger = GetComponentInChildren<DialogueTriggerRoomEntrance>().gameObject;
            Destroy(dialogueTrigger);
        }
        else
        {
            gm.awareEnemies += 1;
            state = EnemyState.UNAWARE;
        }

        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();


        layerMask = LayerMask.GetMask("Default", "Player", "IFrames");

        gradient = new Gradient();
        GradientColorKey[] colorKey = { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(blueColor, 1.0f) };
        GradientAlphaKey[] alphaKey = { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) };
        gradient.SetKeys(colorKey, alphaKey);

        aimValue = maxAimValue;
    }

    public override void EnemyAI()
    {
        if (mapData.iceBossKilled) return;

        if (state == EnemyState.UNAWARE) return;

        healthPercent = Mathf.RoundToInt(enemyScript.health / enemyScript.maxHealth * 100);
        musicManager.UpdateBossHealth(healthPercent);

        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);

        if (!icicleDelay && playerDistance > tooFarAway && state != EnemyState.DYING)
        {
            RainIcicles();

            if (!beamCrystal.spawnedIn)
            {
                beamCrystal.SpawnIn();
                line.SetPosition(0, away);
                line.SetPosition(1, away);
            }
            else
            {
                ShowBeam();
            }
        }
        else
        {
            HideCrystal();
        }


        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerScript.transform.position);
            }

            if (playerDistance <= meleeRange && attackTime <= 0)
            {
                navAgent.velocity = Vector3.zero;
                attackTime = attackMaxTime;
                int randInt;
                if(currentLimb < 4)
                {
                    randInt = Random.Range(0, currentLimb + 1);
                    if (justTransformed)
                    {
                        randInt = currentLimb;
                        justTransformed = false;
                    }
                }
                else
                {
                    randInt = Random.Range(1, currentLimb);
                }


                state = EnemyState.ATTACKING;
                switch (randInt)
                {
                    case 0:
                        frontAnimator.Play("BreathAttack");
                        backAnimator.Play("BreathAttack");
                        break;
                    case 1:
                        frontAnimator.Play("Smash");
                        backAnimator.Play("Smash");
                        break;
                    case 2:
                        frontAnimator.Play("Stomp");
                        backAnimator.Play("Stomp");
                        break;
                    case 3:
                        frontAnimator.Play("RingBlast");
                        backAnimator.Play("RingBlast");
                        break;
                }
            }
        }

        if (attackTime > 0 && state != EnemyState.SPECIAL)
        {
           attackTime -= Time.deltaTime;
        }
    }

    public void Smash()
    {
        StartCoroutine(cameraScript.ScreenShake(0.1f, 0.2f));
        enemySound.OtherSounds(0,2);

        if (!canHitPlayer)
        {
            return;
        }

        if (playerScript.gameObject.layer == 3)
        {
            int damage = smashDamage;
            if (currentLimb > 2) damage += 15;
            enemySound.SwordImpact();
            playerScript.LoseHealth(damage, EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(smashPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerScript.gameObject.layer == 8)
        {
            playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
        }
    }

    public void RingBlast(float lowerBound, float upperBound)
    {
        enemySound.OtherSounds(1, 2);
        float playerDistance = Vector3.Distance(playerScript.transform.position, transform.position);
        if (playerDistance > lowerBound && playerDistance < upperBound)
        {
            if(playerScript.gameObject.layer == 3)
            {
                playerScript.LoseHealth(ringBlastDamage, EnemyAttackType.NONPARRIABLE, null);
                playerScript.LosePoise(ringBlastPoiseDamage);
            }
            else if(playerScript.gameObject.layer == 8)
            {
                playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE);
            }
        }
    }

    private void RainIcicles()
    {
        icicleTimer -= Time.deltaTime;
        if(icicleTimer < 0)
        {
            icicleTimer = icicleMaxTime;
            Transform icicle = Instantiate(iciclePrefab).transform;
            icicle.position = playerScript.transform.position;
            Projectile projectile = icicle.GetComponentInChildren<Projectile>();
            projectile.enemyOfOrigin = enemyScript;
        }
    }

    void ShowBeam()
    {
        Physics.Linecast(transform.position, playerScript.transform.position, out hit, layerMask, QueryTriggerInteraction.Ignore);
        line.SetPosition(0, beamCrystal.transform.position);

        if (hit.collider.CompareTag("Player"))
        {
            line.SetPosition(1, playerScript.transform.position + offset);
            aimValue -= Time.deltaTime;
            if (aimValue < -gradientOffset)
            {
                aimValue = maxAimValue;
                Shoot();
            }
        }
        else
        {
            line.SetPosition(1, hit.point + offset);
            aimValue = maxAimValue;
        }

        float aimRatio = aimValue / maxAimValue;

        if (aimRatio < 1 - gradientOffset)
        {
            line.startColor = gradient.Evaluate(aimRatio + gradientOffset);
        }
        else
        {
            line.startColor = blueColor;
        }
        line.endColor = gradient.Evaluate(aimRatio);
    }

    void Shoot()
    {

        Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = beamCrystal.transform.position;
        projectile.direction = hit.point + offset - projectile.transform.position;
        float angle = Vector3.SignedAngle(Vector3.forward, projectile.direction, Vector3.up);
        projectile.transform.rotation = Quaternion.Euler(25, 0, -angle);
    }

    void TakeDamage(object sender, System.EventArgs e)
    {
        float enemyPercentHealth = (float)enemyScript.health / (float)enemyScript.maxHealth;
        for(int i = currentLimb; i < limbTransitions.Length; i++)
        {
            if(enemyPercentHealth < limbTransitions[i])
            {
                HideIndicators();
                currentLimb++;
                state = EnemyState.SPECIAL;
                if(i == 1)
                {
                    frontAnimator.SetBool("IsTall", true);
                    backAnimator.SetBool("IsTall", true);
                }
                frontAnimator.Play(limbAnimationNames[i]);
                backAnimator.Play(limbAnimationNames[i]);
            }
        }
    }

    public override void StartStagger(float staggerDuration)
    {
        if (state == EnemyState.SPECIAL) return;
        base.StartStagger(staggerDuration);
        HideIndicators();
    }

    public override void EndStagger()
    {
        navAgent.speed = 5;
        base.EndStagger();
    }

    public override void StartDying()
    {
        enemyScript.health = 1;
        HideIndicators();
        frontAnimator.Play("Torso");
        backAnimator.Play("Torso");
        postDialogue.StartWithCallback(StartFinalPhase);
    }

    void StartFinalPhase()
    {
        StartCoroutine(FinalPhaseHealthbar());
        navAgent.speed = 3;
        navAgent.enabled = true;
        state = EnemyState.IDLE;
        directionLock = false;
    }

    IEnumerator FinalPhaseHealthbar()
    {
        float maxTime = 4;
        float timer = maxTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float ratio = 1 - timer / maxTime;
            enemyScript.health = Mathf.RoundToInt(enemyScript.maxHealth * ratio);
            enemyScript.UpdateHealthbar();
            yield return endOfFrame;
        }
        enemyScript.health = enemyScript.maxHealth;
        enemyScript.UpdateHealthbar();

        yield return new WaitForSeconds(3);

        maxTime = 25f;
        timer = maxTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float threshold = Mathf.Lerp(0.55f, 1, timer / maxTime);
            healthbarMaterial.SetFloat("_IceThreshold", threshold);
            yield return endOfFrame;
        }

        healthbarMaterial.SetFloat("_IceThreshold", 0);
        frontAnimator.Play("Death");
        backAnimator.Play("Death");
    }

    public override void Death()
    {
        HideIndicators();
        mapData.iceBossPosition = transform.position;
        mapData.iceBossDirection = animationEvents.facePlayerSlow.faceDirectionID;


        playerData.killedEnemiesNum += 1;

        if (playerData.equippedEmblems.Contains(emblemLibrary.pay_raise))
        {
            playerData.money += Mathf.RoundToInt(enemyScript.reward * 1.25f);
        }
        else
        {
            playerData.money += enemyScript.reward;
        }
        gm.enemies.Remove(enemyScript);
        gm.enemiesInRange.Remove(enemyScript);
        gm.awareEnemies -= 1;

        GlobalEvents.instance.EnemyKilled();

        mapData.iceBossKilled = true;
        gm.awareEnemies -= 1;
        GameObject bossHealthbar = enemyScript.healthbar.transform.parent.gameObject;
        bossHealthbar.SetActive(false);
        GlobalEvents.instance.BossKilled();
    }

    void HideIndicators()
    {
        attackArc.HideAttackArc();
        iceBreath.StopIceBreath();
        animationEvents.ClearAll();
        line.SetPosition(0, away);
        line.SetPosition(1, away);
    }

    void HideCrystal()
    {
        if (beamCrystal.spawnedIn)
        {
            beamCrystal.SpawnOut();
        }
        line.SetPosition(0, away);
        line.SetPosition(1, away);
    }

    void SetupDeathPose()
    {
        transform.position = mapData.iceBossPosition;
        FacePlayer facePlayer = GetComponent<FacePlayer>();
        facePlayer.FaceDirection(mapData.iceBossDirection);
        directionLock = true;
        frontAnimator.Play("DeathPose");
        backAnimator.Play("DeathPose");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        enemyEvents.OnTakeDamage += TakeDamage;
    }

    void IEndDialogue.EndDialogue()
    {
        state = EnemyState.IDLE;
        musicManager.ChangeMusicState(MusicState.BOSSMUSIC);
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        icicleDelay = false;
    }
}
