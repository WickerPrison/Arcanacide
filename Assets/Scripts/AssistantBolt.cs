using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AssistantBolt : MonoBehaviour
{
    AssistantController controller;
    Transform origin;
    PlayerScript playerScript;
    PlayerMovement playerMovement;
    NavMeshAgent navAgent;
    TouchingCollider touchingCollider;
    Rigidbody rb;
    List<Collider> colliders;
    Bolts bolts;
    int damage;
    float dps = 50;
    float damageCounter = 0;
    [System.NonSerialized] public int pathfindingMethod;
    [SerializeField] PlayerData playerData;
    float offset = 5;
    bool hittingPlayer = false;


    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Assistant").GetComponent<AssistantController>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerMovement = playerScript.GetComponent<PlayerMovement>();
        navAgent = GetComponent<NavMeshAgent>();
        touchingCollider = GetComponentInChildren<TouchingCollider>();
        colliders = touchingCollider.GetTouchingObjects();
        bolts = GetComponentInChildren<Bolts>();
        origin = controller.boltOrigin;
        rb = GetComponent<Rigidbody>();

        FindRandomPosition();
    }

    private void Update()
    {
        bolts.SetPositions(origin.position, transform.position);

        Debug.Log(playerMovement.lastMoveDir);
        Vector3 destination = playerScript.transform.position;
        if(pathfindingMethod == 1)
        {
            destination += playerMovement.lastMoveDir * 3;
        }
        else if(pathfindingMethod == 2)
        {
            destination -= playerMovement.lastMoveDir * 3;
        }

        Vector3 direction = destination - transform.position;
        rb.velocity += direction.normalized + Vector3.right * 0.1f;
        rb.velocity = rb.velocity.normalized * 8;
        //navAgent.SetDestination(FindDestination());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hittingPlayer = false;
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player") && collider.gameObject.layer == 3)
            {
                hittingPlayer = true;
                bolts.SoundOn();
                damageCounter += dps * Time.deltaTime;
                if(damageCounter > 1)
                {
                    damage = Mathf.FloorToInt(damageCounter);
                    playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
                    playerScript.LosePoise(damage);
                    damageCounter -= damage;
                }
            }
        }

        if (!hittingPlayer)
        {
            bolts.SoundOff();
        }
    }

    Vector3 FindDestination()
    {
        Vector3 predictDir;
        switch (pathfindingMethod)
        {
            case 0:
                return playerScript.transform.position;
            case 1:
                predictDir = new Vector3(playerData.moveDir.x, 0, playerData.moveDir.y).normalized * offset;
                return playerScript.transform.position + predictDir;
            case 2:
                predictDir = new Vector3(playerData.moveDir.x, 0, playerData.moveDir.y).normalized * -offset;
                return playerScript.transform.position + predictDir;
            default:
                return playerScript.transform.position;
        }
    }

    void FindRandomPosition()
    {
        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        transform.position = new Vector3(xPos, 0, zPos);

        if(Vector3.Distance(transform.position, playerScript.transform.position) < 5)
        {
            FindRandomPosition();
        }
    }

    private void OnEnable()
    {
        controller.onEndBolts += onEndBolts;
    }

    private void OnDisable()
    {
        controller.onEndBolts -= onEndBolts;
    }

    private void onEndBolts(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
}
