using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBox : MonoBehaviour
{
    [SerializeField] AudioClip impactSFX;
    public int damage;
    public float poiseDamage;
    [SerializeField] bool canHurtEnemies = false;
    FireWave fireWave;
    EnemyScript enemyOfOrigin;

    private void Start()
    {
        if(fireWave = GetComponentInParent<FireWave>())
        {
            enemyOfOrigin = fireWave.enemyOfOrigin;
        }
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
                AudioSource.PlayClipAtPoint(impactSFX, transform.position, 1);
                Destroy(gameObject);
            }
            else if(other.gameObject.layer == 8)
            {
                PlayerController playerController;
                playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.PerfectDodge(gameObject, enemyOfOrigin);
            }
        }
        else if(canHurtEnemies && other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            enemyScript.LoseHealth(damage, poiseDamage);
            Destroy(gameObject);
        }
        else
        {
            if(fireWave!= null)
            {
                fireWave.boxNum -= 1;
            }
            Destroy(gameObject);
        }
    }
}
