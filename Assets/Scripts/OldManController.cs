using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OldManController : EnemyController
{
    [SerializeField] GameObject chargeIndicator;
    [SerializeField] List<string> layerNames = new List<string>();
    float chargeIndicatorWidth;
    [SerializeField] float maxChargeDistance;
    [SerializeField] Collider attackPointCollider;
    float playerDistance;
    LayerMask layerMask;
    List<Vector3> chargePath = new List<Vector3>();
    bool charging = false;
    [SerializeField] float chargeSpeed;
    float chargeDelay = 0.7f;
    CapsuleCollider enemyCollider;
    [SerializeField] int chargeDamage;
    bool isColliding;
    [SerializeField] ParticleSystem staticVFX;

    public override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Default");
        enemyCollider = GetComponent<CapsuleCollider>();
        chargeIndicatorWidth = enemyCollider.radius * 2;
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        playerDistance = Vector3.Distance(transform.position, playerController.transform.position);

        if (hasSeenPlayer)
        { 
            if(attackTime <= 0 && !charging)
            {
                attacking = true;
                int randNum = Random.Range(1,3);
                if(randNum == 1)
                {
                    attackTime = attackMaxTime;
                    frontAnimator.Play("TeleportAway");
                    backAnimator.Play("TeleportAway");
                }
                else if(randNum == 2)
                {
                    attackTime = attackMaxTime;
                    Charge();
                }
            }
        }

        if(attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public override void Update()
    {
        base.Update();

        isColliding = false;
    }

    private void FixedUpdate()
    {
        if (charging)
        {
            Charging();
        }
    }

    public override void SpecialAbility()
    {
        float yDirection = Random.Range(-1f, 1f);
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        navAgent.Warp(playerController.transform.position + direction.normalized * 1.5f);
    }

    void Charge()
    { 
        Vector3 playerDirection = playerController.transform.position - transform.position;

        chargePath.Clear();
        Vector3 footPosition = new Vector3(transform.position.x, 0, transform.position.z);
        LayChargeIndicator(footPosition, playerDirection, maxChargeDistance, playerDirection);

        frontAnimator.Play("StartCharge");
        backAnimator.Play("StartCharge");
    }

    public override void SpellAttack()
    {
        charging = true;
        frontAnimator.SetBool("Charging", true);
        backAnimator.SetBool("Charging", true);
        enemyCollider.isTrigger = true;        
    }

    void Charging()
    {
        attackPointCollider.enabled = false;
        Vector3 footPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 chargeDirection = chargePath[0] - footPosition;
        transform.Translate(chargeDirection.normalized * Time.fixedDeltaTime * chargeSpeed);
        float moveDistance = Time.fixedDeltaTime * chargeSpeed;
        if(Vector3.Distance(footPosition, chargePath[0]) <= moveDistance)
        {
            chargePath.RemoveAt(0);
            if(chargePath.Count == 0)
            {
                attackPointCollider.enabled = true;
                enemyCollider.isTrigger = false;
                charging = false;
                attacking = false;
                frontAnimator.SetBool("Charging", false);
                backAnimator.SetBool("Charging", false);
                attackTime = attackMaxTime;
            }
        }
    }

    void LayChargeIndicator(Vector3 initialPosition, Vector3 direction, float chargeDistance, Vector3 previousNormal)
    {
        RaycastHit hit;
        bool pathBlocked = Physics.Raycast(initialPosition, direction, out hit, chargeDistance, layerMask);

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

        DeathTimer deathTimer = indicator.GetComponent<DeathTimer>();
        deathTimer.timeToDie = maxChargeDistance / chargeSpeed + chargeDelay + .2f;

        if(chargeDistance > 0)
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
        if(other.gameObject.layer == 3 && charging && !isColliding)
        {
            isColliding = true;
            playerScript.LoseHealth(chargeDamage);
        }
    }

    public override void Death()
    {
        staticVFX.Stop();
        base.Death();
    }
}