using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //This script controls the actions of the enemy units. It will likely be inherited by all enemy types

    [SerializeField] bool isMelee;
    public ParticleSystem frontSmear;
    public ParticleSystem backSmear;
    GameObject player;
    GameManager gm;
    TutorialManager tutorialManager;
    EnemyScript enemyScript;
    EnemySound enemySound;
    public PlayerController playerController;
    public PlayerScript playerScript;
    public PlayerAnimation playerAnimation;
    public Animator frontAnimator;
    public Animator backAnimator;
    public NavMeshAgent navAgent;
    public bool hasSeenPlayer = false;
    public float attackMaxTime = 2;
    public float attackTime;
    public float detectRange = 10f;
    public float attackRange;
    public bool attacking = false;
    public GameObject projectilePrefab;
    public Transform frontAttackPoint;
    public Transform backAttackPoint;
    public bool charging = false;
    public bool detectionTrigger = false;
    public bool directionLock = false;
    public bool parryWindow = false;
    public bool isParrying = false;
    public int hitDamage;
    public float hitPoiseDamage;
    bool facingFront;

    public bool canHitPlayer = false;

    Vector3 frontAnimatorPosition;
    Vector3 backAnimatorPosition;
    public Vector3 away = new Vector3(100, 100, 100);
    Rigidbody rb;
    float scaleX;

    float startDelay;

    Vector3 frontSmearScale;
    Vector3 frontSmearRotation;
    Vector3 frontSmearPosition;
    Vector3 backSmearScale;
    Vector3 backSmearRotation;
    Vector3 backSmearPosition;


    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyScript = gameObject.GetComponent<EnemyScript>();
        enemySound = GetComponentInChildren<EnemySound>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerScript = player.GetComponent<PlayerScript>();
        playerAnimation = player.GetComponent<PlayerAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        tutorialManager = gm.gameObject.GetComponent<TutorialManager>();
        scaleX = frontAnimator.transform.localScale.x;
        frontAnimatorPosition = frontAnimator.transform.localPosition;
        backAnimatorPosition = backAnimator.transform.localPosition;
        startDelay = Random.Range(0.4f, 1);

        if (isMelee)
        {
            frontSmearScale = frontSmear.transform.localScale;
            frontSmearRotation = new Vector3(90, -20, 0);
            frontSmearPosition = new Vector3(-0.17f, -0.3f, 0.17f);
            backSmearScale = backSmear.transform.localScale;
            backSmearRotation = new Vector3(-90, 70, 0);
            backSmearPosition = new Vector3(0.17f, -0.3f, 0.17f);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        EnemyAI();

        frontAnimator.SetFloat("Velocity", navAgent.velocity.magnitude);
        backAnimator.SetFloat("Velocity", navAgent.velocity.magnitude);

        if (navAgent.enabled && !directionLock)
        {
            FacePlayer();
        }
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
        projectile.transform.LookAt(playerController.transform.position);
        projectileScript.target = playerController.transform;
    }

    public virtual void SpecialAbility()
    {

    }

    public virtual void OnHit()
    {

    }

    public virtual void Stagger()
    {
        frontAnimator.Play("Stagger");
        backAnimator.Play("Stagger");
    }

    public virtual void AttackHit(int smearSpeed)
    {
        parryWindow = false;
        ParticleSystem.ShapeModule frontSmearShape = frontSmear.shape;
        ParticleSystem.ShapeModule backSmearShape = backSmear.shape;
        frontSmearShape.arcSpeed = smearSpeed;
        backSmearShape.arcSpeed = -smearSpeed;
        frontSmear.Play();
        backSmear.Play();

        if (!canHitPlayer)
        {
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            if (playerAnimation.parryWindow)
            {
                playerAnimation.isParrying = true;
                playerController.Parry(enemyScript);
            }
            else if (isParrying)
            {
                isParrying = false;
                playerController.Parry(enemyScript);
            }
            else
            {
                enemySound.SwordImpact();
                playerScript.LoseHealth(hitDamage);
                playerScript.LosePoise(hitPoiseDamage);
            }
        }
        else if(playerController.gameObject.layer == 8)
        {
            playerController.PathOfTheSword();
        }
    }

    public virtual void AttackPoint()
    {
        Vector3 direction = playerController.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        frontAttackPoint.position = transform.position + direction.normalized;
        frontAttackPoint.transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    void FacePlayer()
    {
        if(playerController.transform.position.z < transform.position.z)
        {
            facingFront = true;
            if (playerController.transform.position.x > transform.position.x)
            {
                FrontRight();
            }
            else
            {
                FrontLeft();
            }
        }
        else
        {
            facingFront = false;
            if (playerController.transform.position.x > transform.position.x)
            {
                BackRight();
            }
            else
            {
                BackLeft();
            }
        }
    }

    void FrontRight()
    {
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(scaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
    }

    void FrontLeft()
    {
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = new Vector3(-frontAnimatorPosition.x, frontAnimatorPosition.y, frontAnimatorPosition.z);
        frontAnimator.transform.localScale = new Vector3(-scaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
    }

    void BackRight()
    {
        backAnimator.transform.localPosition = backAnimatorPosition;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localScale = new Vector3(scaleX, backAnimator.transform.localScale.y, backAnimator.transform.localScale.z);
    }

    void BackLeft()
    {
        backAnimator.transform.localPosition = new Vector3(-backAnimatorPosition.x, backAnimatorPosition.y, backAnimatorPosition.z);
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localScale = new Vector3(-scaleX, backAnimator.transform.localScale.y, backAnimator.transform.localScale.z);
    }

    public void SmearDirection()
    {
        if (playerController.transform.position.z < transform.position.z)
        {
            if (playerController.transform.position.x > transform.position.x)
            {
                backSmear.transform.position = away;
                frontSmear.transform.localScale = frontSmearScale;
                frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, frontSmearRotation.y, frontSmearRotation.z);
                frontSmear.transform.localPosition = frontSmearPosition;
            }
            else
            {
                backSmear.transform.position = away;
                frontSmear.transform.localScale = new Vector3(-frontSmearScale.x, frontSmearScale.y, frontSmearScale.z);
                frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, -frontSmearRotation.y, frontSmearRotation.z);
                frontSmear.transform.localPosition = new Vector3(-frontSmearPosition.x, frontSmearPosition.y, frontSmearPosition.z);
            }
        }
        else
        {
            if (playerController.transform.position.x < transform.position.x)
            {
                frontSmear.transform.position = away;
                backSmear.transform.localScale = backSmearScale;
                backSmear.transform.localRotation = Quaternion.Euler(backSmearRotation.x, -backSmearRotation.y, backSmearRotation.z);
                backSmear.transform.localPosition = new Vector3(-backSmearPosition.x, backSmearPosition.y, backSmearPosition.z);
            }
            else
            {
                frontSmear.transform.position = away;
                backSmear.transform.localScale = new Vector3(-backSmearScale.x, backSmearScale.y, backSmearScale.z);
                backSmear.transform.localRotation = Quaternion.Euler(backSmearRotation.x, backSmearRotation.y, backSmearRotation.z);
                backSmear.transform.localPosition = backSmearPosition;
            }
        }
    }
}
/*
backAnimator.transform.localPosition = away;
frontAnimator.transform.localPosition = frontAnimatorPosition;
frontAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
frontAnimator.transform.localPosition = new Vector3(frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
backAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
backAnimator.transform.localPosition = new Vector3(backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
frontSmear.transform.localScale = frontSmearScale;
frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, frontSmearRotation.y, frontSmearRotation.z);
frontSmear.transform.localPosition = frontSmearPosition;
backSmear.transform.position = away;
*/