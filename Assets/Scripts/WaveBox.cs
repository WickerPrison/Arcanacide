using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBox : MonoBehaviour
{
    [SerializeField] EventReference impactSFX;
    public int damage;
    public float poiseDamage;
    [SerializeField] bool canHurtEnemies = false;
    FireWave fireWave;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;

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
                RuntimeManager.PlayOneShot(impactSFX, 1, transform.position);
                Destroy(gameObject);
            }
            else if(other.gameObject.layer == 8)
            {
                PlayerScript playerScript;
                playerScript = other.gameObject.GetComponent<PlayerScript>();
                playerScript.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin, gameObject);
            }
        }
        else if(canHurtEnemies && other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            enemyScript.LoseHealth(damage, poiseDamage);
            RuntimeManager.PlayOneShot(impactSFX, 1, transform.position);
            Destroy(gameObject);
        }
        else
        {
            if(fireWave!= null)
            {
                fireWave.boxNum -= 1;
            }
            RuntimeManager.PlayOneShot(impactSFX, 1, transform.position);
            Destroy(gameObject);
        }
    }
}
