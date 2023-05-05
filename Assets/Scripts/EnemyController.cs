using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

    public enum EnemyState
    {
        UNAWARE, IDLE, ATTACKING, STAGGERED, DYING, SPECIAL, Disabled
    }

public class EnemyController : MonoBehaviour
{
    //This script controls the actions of the enemy units. It will likely be inherited by all enemy types

    [System.NonSerialized] public EnemyState state = EnemyState.UNAWARE;
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
    public float attackMaxTime = 2;
    [System.NonSerialized] public float attackTime;
    public float detectRange = 10f;
    public float attackRange;
    public GameObject projectilePrefab;
    [System.NonSerialized] public float playerDistance;
    [System.NonSerialized] public bool directionLock = false;
    [System.NonSerialized] public bool parryWindow = false;
    [System.NonSerialized] public bool isParrying = false;
    public int spellAttackDamage = 15;
    public int spellAttackPoiseDamage = 15;
    public int hitDamage;
    public float hitPoiseDamage;
    [System.NonSerialized] public bool facingFront;
    float staggerTimer = 0;

    [System.NonSerialized] public bool canHitPlayer = false;

    float startDelay;

    public virtual void Awake()
    {
        enemyScript = gameObject.GetComponent<EnemyScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemySound = GetComponentInChildren<EnemySound>();
        smearScript = GetComponentInChildren<Smear>();
        playerController = player.GetComponent<PlayerController>();
        playerAnimation = player.GetComponent<PlayerAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        startDelay = Random.Range(0.4f, 1);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (state == EnemyState.STAGGERED)
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

        playerDistance = Vector3.Distance(transform.position, playerController.transform.position);

        if (state == EnemyState.UNAWARE && playerDistance <= detectRange)
        {
            state = EnemyState.IDLE;
            gm.awareEnemies += 1;
        }
    }

    public virtual void SpecialEffect()
    {

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
        if (state == EnemyState.DYING)
        {
            return;
        }

        staggerTimer += staggerDuration;
        state = EnemyState.STAGGERED;
        navAgent.enabled = false;
        directionLock = true;
        frontAnimator.Play("Stagger");
        backAnimator.Play("Stagger");
    }

    public virtual void EndStagger()
    {
        state = EnemyState.IDLE;
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