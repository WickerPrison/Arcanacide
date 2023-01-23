using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class EnemyController : MonoBehaviour
{
    //This script controls the actions of the enemy units. It will likely be inherited by all enemy types

    GameObject player;
    [System.NonSerialized] public GameManager gm;
    [System.NonSerialized] public Smear smearScript;
    [System.NonSerialized] public EnemyScript enemyScript;
    [System.NonSerialized] public EnemySound enemySound;
    [System.NonSerialized] public PlayerController playerController;
    [System.NonSerialized] public PlayerScript playerScript;
    [System.NonSerialized] public PlayerAnimation playerAnimation;
    public Animator frontAnimator;
    public Animator backAnimator;
    public NavMeshAgent navAgent;
    [System.NonSerialized] public bool hasSeenPlayer = false;
    public float attackMaxTime = 2;
    [System.NonSerialized] public float attackTime;
    public float detectRange = 10f;
    public float attackRange;
    [System.NonSerialized] public bool attacking = false;
    public GameObject projectilePrefab;
    [System.NonSerialized] public bool detectionTrigger = false;
    [System.NonSerialized] public bool directionLock = false;
    [System.NonSerialized] public bool parryWindow = false;
    [System.NonSerialized] public bool isParrying = false;
    public int spellAttackDamage = 15;
    public int spellAttackPoiseDamage = 15;
    public int hitDamage;
    public float hitPoiseDamage;
    [System.NonSerialized] public bool facingFront;
    float staggerTimer = 0;
    [System.NonSerialized] public bool isStaggered = false;
    [System.NonSerialized] public bool isDisabled;

    [System.NonSerialized] public bool canHitPlayer = false;

    float startDelay;


    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyScript = gameObject.GetComponent<EnemyScript>();
        enemySound = GetComponentInChildren<EnemySound>();
        smearScript = GetComponentInChildren<Smear>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerScript = player.GetComponent<PlayerScript>();
        playerAnimation = player.GetComponent<PlayerAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        startDelay = Random.Range(0.4f, 1);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (enemyScript.isDying || isDisabled)
        {
            return;
        }

        if (isStaggered)
        {
            staggerTimer -= Time.deltaTime;
            if (staggerTimer <= 0)
            {
                EndStagger();
            }
        }
        else
        {
            EnemyAI();
        }

        frontAnimator.SetFloat("Velocity", navAgent.velocity.magnitude);
        backAnimator.SetFloat("Velocity", navAgent.velocity.magnitude);
    }

    //This virtual function will likely be overridden by all specific enemy types
    //it is called in the Update function and is responsible for controlling the enemies
    // actions from moment to moment
    public virtual void EnemyAI()
    {
        if(startDelay > 0)
        {
            startDelay -= Time.deltaTime;
            return;
        }

        if (!detectionTrigger && Vector3.Distance(transform.position, playerController.transform.position) <= detectRange)
        {
            detectionTrigger = true;
            hasSeenPlayer = true;
            gm.awareEnemies += 1;
        }
    }

    public virtual void SpellAttack()
    {

    }

    public virtual void SpecialAbility()
    {

    }

    public virtual void SpecialAbilityOff()
    {

    }

    public virtual void OnHit()
    {
        ElectricAlly ally = GetComponent<ElectricAlly>();
        if (ally != null)
        {
            ally.OnHit();
        }
    }

    public virtual void StartStagger(float staggerDuration)
    {
        staggerTimer += staggerDuration;
        isStaggered = true;
        navAgent.enabled = false;
        attacking = false;
        directionLock = true;
        frontAnimator.Play("Stagger");
        backAnimator.Play("Stagger");
    }

    public virtual void EndStagger()
    {
        isStaggered = false;
        navAgent.enabled = true;
        directionLock = false;
        frontAnimator.Play("Idle");
        backAnimator.Play("Idle");
    }

    public virtual void AttackHit(int smearSpeed)
    {
        parryWindow = false;
        enemySound.SwordSwoosh();

        if (!canHitPlayer)
        {
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            enemySound.SwordImpact();
            playerScript.LoseHealth(hitDamage, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if(playerController.gameObject.layer == 8)
        {
            playerController.PerfectDodge();
        }
    }

    public virtual void AdditionalAttackEffects()
    {

    }

    public virtual void StartDying()
    {
        frontAnimator.Play("Death");
        backAnimator.Play("Death");
    }

    public virtual void Death()
    {

    }
}