using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //This script controls the actions of the enemy units. It will likely be inherited by all enemy types

    public PlayerController playerController;
    public Animator frontAnimator;
    public NavMeshAgent navAgent;
    public bool hasSeenPlayer = false;
    public float attackMaxTime = 2;
    public float attackTime;
    public float detectRange = 10f;
    public float spellRange = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform attackPoint;

    Rigidbody rb;
    float scaleX;
    float offsetX;

    // Start is called before the first frame update
    public virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        scaleX = frontAnimator.transform.localScale.x;
        offsetX = frontAnimator.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAI();

        //the enemy currently only has a front animator, that will change eventually
        frontAnimator.SetFloat("Velocity", navAgent.velocity.magnitude);

        FacePlayer();
    }

    //This virtual function will likely be overridden by all specific enemy types
    //it is called in the Update function and is responsible for controlling the enemies
    // actions from moment to moment
    public virtual void EnemyAI()
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) <= detectRange)
        {
            hasSeenPlayer = true;
        }

        if (hasSeenPlayer)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            if (Vector3.Distance(transform.position, playerController.transform.position) < spellRange)
            {
                if (attackTime <= 0)
                {
                    frontAnimator.SetTrigger("SpellAttack");
                    attackTime = attackMaxTime;
                }
            }

        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    public void SpellAttack()
    {
        GameObject projectile;
        Projectile projectileScript;
        projectile = Instantiate(projectilePrefab);
        projectileScript = projectile.GetComponent<Projectile>();
        projectile.transform.position = attackPoint.position;
        projectile.transform.LookAt(playerController.transform.position + new Vector3(0, -0.5f,0));
        projectileScript.direction = playerController.transform.position + new Vector3(0, -0.5f, 0) - projectile.transform.position;
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
}
