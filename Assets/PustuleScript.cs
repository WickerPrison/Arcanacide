using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PustuleScript : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float poiseDamage;
    [SerializeField] float pulseDamageRate;
    [SerializeField] float pulseHealRate;
    [SerializeField] SpriteRenderer airPustuleSprite;
    [SerializeField] SpriteRenderer groundPustuleSprite;
    [SerializeField] SpriteRenderer pulseEffect;
    Vector3 startPoint;
    [System.NonSerialized] public Vector3 endPoint;
    [System.NonSerialized] public Vector3 direction;
    [SerializeField] float timeToHit;
    [SerializeField] float arcHeight;
    float speed;
    float midpoint;
    float arcWidth;
    Collider sphereCollider;
    bool groundPustule = false;
    float pulseDamageCounter = 0;
    float pulseHealCounter = 0;

    private void Start()
    {
        sphereCollider = GetComponent<Collider>();

        startPoint = transform.position;
        direction = new Vector3(endPoint.x, 0, endPoint.z) - new Vector3(startPoint.x, 0, startPoint.z);
        float distance = Vector2.Distance(new Vector2(startPoint.x, startPoint.z), new Vector2(endPoint.x, endPoint.z));
        speed = distance / timeToHit;

        midpoint = distance / 2;
        arcWidth = arcHeight / Mathf.Pow(midpoint, 2);
    }

    void FixedUpdate()
    {
        if (groundPustule) return;

        transform.position = transform.position + direction.normalized * Time.fixedDeltaTime * speed;
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(startPoint.x, startPoint.z));
        float currentHeight = -arcWidth * Mathf.Pow(distance - midpoint, 2) + arcHeight;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        if (transform.position.y <= 0)
        {
            BecomeGroundPustule();
        }
    }

    private void Update()
    {
        if (!groundPustule) return;

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 3);
        foreach(Collider nearbyObject in nearbyObjects)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                pulseDamageCounter += Time.deltaTime * pulseDamageRate;
                if(pulseDamageCounter > 1)
                {
                    int pulseDamage = Mathf.RoundToInt(pulseDamageCounter);
                    pulseDamageCounter = 0;
                    nearbyObject.GetComponent<PlayerScript>().LoseHealth(pulseDamage);
                }
            }
            else if (nearbyObject.CompareTag("Enemy"))
            {
                pulseHealCounter += Time.deltaTime * pulseHealRate;
                if(pulseHealCounter > 1)
                {
                    int pulseHeal = Mathf.RoundToInt(pulseHealCounter);
                    pulseHealCounter = 0;
                    nearbyObject.GetComponent<EnemyScript>().GainHealth(pulseHeal);
                }
            }
        }
    }

    void BecomeGroundPustule()
    {
        transform.position = endPoint;
        sphereCollider.isTrigger = false;
        groundPustule = true;
        airPustuleSprite.enabled = false;
        groundPustuleSprite.enabled = true;
        pulseEffect.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript playerScript = other.GetComponent<PlayerScript>();
            playerScript.LoseHealth(damage);
            playerScript.LosePoise(poiseDamage);
            Destroy(gameObject);
        }
    }
}
