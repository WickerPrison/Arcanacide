using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AssistantBolt : MonoBehaviour
{
    AssistantController controller;
    Transform origin;
    PlayerScript playerScript;
    NavMeshAgent navAgent;
    TouchingCollider touchingCollider;
    List<Collider> colliders;
    Bolts bolts;
    int damage;
    float dps = 50;
    float damageCounter = 0;
    [System.NonSerialized] public int pathfindingMethod;
    [SerializeField] PlayerData playerData;
    float offset = 5;


    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Assistant").GetComponent<AssistantController>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        navAgent = GetComponent<NavMeshAgent>();
        touchingCollider = GetComponentInChildren<TouchingCollider>();
        colliders = touchingCollider.GetTouchingObjects();
        bolts = GetComponentInChildren<Bolts>();
        origin = controller.boltOrigin;

        FindRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        bolts.SetPositions(transform.position, origin.position);

        navAgent.SetDestination(FindDestination());

        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player") && collider.gameObject.layer == 3)
            {
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
