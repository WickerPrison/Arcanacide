using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] EventReference impactSound;
    [SerializeField] EventReference fireSound;
    EventInstance fmodInstance;
    public EnemyScript enemyOfOrigin;
    BossController bossController;
    Transform player;
    float duration = 7;
    int damage = 20;
    float poiseDamage = 70;
    float speed = 8;

    private void Start()
    {
        fmodInstance = RuntimeManager.CreateInstance(fireSound);
        fmodInstance.start();
        fmodInstance.setTimelinePosition(Random.Range(0, 2000));
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        bossController = enemyOfOrigin.GetComponent<BossController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.layer == 3)
            {
                PlayerScript playerScript;
                playerScript = other.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(damage,EnemyAttackType.PROJECTILE, enemyOfOrigin);
                playerScript.LosePoise(poiseDamage);
                RuntimeManager.PlayOneShot(impactSound, 0.5f, transform.position);
                DestroyBonfire();
            }
            else if(other.gameObject.layer == 8)
            {
                PlayerScript playerScript;
                playerScript = other.gameObject.GetComponent<PlayerScript>();
                playerScript.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin, gameObject);
            }
        }
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            DestroyBonfire();
        }

        if (bossController.hasSurrendered)
        {
            DestroyBonfire();
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = player.position - transform.position;
        transform.Translate(direction.normalized * Time.fixedDeltaTime * speed);
    }

    void DestroyBonfire()
    {
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
        Destroy(gameObject);
    }
}
