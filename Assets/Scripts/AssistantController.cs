using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class AssistantController : MonoBehaviour
{
    private enum AssistantState
    {
        DIALOGUE, IDLE, ATTACKING
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

    FinalBossEvents bossEvents;
    PlayerScript playerScript;
    AssistantState state = AssistantState.DIALOGUE;
    float moveSpeed = 4;
    float attackTime = 5;
    float attackTimer = 0;
    int beamsNum = 5;
    int boltsNum = 3;
    float skybeamDistance = 9f;
    LayerMask defaultMask;
    StudioEventEmitter sfx;
    [System.NonSerialized] public List<AssistantBolt> assistantBolts = new List<AssistantBolt>();

    public event System.EventHandler onEndBolts;

    private void Awake()
    {
        bossEvents = bossController.GetComponent<FinalBossEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        defaultMask = LayerMask.GetMask("Default");
        sfx = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        return;
        if (state == AssistantState.DIALOGUE) return;
        if(state == AssistantState.IDLE)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer < 0)
            {
                attackTimer = attackTime;

                int randInt = UnityEngine.Random.Range(0, 2 + bossController.phase);
                state = AssistantState.ATTACKING;
                randInt = 1;
                switch (randInt)
                {
                    case 0:
                        frontAnimator.Play("ThrowBombs");
                        break;
                    case 1:
                        frontAnimator.Play("Beams");
                        break;
                    case 2:
                        frontAnimator.Play("IceRings");
                        break;
                    case 3:
                        frontAnimator.Play("Bolts");
                        break;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Position();
    }

    void Position()
    {
        if (state == AssistantState.DIALOGUE) return;
        Vector3 direction = (bossController.transform.position - playerScript.transform.position).normalized;
        Vector3 destination = bossController.transform.position + 4 * direction;
        destination += Vector3.right * Mathf.Sin(Time.time / 2) * 2 + Vector3.forward * Mathf.Cos(Time.time / 2) * 2;
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.fixedDeltaTime);
    }

    public void CallAnimation(string animationName)
    {
        frontAnimator.Play(animationName);
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
        sfx.Play();
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
        sfx.Stop();
    }

    public void CallIceRings()
    {
        frontAnimator.Play("IceRings");
    }

    public void IceRings()
    {
        float randomAngle = UnityEngine.Random.Range(0, 360);
        for(int i = 0; i < 3; i++)
        {
            Vector3 direction = RotateDirection(Vector3.right, randomAngle + i * 120);

            RaycastHit hit;
            Physics.Raycast(bossController.transform.position + Vector3.up, direction, out hit, skybeamDistance, defaultMask);

            if (hit.collider == null || !hit.collider.CompareTag("Wall"))
            {
                Transform skybeam = Instantiate(ACPrefab).transform;
                skybeam.position = bossController.transform.position + direction * skybeamDistance;

            }

        }
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
        bossController.SetAttackTime(time);
    }

    private void onCombo(object sender, EventArgs e)
    {
        frontAnimator.Play("ThrowBombs");
    }

    Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
    }

    private void OnEnable()
    {
        bossEvents.assistantSitUp += assistantSitUp;
        bossEvents.endDialogue += endDialogue;
        bossEvents.freezeAssistant += freezeAssistant;
        bossEvents.onCombo += onCombo;
    }

    private void OnDisable()
    {
        bossEvents.assistantSitUp -= assistantSitUp;
        bossEvents.endDialogue -= endDialogue;
        bossEvents.freezeAssistant -= freezeAssistant;
        bossEvents.onCombo -= onCombo;
    }

    private void assistantSitUp(object sender, EventArgs e)
    {
        frontAnimator.Play("SitUp");
    }

    private void endDialogue(object sender, EventArgs e)
    {
        GetComponentInChildren<SortingGroup>().sortingOrder = 1;
        frontAnimator.Play("BecomeActive");
    }

    public void BecomeActive()
    {
        state = AssistantState.IDLE;
    }

    private void freezeAssistant(object sender, EventArgs e)
    {
        frontAnimator.Play("Idle");
        EndBolts();
        state = AssistantState.DIALOGUE;
    }
}
