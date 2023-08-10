using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] AudioClip impactSound;
    public EnemyScript enemyOfOrigin;
    BossController bossController;
    AudioSource sfx;
    Transform player;
    float duration = 7;
    int damage = 20;
    float poiseDamage = 70;
    float speed = 8;

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
        sfx.time = Random.Range(0, 0.5f);
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
                AudioSource.PlayClipAtPoint(impactSound, transform.position, 0.5f);
                Destroy(gameObject);
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
            Destroy(gameObject);
        }

        if (bossController.hasSurrendered)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = player.position - transform.position;
        transform.Translate(direction.normalized * Time.fixedDeltaTime * speed);
    }
}
