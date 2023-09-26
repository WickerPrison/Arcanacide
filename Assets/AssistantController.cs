using System;
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
    [SerializeField] GameObject beamPrefab;
    [SerializeField] GameObject boltsPrefab;
    public Transform boltOrigin;

    NavMeshAgent navAgent;
    PlayerScript playerScript;
    AssistantState state = AssistantState.IDLE;
    float attackTime = 5;
    float attackTimer = 0;
    int beamsNum = 10;
    int boltsNum = 3;
    [System.NonSerialized] public List<AssistantBolt> assistantBolts = new List<AssistantBolt>();

    public event EventHandler onEndBolts;


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

                int randInt = UnityEngine.Random.Range(0, 3);
                randInt = 2;
                switch (randInt)
                {
                    case 0:
                        state = AssistantState.ATTACKING;
                        frontAnimator.Play("ThrowBombs");
                        break;
                    case 1:
                        state = AssistantState.ATTACKING;
                        frontAnimator.Play("Beams");
                        break;
                    case 2:
                        state = AssistantState.ATTACKING;
                        frontAnimator.Play("Bolts");
                        break;
                }
            }
        }
    }

    public void ThrowBomb(int hand)
    {
        ArcProjectile bomb = Instantiate(bombPrefab).GetComponent<ArcProjectile>();
        bomb.transform.position = frontArmbombs[hand].transform.position;
        bomb.endPoint = playerScript.transform.position;
    }

    public void SummonBeams()
    {
        for(int i = 0; i < beamsNum; i++)
        {
            Instantiate(beamPrefab);
        }
    }

    public void StartBolts()
    {
        assistantBolts.Clear();
        for (int i = 0; i < boltsNum; i++)
        {
            AssistantBolt bolt = Instantiate(boltsPrefab).GetComponent<AssistantBolt>();
            assistantBolts.Add(bolt);
            bolt.pathfindingMethod = i;
        }
    }

    public void EndBolts()
    {
        onEndBolts?.Invoke(this, EventArgs.Empty);
    }

    public void EndAttack()
    {
        state = AssistantState.IDLE;
        attackTimer = attackTime;
    }
}
