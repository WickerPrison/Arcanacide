using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PustuleScript : ArcProjectile
{
    [SerializeField] float pulseDamageRate;
    [SerializeField] EventReference pulseDamageSFX;
    [SerializeField] float pulseHealRate;
    [SerializeField] EventReference pulseHealSFX;
    [SerializeField] SpriteRenderer pulseEffect;
    [System.NonSerialized] public EnemyScript enemyScript;
    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale;
    Collider sphereCollider;
    bool groundPustule = false;
    float pulseDamageCounter = 0;
    float pulseHealCounter = 0;
    WaitForSeconds lifetime = new WaitForSeconds(15);
    [SerializeField] Transform sphere;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    public override void Start()
    {
        sphereCollider = GetComponent<Collider>();
        base.Start();
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
                    nearbyObject.GetComponent<PlayerScript>().LoseHealth(pulseDamage, EnemyAttackType.NONPARRIABLE, null);
                    RuntimeManager.PlayOneShot(pulseDamageSFX, .5f);
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
                    //RuntimeManager.PlayOneShot(pulseHealSFX, 0.5f);
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        if (!groundPustule)
            base.FixedUpdate();
    }

    void BecomeGroundPustule()
    {
        if (groundPustule) return;
        transform.position = endPoint;
        sphereCollider.isTrigger = false;
        groundPustule = true;
        pulseEffect.enabled = true;
        StartCoroutine(PustuleGrowth());
        StartCoroutine(LifetimeCounter());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript playerScript = other.GetComponent<PlayerScript>();
            playerScript.LoseHealth(spellDamage, EnemyAttackType.PROJECTILE, enemyScript);
            playerScript.LosePoise(poiseDamage);
            RuntimeManager.PlayOneShot(impactSound, 1);
            Destroy(gameObject);
        }
    }

    IEnumerator PustuleGrowth()
    {
        float growthTime = 0.3f;
        float timer = growthTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            sphere.localScale = Vector3.Lerp(endScale, startScale, timer / growthTime);
            yield return endOfFrame;
        }
    }

    IEnumerator LifetimeCounter()
    {
        yield return lifetime;
        Destroy(gameObject);
    }

    public override void Explosion()
    {
        BecomeGroundPustule();
    }

    public override void SpawnIndicator()
    {
        //override this function with nothing
    }
}
