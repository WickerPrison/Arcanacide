using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ElectricBossController : EnemyController, IEndDialogue
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
    public int chargeDamage;
    public int chargeBurstDamage;
    [SerializeField] float chargeBurstPoiseDamage;
    [SerializeField] float chargeBurstStagger;
    bool isColliding;
    [SerializeField] float[] hadokenAngles;
    public bool phase2 = false;
    int phaseTrigger;
    [SerializeField] ParticleSystem bodyLightning;
    public float abilityTime;
    float abilityMaxTime = 5;
    MusicManager musicManager;
    int healthPercent;
    int livingFriends;
    [System.NonSerialized] public float friendshipPower;

    public override void Awake()
    {
        base.Awake();
        enemyScript = GetComponent<EnemyScript>();
        livingFriends = 3 - mapData.carolsDeadFriends.Count;
        friendshipPower = 1 + livingFriends / 3f;
        enemyScript.maxHealth = Mathf.RoundToInt(enemyScript.maxHealth * friendshipPower);
        enemyScript.health = enemyScript.maxHealth;
        phaseTrigger = Mathf.RoundToInt(enemyScript.maxHealth / 2);
    }

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
        musicManager = gm.GetComponentInChildren<MusicManager>();
        if (mapData.electricBossKilled)
        {
            enemyEvents.HideBossHealthbar();
            musicManager.ChangeMusicState(MusicState.MAINLOOP);
            gm.enemies.Remove(enemyScript);
            Destroy(gameObject);
            return;
        }

        gm.awareEnemies += 1;
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
        healthPercent = Mathf.RoundToInt(enemyScript.health / enemyScript.maxHealth * 100);
        musicManager.UpdateBossHealth(healthPercent);
        if(!phase2 && enemyScript.health < phaseTrigger)
        {
            phase2 = true;
            bodyLightning.Play();
        }

        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);

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
            else if(navAgent.enabled)
            {
                navAgent.SetDestination(playerScript.transform.position);
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
        else if (state == EnemyState.SPECIAL)
        {
            float distance = Vector3.Distance(transform.position, fleePoint);
            if(distance <= navAgent.stoppingDistance)
            {
                UseAbilty();
            }
        }
    }

    public void Attack()
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
        Vector3 startPos = playerScript.transform.position + new Vector3(xPos * Mathf.Pow(-1, xDir), 0, zPos * Mathf.Pow(-1, yDir));
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
        if (!canHitPlayer)
        {
            enemySound.OtherSounds(1, 1);
            return;
        }

        if (playerScript.gameObject.layer == 3)
        {
            enemySound.OtherSounds(0, 2);
            playerScript.LoseHealth(Mathf.RoundToInt(hitDamage * friendshipPower), EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if (playerScript.gameObject.layer == 8)
        {
            enemySound.OtherSounds(1, 1);
            playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
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
                StartBeams();
                break;
            case 1:
                StartSummon();
                break;
            case 2:
                Charge();
                break;
            case 3:
                StartHadoken();
                break;
        }
    }

    public void StartBeams()
    {
        frontAnimator.Play("Beams");
        backAnimator.Play("Beams");
    }

    public void StartSummon()
    {
        frontAnimator.Play("Summon");
        backAnimator.Play("Summon");
    }

    public void StartHadoken()
    {
        frontAnimator.Play("Hadoken");
        backAnimator.Play("Hadoken");
    }

    public void Hadoken()
    {
        Hadoken hadoken = Instantiate(hadokenPrefab).GetComponent<Hadoken>();
        int frontOrBack = facingFront ? 0 : 1;
        hadoken.transform.position = firePoints[frontOrBack].position;
        hadoken.direction = playerScript.transform.position + new Vector3(0, 1, 0) - firePoints[frontOrBack].position;
        hadoken.spellDamage = Mathf.RoundToInt(hadoken.spellDamage * friendshipPower);
        hadoken.friendshipPower = friendshipPower;
        if (phase2)
        {
            foreach(float angle in hadokenAngles)
            {
                hadoken = Instantiate(hadokenPrefab).GetComponent<Hadoken>();
                hadoken.transform.position = firePoints[frontOrBack].position;
                hadoken.direction = playerScript.transform.position + new Vector3(0, 1, 0) - firePoints[frontOrBack].position;
                hadoken.direction = hadoken.RotateByAngle(hadoken.direction, angle);
                hadoken.spellDamage = Mathf.RoundToInt(hadoken.spellDamage * friendshipPower);
                hadoken.friendshipPower = friendshipPower;
            }
        }
    }

    public void Charge()
    {
        Vector3 playerDirection = playerScript.transform.position - transform.position;
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
            playerScript.LoseHealth(Mathf.RoundToInt(friendshipPower * chargeBurstDamage), EnemyAttackType.NONPARRIABLE, null);
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
            playerScript.LoseHealth(Mathf.RoundToInt(friendshipPower * chargeDamage), EnemyAttackType.NONPARRIABLE, null);
            enemySound.SwordImpact();
        }
        else if (other.gameObject.layer == 8 && charging && !isColliding)
        {
            isColliding = true;
            playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE);
        }
    }

    public override void StartStagger(float staggerDuration)
    {
        if (state == EnemyState.ATTACKING) return;

        base.StartStagger(staggerDuration);
    }

    public void EndDialogue()
    { 
        state = EnemyState.IDLE;
        musicManager.ChangeMusicState(MusicState.BOSSMUSIC);
    }

    public override void Death()
    {
        base.Death();

        mapData.electricBossKilled = true;
        gm.awareEnemies -= 1;
        GlobalEvents.instance.BossKilled();
    }

}
