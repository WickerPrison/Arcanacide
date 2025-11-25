using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    public enum EnemyState
    {
        UNAWARE, IDLE, ATTACKING, STAGGERED, DYING, SPECIAL, DISABLED
    }

public class EnemyController : MonoBehaviour
{
    //This script controls the actions of the enemy units. It will likely be inherited by all enemy types

    [System.NonSerialized] public EnemyState state = EnemyState.UNAWARE;
    GameObject player;
    [System.NonSerialized] public GameManager gm;
    [System.NonSerialized] public Smear smearScript;
    [System.NonSerialized] public EnemyScript enemyScript;
    [System.NonSerialized] public EnemyEvents enemyEvents;
    [System.NonSerialized] public EnemySound enemySound;
    //[System.NonSerialized] public PlayerMovement playerMovement;
    [System.NonSerialized] public PlayerAbilities playerAbilities;
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
    [System.NonSerialized] public float playerDistance = 100;
    [System.NonSerialized] public bool directionLock = false;
    [System.NonSerialized] public bool isParrying = false;
    public int spellAttackDamage = 15;
    public int spellAttackPoiseDamage = 15;
    public int hitDamage;
    public float hitPoiseDamage;
    [System.NonSerialized] public bool facingFront;
    float staggerTimer = 0;
    LayerMask sightBlocker; 

    [System.NonSerialized] public bool canHitPlayer = false;

    float startDelay;

    public virtual void Awake()
    {
        enemyEvents = GetComponent<EnemyEvents>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        sightBlocker = LayerMask.GetMask("SightBlocker");
        enemyScript = GetComponent<EnemyScript>();
        enemySound = GetComponentInChildren<EnemySound>();
        smearScript = GetComponentInChildren<Smear>();
        //playerMovement = player.GetComponent<PlayerMovement>();
        playerAbilities = player.GetComponent<PlayerAbilities>();
        playerAnimation = player.GetComponent<PlayerAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
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
        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);

        if(startDelay > 0)
        {
            startDelay -= Time.deltaTime;
            return;
        }

        if (state == EnemyState.UNAWARE && playerDistance <= detectRange)
        {
            Debug.DrawLine(transform.position, player.transform.position, Color.red, 1000);
            if(!Physics.Linecast(transform.position, player.transform.position, sightBlocker))
            {
                state = EnemyState.IDLE;
                gm.awareEnemies += 1;
            }
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

    public virtual void StartStagger(float staggerDuration)
    {
        if (state == EnemyState.DYING)
        {
            return;
        }

        enemyEvents.Stagger();
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
        enemySound.SwordSwoosh();
        enemyEvents.Attack();
        if (!canHitPlayer)
        {
            return;
        }

        if (playerScript.gameObject.layer == 3)
        {
            enemySound.SwordImpact();
            playerScript.LoseHealth(hitDamage, EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(hitPoiseDamage);
            AdditionalAttackEffects();
        }
        else if(playerScript.gameObject.layer == 8)
        {
            playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
        }
    }

    public virtual void AdditionalAttackEffects()
    {

    }

    public virtual void StartDying()
    {
        enemyEvents.StartDying();
        frontAnimator.Play("Death");
        backAnimator.Play("Death");
    }

    public virtual void Death()
    {

    }

    public virtual void OnTakeDamage(object sender, System.EventArgs e)
    {

    }

    public virtual void OnLosePoise(object sender, System.EventArgs e)
    {
        
    }

    public virtual void EnableController()
    {
        state = EnemyState.IDLE;
        navAgent.enabled = true;
        directionLock = false;
        frontAnimator.Play("Idle");
        backAnimator.Play("Idle");
    }

    public virtual void DisableController()
    {
        if (state == EnemyState.DYING) return;
        if (state == EnemyState.UNAWARE)
        {
            gm.awareEnemies += 1;
        }
        state = EnemyState.DISABLED;
        navAgent.enabled = false;
        directionLock = true;
        frontAnimator.Play("Idle");
        backAnimator.Play("Idle");
    }

    private void Global_onEnemiesEnable(object sender, bool setActive)
    {
        if (!setActive)
        {
            DisableController();
        }
        else if (state == EnemyState.DISABLED)
        {
            EnableController();
        }
    }

    public virtual void OnEnable()
    {
        enemyEvents.OnTakeDamage += OnTakeDamage;
        enemyEvents.OnLosePoise += OnLosePoise;
        GlobalEvents.instance.onEnemiesEnable += Global_onEnemiesEnable;
    }

    public virtual void OnDisable()
    {
        enemyEvents.OnTakeDamage -= OnTakeDamage;
        enemyEvents.OnLosePoise -= OnLosePoise;
        GlobalEvents.instance.onEnemiesEnable -= Global_onEnemiesEnable;
    }
}