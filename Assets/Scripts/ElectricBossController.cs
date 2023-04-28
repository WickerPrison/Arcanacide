using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ElectricBossController : EnemyController
{
    // special means running away
    [SerializeField] Transform[] firePoints;
    [SerializeField] GameObject hadokenPrefab;
    [SerializeField] MapData mapData;
    float meleeRange = 3;
    StepWithAttack stepWithAttack;
    [System.NonSerialized] public FacePlayer facePlayer;
    float fleeRadiusMin = 6;
    float fleeRadiusMax = 11;
    Vector3 fleePoint;

    [SerializeField] GameObject chargeIndicator;
    float chargeIndicatorWidth;
    [SerializeField] float maxChargeDistance;
    LayerMask playerMask;
    LayerMask layerMask;
    List<Vector3> chargePath = new List<Vector3>();
    List<ChargeIndicator> chargeIndicators = new List<ChargeIndicator>();
    bool charging = false;
    [SerializeField] float chargeSpeed;
    float chargeDelay = 0.7f;
    CapsuleCollider enemyCollider;
    [SerializeField] int chargeDamage;
    [SerializeField] int chargeBurstDamage;
    [SerializeField] float chargeBurstPoiseDamage;
    [SerializeField] float chargeBurstStagger;
    bool isColliding;
    [SerializeField] float[] hadokenAngles;
    public bool phase2 = false;
    int phaseTrigger = 200;
    [SerializeField] ParticleSystem bodyLightning;
    float abilityTime;
    float abilityMaxTime = 5;

    public override void Start()
    {
        base.Start();
        stepWithAttack = GetComponent<StepWithAttack>();
        facePlayer = GetComponent<FacePlayer>();
        layerMask = LayerMask.GetMask("Default");
        playerMask = LayerMask.GetMask("Player");
        enemyCollider = GetComponent<CapsuleCollider>();
        chargeIndicatorWidth = enemyCollider.radius * 2;
        abilityTime = abilityMaxTime;
        ChooseRandomPoint();
        if (mapData.electricBossKilled)
        {
            GameObject bossHealthbar = enemyScript.healthbar.transform.parent.gameObject;
            bossHealthbar.SetActive(false);
            MusicManager musicManager = gm.GetComponentInChildren<MusicManager>();
            musicManager.ImmediateStop();
            gm.enemies.Remove(enemyScript);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (charging)
        {
            Charging();
        }
    }

    public override void EnemyAI()
    {
        if(!phase2 && enemyScript.health < phaseTrigger)
        {
            phase2 = true;
            bodyLightning.Play();
        }

        base.EnemyAI();
        if (state == EnemyState.IDLE)
        {
            frontAnimator.SetFloat("PlayerDistance", playerDistance);
            backAnimator.SetFloat("PlayerDistance", playerDistance);
            
            if(abilityTime <= 0)
            {
                RunAway();
            }
            else if(attackTime <= 0 && playerDistance < meleeRange)
            {
                Attack();
            }
            else
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if (abilityTime > 0)
            {
                abilityTime -= Time.deltaTime;
            }

            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }
        }

        if (state == EnemyState.SPECIAL)
        {
            float distance = Vector3.Distance(transform.position, fleePoint);
            if(distance <= navAgent.stoppingDistance)
            {
                UseAbilty();
            }
        }
    }

    void Attack()
    {
        attackTime = attackMaxTime;
        state = EnemyState.ATTACKING;
        frontAnimator.Play("Attack");
        backAnimator.Play("Attack");
    }

    void RunAway()
    {
        state = EnemyState.SPECIAL;
        ChooseRandomPoint();

        if (navAgent.enabled)
        {
            navAgent.SetDestination(fleePoint);
            if (navAgent.velocity.magnitude > 0)
            {
                facePlayer.SetDestination(fleePoint);
            }
            else
            {
                facePlayer.ResetDestination();
            }
        }
    }

    void ChooseRandomPoint()
    {
        int xDir = Random.Range(1, 3);
        int yDir = Random.Range(1, 3);
        float xPos = Random.Range(fleeRadiusMin, fleeRadiusMax);
        float zPos = Random.Range(fleeRadiusMin, fleeRadiusMax);
        Vector3 startPos = playerController.transform.position + new Vector3(xPos * Mathf.Pow(-1, xDir), 0, zPos * Mathf.Pow(-1, yDir));
        NavMeshHit hit;
        NavMesh.SamplePosition(startPos, out hit, fleeRadiusMax + 1, NavMesh.AllAreas);
        fleePoint = hit.position;
    }

    public void SwingSword(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        stepWithAttack.Step(0.15f);
        enemySound.SwordSwoosh();
    }

    public void SwooshShock()
    {
        parryWindow = false;

        if (!canHitPlayer)
        {
            enemySound.OtherSounds(1, 1);
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            enemySound.OtherSounds(0, 1);
            playerScript.LoseHealth(hitDamage, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerController.gameObject.layer == 8)
        {
            enemySound.OtherSounds(1, 1);
            playerController.PerfectDodge();
        }
    }

    void UseAbilty()
    {
        abilityTime = abilityMaxTime;
        state = EnemyState.ATTACKING;

        int randInt = Random.Range(0, 4);
        switch (randInt)
        {
            case 0:
                frontAnimator.Play("Beams");
                backAnimator.Play("Beams");
                break;
            case 1:
                frontAnimator.Play("Summon");
                backAnimator.Play("Summon");
                break;
            case 2:
                Charge();
                break;
            case 3: 
                frontAnimator.Play("Hadoken");
                backAnimator.Play("Hadoken");
                break;
        }
    }

    public void Hadoken()
    {
        Hadoken hadoken = Instantiate(hadokenPrefab).GetComponent<Hadoken>();
        int frontOrBack = facingFront ? 0 : 1;
        hadoken.transform.position = firePoints[frontOrBack].position;
        hadoken.direction = playerController.transform.position + new Vector3(0, 1, 0) - firePoints[frontOrBack].position;
        if (phase2)
        {
            foreach(float angle in hadokenAngles)
            {
                hadoken = Instantiate(hadokenPrefab).GetComponent<Hadoken>();
                hadoken.transform.position = firePoints[frontOrBack].position;
                hadoken.direction = playerController.transform.position + new Vector3(0, 1, 0) - firePoints[frontOrBack].position;
                hadoken.direction = hadoken.RotateByAngle(hadoken.direction, angle);
            }
        }
    }

    void Charge()
    {
        Vector3 playerDirection = playerController.transform.position - transform.position;
        playerDirection.y = 0;

        chargePath.Clear();
        chargeIndicators.Clear();
        Vector3 footPosition = new Vector3(transform.position.x, 0, transform.position.z);
        LayChargeIndicator(footPosition, playerDirection, maxChargeDistance, playerDirection);

        frontAnimator.Play("StartCharge");
        backAnimator.Play("StartCharge");
    }

    public void StartCharge()
    {
        charging = true;
        frontAnimator.SetBool("Charging", true);
        backAnimator.SetBool("Charging", true);
        enemyCollider.isTrigger = true;
        navAgent.enabled = false;
    }

    void Charging()
    {
        Vector3 footPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 chargeDirection = chargePath[0] - footPosition;
        transform.Translate(chargeDirection.normalized * Time.fixedDeltaTime * chargeSpeed);
        float moveDistance = Time.fixedDeltaTime * chargeSpeed;
        facePlayer.SetDestination(chargePath[0]);
        facePlayer.ManualFace();
        if (Vector3.Distance(footPosition, chargePath[0]) <= moveDistance)
        {
            chargePath.RemoveAt(0);
            if (chargePath.Count == 0)
            {
                StartCoroutine(EndCharge());
            }
        }
    }

    IEnumerator EndCharge()
    {
        enemyCollider.isTrigger = false;
        charging = false;
        state = EnemyState.IDLE;
        frontAnimator.SetBool("Charging", false);
        backAnimator.SetBool("Charging", false);
        yield return new WaitForSeconds(.2f);


        bool hitPlayer = false;
        foreach(ChargeIndicator indicator in chargeIndicators)
        {
            ParticleSystem particleSystem = indicator.gameObject.GetComponentInChildren<ParticleSystem>();
            particleSystem.Play();
            Vector3 centerPoint = (indicator.initialPosition + indicator.finalPosition) / 2;
            Quaternion direction = Quaternion.LookRotation(indicator.finalPosition - indicator.initialPosition);
            Vector3 halfExtents = new Vector3(indicator.indicatorWidth / 2, indicator.indicatorWidth / 2, Vector3.Distance(indicator.finalPosition, indicator.initialPosition) / 2);
            Collider[] hitColliders = Physics.OverlapBox(centerPoint, halfExtents, direction, playerMask, QueryTriggerInteraction.Ignore);
            if(hitColliders.Length > 0)
            {
                hitPlayer = true;
            }
        }

        if (hitPlayer)
        {
            enemySound.OtherSounds(0, 1);
            playerScript.LoseHealth(chargeBurstDamage);
            playerScript.StartStagger(chargeBurstStagger);
            playerScript.LosePoise(chargeBurstPoiseDamage);
        }
        else
        {
            enemySound.OtherSounds(1, 1);
        }

        yield return new WaitForSeconds(1);

        navAgent.enabled = true;
        facePlayer.ResetDestination();
    }

    void LayChargeIndicator(Vector3 initialPosition, Vector3 direction, float chargeDistance, Vector3 previousNormal)
    {
        RaycastHit hit;
        bool pathBlocked = Physics.Raycast(initialPosition, direction, out hit, chargeDistance, layerMask, QueryTriggerInteraction.Ignore);

        Vector3 finalPosition;

        if (!pathBlocked)
        {
            finalPosition = initialPosition + direction.normalized * chargeDistance;
            chargeDistance = 0;
        }
        else
        {
            finalPosition = initialPosition + direction.normalized * hit.distance;
            chargeDistance -= hit.distance;
        }

        chargePath.Add(finalPosition);

        ChargeIndicator indicator = Instantiate(chargeIndicator).GetComponent<ChargeIndicator>();
        indicator.transform.position = Vector3.zero;
        indicator.initialPosition = initialPosition;
        indicator.finalPosition = finalPosition;
        indicator.indicatorWidth = chargeIndicatorWidth;
        indicator.initialNormal = previousNormal;
        chargeIndicators.Add(indicator);

        DeathTimer deathTimer = indicator.GetComponent<DeathTimer>();
        deathTimer.timeToDie = maxChargeDistance / chargeSpeed + chargeDelay + 1f;

        if (chargeDistance > 0)
        {
            indicator.finalNormal = hit.normal;
            Vector3 newDirection = Vector3.Reflect(direction, hit.normal);
            LayChargeIndicator(finalPosition, newDirection, chargeDistance, hit.normal);
        }
        else
        {
            indicator.finalNormal = -direction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && charging && !isColliding)
        {
            isColliding = true;
            playerScript.LoseHealth(chargeDamage);
            enemySound.SwordImpact();
        }
        else if (other.gameObject.layer == 8 && charging && !isColliding)
        {
            isColliding = true;
            playerScript.GetComponent<PlayerController>().PerfectDodge();
        }
    }

    public override void StartStagger(float staggerDuration)
    {
        if (state == EnemyState.ATTACKING) return;

        base.StartStagger(staggerDuration);
    }

    public override void Death()
    {
        base.Death();

        mapData.electricBossKilled = true;
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

}
