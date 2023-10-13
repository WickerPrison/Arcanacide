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
    [SerializeField] GameObject ACPrefab;
    [SerializeField] Transform ACorigin;
    [SerializeField] ChaosBossController bossController;

    NavMeshAgent navAgent;
    PlayerScript playerScript;
    AssistantState state = AssistantState.IDLE;
    float attackTime = 5;
    float attackTimer = 0;
    int beamsNum = 5;
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
        navAgent.SetDestination(playerScript.transform.position);

        if(state == AssistantState.IDLE)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer < 0)
            {
                attackTimer = attackTime;

                int randInt = UnityEngine.Random.Range(0, 2 + bossController.phase);
                randInt = 1;
                state = AssistantState.ATTACKING;
                switch (randInt)
                {
                    case 0:
                        frontAnimator.Play("ThrowBombs");
                        break;
                    case 1:
                        frontAnimator.Play("Beams");
                        break;
                    case 2:
                        if (bossController.phase == 1)
                            frontAnimator.Play("IceRing");
                        else
                            frontAnimator.Play("IceRing2");
                        break;
                    case 3:
                        frontAnimator.Play("Bolts");
                        break;
                }
            }
        }
    }

    public void ThrowBomb(int hand)
    {
        AssistantBomb bomb = Instantiate(bombPrefab).GetComponent<AssistantBomb>();
        bomb.transform.position = frontArmbombs[hand].transform.position;
        bomb.endPoint = playerScript.transform.position;
        bomb.phase = bossController.phase;
    }

    public void SummonBeams()
    {
        
        for(int i = 0; i < beamsNum * bossController.phase; i++)
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

    public void ThrowAC()
    {
        ArcProjectile ac = Instantiate(ACPrefab).GetComponent<ArcProjectile>();
        ac.transform.position = ACorigin.position;
        ac.endPoint = playerScript.transform.position;
    }

    public void EndAttack(float time)
    {
        state = AssistantState.IDLE;
        attackTimer = time;
    }
}
