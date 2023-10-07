using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBox : MonoBehaviour
{
    [SerializeField] float maxAudioDistance;
    [SerializeField] EventReference impactSFX;
    public int damage;
    public float poiseDamage;
    [SerializeField] bool canHurtEnemies = false;
    FireWave fireWave;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;
    [SerializeField] bool sound3D = false;
    [SerializeField] float maxDistance;

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
                PlaySound();
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
            PlaySound();
            Destroy(gameObject);
        }
        else
        {
            if(fireWave!= null)
            {
                fireWave.boxNum -= 1;
            }
            PlaySound();
            Destroy(gameObject);
        }
    }

    void PlaySound()
    {
        if (sound3D)
        {
            AudioMethods.PlayOneShot(impactSFX, "MaxDistance", maxDistance, transform.position);
        }
        else
        {
            RuntimeManager.PlayOneShot(impactSFX, 1, transform.position);
        }
    }
}
