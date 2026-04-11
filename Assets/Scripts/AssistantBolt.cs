using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AssistantBolt : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] int damage;
    [SerializeField] float poiseDamage;
    AssistantController controller;
    Transform origin;
    PlayerScript playerScript;
    Rigidbody rb;
    Bolts bolts;
    [System.NonSerialized] public int pathfindingMethod;
    [SerializeField] PlayerData playerData;
    bool launching = false;
    Vector3 launchDirection;
    [SerializeField] EventReference electricImpact;


    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Assistant").GetComponent<AssistantController>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        bolts = GetComponentInChildren<Bolts>();
        origin = controller.boltOrigin;
        rb = GetComponent<Rigidbody>();

        FindRandomPosition();
    }

    private void Update()
    {
        bolts.SetPositions(origin.position, transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (launching)
        {
            rb.velocity += launchDirection * acceleration;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript.HitPlayer(() =>
            {
                playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
                playerScript.LosePoise(poiseDamage);
                FmodUtils.PlayOneShot(electricImpact, 1);
                playerScript.StartStagger(0.2f);
            }, () =>
            {
                playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE);
            });
        }
    }

    void FindRandomPosition()
    {
        float xPos = Random.Range(-10, 10);
        float zPos = Random.Range(-10, 10);
        transform.position = new Vector3(xPos, 0, zPos);

        if (Vector3.Distance(transform.position, playerScript.transform.position) < 5)
        {
            FindRandomPosition();
        }
    }

    private void OnEnable()
    {
        controller.onEndBolts += onEndBolts;
        controller.onLaunchBolt += Controller_onLaunchBolt;
    }

    private void OnDisable()
    {
        controller.onEndBolts -= onEndBolts;
        controller.onLaunchBolt -= Controller_onLaunchBolt;
    }

    private void onEndBolts(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void Controller_onLaunchBolt(object sender, int index)
    {
        if(index == pathfindingMethod)
        {
            launchDirection = Vector3.Normalize(playerScript.transform.position - transform.position);
            launching = true;
            rb.velocity = Vector3.zero;
        }
    }
}
