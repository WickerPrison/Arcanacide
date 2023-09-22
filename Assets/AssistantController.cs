using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AssistantController : MonoBehaviour
{
    private enum AssistantState
    {
        IDLE, ATTACKING
    }

    [SerializeField] Animator frontAnimator;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Transform[] frontArmbombs;

    NavMeshAgent navAgent;
    PlayerScript playerScript;
    AssistantState state = AssistantState.IDLE;
    float attackTime = 5;
    float attackTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == AssistantState.IDLE)
        {
            navAgent.SetDestination(playerScript.transform.position);
            attackTimer -= Time.deltaTime;
            if(attackTimer < 0)
            {
                attackTimer = attackTime;
                state = AssistantState.ATTACKING;
                frontAnimator.Play("ThrowBombs");
            }
        }
    }

    public void ThrowBomb(int hand)
    {
        ArcProjectile bomb = Instantiate(bombPrefab).GetComponent<ArcProjectile>();
        bomb.transform.position = frontArmbombs[hand].transform.position;
        bomb.endPoint = playerScript.transform.position;
    }

    public void EndAttack()
    {
        state = AssistantState.IDLE;
        attackTimer = attackTime;
    }
}
