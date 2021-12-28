using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //This script controls the actions of the enemy units. It will likely be inherited by all enemy types

    GameObject player;
    GameManager gm;
    public PlayerController playerController;
    public PlayerScript playerScript;
    public PlayerAnimation playerAnimation;
    public Animator frontAnimator;
    public NavMeshAgent navAgent;
    public bool hasSeenPlayer = false;
    public float attackMaxTime = 2;
    public float attackTime;
    public float detectRange = 10f;
    public float attackRange;
    public bool attacking = false;
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public bool charging = false;
    public bool detectionTrigger = false;

    Rigidbody rb;
    float scaleX;
    float offsetX;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerScript = player.GetComponent<PlayerScript>();
        playerAnimation = player.GetComponent<PlayerAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        scaleX = frontAnimator.transform.localScale.x;
        offsetX = frontAnimator.transform.localPosition.x;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        EnemyAI();

        //the enemy currently only has a front animator, that will change eventually
        frontAnimator.SetFloat("Velocity", navAgent.velocity.magnitude);

        if (navAgent.enabled)
        {
            FacePlayer();
        }
    }

    //This virtual function will likely be overridden by all specific enemy types
    //it is called in the Update function and is responsible for controlling the enemies
    // actions from moment to moment
    public virtual void EnemyAI()
    {
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
        projectile.transform.position = attackPoint.position;
        projectile.transform.LookAt(playerController.transform.position);
        projectileScript.target = playerController.transform;
    }

    public virtual void SpecialAbility()
    {

    }

    void FacePlayer()
    {
        if(playerController.transform.position.x > transform.position.x)
        {
            frontAnimator.transform.localScale = new Vector3(scaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
            frontAnimator.transform.localPosition = new Vector3(offsetX, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        }
        else
        {
            frontAnimator.transform.localScale = new Vector3(-scaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
            frontAnimator.transform.localPosition = new Vector3(-offsetX, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        }
    }

    public virtual void OnHit()
    {

    }

    public virtual void AttackHit(int smearSpeed)
    {

    }

    public virtual bool SwordClash()
    {
        return false;
    }
}
