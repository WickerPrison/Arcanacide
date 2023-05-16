using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using Random = UnityEngine.Random;

[System.Serializable]
public class IceBoss : EnemyController
{
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

    public override void Start()
    {
        base.Start();

        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();

        beamCrystal = GetComponentInChildren<IceBossBeamCrystal>();

        line.SetPosition(0, away);
        line.SetPosition(1, away);

        layerMask = LayerMask.GetMask("Default", "Player", "IFrames");

        gradient = new Gradient();
        GradientColorKey[] colorKey = { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(blueColor, 1.0f) };
        GradientAlphaKey[] alphaKey = { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) };
        gradient.SetKeys(colorKey, alphaKey);

        aimValue = maxAimValue;
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (state == EnemyState.IDLE)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if (playerDistance > tooFarAway)
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
                if (beamCrystal.spawnedIn)
                {
                    beamCrystal.SpawnOut();
                }
                line.SetPosition(0, away);
                line.SetPosition(1, away);
            }

            if (playerDistance <= meleeRange && attackTime <= 0)
            {
                navAgent.velocity = Vector3.zero;
                attackTime = attackMaxTime;
                int randInt;
                if(currentLimb < 4)
                {
                    randInt = Random.Range(0, currentLimb + 1);
                }
                else
                {
                    randInt = Random.Range(1, currentLimb);
                }

                if (justTransformed)
                {
                    randInt = currentLimb;
                    justTransformed = false;
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

        if (fullyTransformed)
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

        if (playerController.gameObject.layer == 3)
        {
            int damage = smashDamage;
            if (currentLimb > 2) damage += 15;
            enemySound.SwordImpact();
            playerScript.LoseHealth(damage, enemyScript);
            playerScript.LosePoise(smashPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerController.gameObject.layer == 8)
        {
            playerController.PerfectDodge();
        }
    }

    public void RingBlast(float lowerBound, float upperBound)
    {
        enemySound.OtherSounds(1, 2);
        float playerDistance = Vector3.Distance(playerController.transform.position, transform.position);
        if (playerDistance > lowerBound && playerDistance < upperBound)
        {
            if(playerController.gameObject.layer == 3)
            {
                playerScript.LoseHealth(ringBlastDamage);
                playerScript.LosePoise(ringBlastPoiseDamage);
            }
            else if(playerController.gameObject.layer == 8)
            {
                playerController.PerfectDodge();
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
            icicle.position = playerController.transform.position;
        }
    }

    void ShowBeam()
    {
        RaycastHit hit;
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
        projectile.direction = playerController.transform.position - transform.position;
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
                attackArc.HideAttackArc();
                iceBreath.StopIceBreath();
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
        attackArc.HideAttackArc();
    }

    public override void EndStagger()
    {
        navAgent.speed = 5;
        base.EndStagger();
    }


    private void OnEnable()
    {
        enemyScript.OnTakeDamage += TakeDamage;
    }
}
