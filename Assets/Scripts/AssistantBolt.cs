using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AssistantBolt : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float acceleration;
    AssistantController controller;
    Transform origin;
    PlayerScript playerScript;
    PlayerMovement playerMovement;
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
    float moveSpeed = 6;
    bool launching = false;
    Vector3 launchDirection;


    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Assistant").GetComponent<AssistantController>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerMovement = playerScript.GetComponent<PlayerMovement>();
        touchingCollider = GetComponentInChildren<TouchingCollider>();
        colliders = touchingCollider.GetTouchingObjects();
        bolts = GetComponentInChildren<Bolts>();
        origin = controller.boltOrigin;
        rb = GetComponent<Rigidbody>();

        transform.position = CirclePosition();
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
        else
        {
            transform.position = CirclePosition();
        }

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

    Vector3 CirclePosition()
    {
        Vector3 direction = Utils.RotateDirection(Vector3.right, Time.time * rotateSpeed + 120 * pathfindingMethod).normalized;
        Vector3 position = playerScript.transform.position + direction * 3f;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(position, out hit, 3f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
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
            Debug.Log("launch bolt");
            launchDirection = Vector3.Normalize(playerScript.transform.position - transform.position);
            launching = true;
            rb.velocity = Vector3.zero;
        }
    }
}
