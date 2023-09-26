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

        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        transform.position = new Vector3(xPos, 0, zPos);
    }

    // Update is called once per frame
    void Update()
    {
        bolts.SetPositions(transform.position, origin.position);

        navAgent.SetDestination(playerScript.transform.position);

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
