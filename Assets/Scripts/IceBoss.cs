using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class IceBoss : EnemyController
{
    public MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;

    int smashDamage = 30;
    float smashPoiseDamage = 30;

    int ringBlastDamage = 40;
    float ringBlastPoiseDamage = 50;

    float damageCounter;
    float fullyTransformedDamage = 4;


    [SerializeField] GameObject iciclePrefab;
    [SerializeField] float[] limbTransitions;
    [SerializeField] string[] limbAnimationNames;
    [System.NonSerialized] public int currentLimb = 0;
    IceBossBeamCrystal beamCrystal;
    [SerializeField] BossIceBreath iceBreath;
    [SerializeField] AttackArcGenerator attackArc;
    public IceBossAnimationEvents animationEvents;
    CameraFollow cameraScript;

    float tooFarAway = 5;
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

    public override void Awake()
    {
        if (mapData.iceBossKilled)
        {
            enemyScript = GetComponent<EnemyScript>();
            enemyScript.enabled = false;
           // return;
        }

        base.Awake();
    }

    public override void Start()
    {
        beamCrystal = GetComponentInChildren<IceBossBeamCrystal>();

        line.SetPosition(0, away);
        line.SetPosition(1, away);

        base.Start();

        if (mapData.iceBossKilled)
        {
            SetupDeathPose();
            GameObject bossHealthbar = enemyScript.healthbar.transform.parent.gameObject;
            bossHealthbar.SetActive(false);
            MusicManager musicManager = gm.GetComponentInChildren<MusicManager>();
            musicManager.ImmediateStop();
            //return;
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

        base.EnemyAI();

        if (playerDistance > tooFarAway && state != EnemyState.DYING)
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

        if (fullyTransformed && state != EnemyState.DYING)
        {
            LoseHealth();
        }
    }

    public void Smash()
    {
        StartCoroutine(cameraScript.ScreenShake(0.1f, 0.2f));
        parryWindow = false;
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
            playerScript.PerfectDodge();
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
                playerScript.PerfectDodge();
            }
        }
    }

    public IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(15);
        fullyTransformed = true;
    }

    void LoseHealth()
    {
        if (state == EnemyState.DYING) return;

        damageCounter += fullyTransformedDamage * Time.deltaTime;

        if(damageCounter > 1)
        {
            enemyScript.invincible = false;
            enemyScript.LoseHealth(Mathf.RoundToInt(damageCounter), 0);
            damageCounter = 0;
            enemyScript.invincible = true;
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
        HideIndicators();
        HideCrystal();
        base.StartDying();
    }

    public override void Death()
    {
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

        if (playerData.equippedEmblems.Contains(emblemLibrary.vampiric_strikes))
        {
            PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            int healAmount = Mathf.FloorToInt(playerData.MaxHealth() / 5);
            playerScript.PartialHeal(healAmount);
        }

        mapData.iceBossKilled = true;
        playerScript.GainMaxHealCharges();
        gm.awareEnemies -= 1;
        GameObject bossHealthbar = enemyScript.healthbar.transform.parent.gameObject;
        bossHealthbar.SetActive(false);
        ManagerVanquished managerVanquished = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<ManagerVanquished>();
        managerVanquished.ShowMessage();
        SoundManager sm = gm.gameObject.GetComponent<SoundManager>();
        sm.BossDefeated();
        MusicManager musicManager = gm.GetComponentInChildren<MusicManager>();
        musicManager.StartFadeOut(4);
    }

    void HideIndicators()
    {
        attackArc.HideAttackArc();
        iceBreath.StopIceBreath();
        animationEvents.ClearAll();
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
}
