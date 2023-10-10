using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBox : MonoBehaviour
{
    [SerializeField] float maxAudioDistance;
    [SerializeField] EventReference playerImpactSFX;
    [SerializeField] float playerImpactVolume = 0.8f;
    [SerializeField] EventReference impactSFX;
    [SerializeField] float impactVolume = 0.8f;
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
                PlaySound(playerImpactSFX, playerImpactVolume);
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
            PlaySound(playerImpactSFX, playerImpactVolume);
            Destroy(gameObject);
        }
        else
        {
            if(fireWave!= null)
            {
                fireWave.boxNum -= 1;
            }
            PlaySound(impactSFX, impactVolume);
            Destroy(gameObject);
        }
    }

    void PlaySound(EventReference sound, float volume)
    {
        if (sound3D)
        {
            string[] parameters = { "MaxDistance", "Volume" };
            float[] values = { maxDistance, volume };

            AudioMethods.PlayOneShot(sound, parameters, values, transform.position);
        }
        else
        {
            RuntimeManager.PlayOneShot(sound, volume, transform.position);
        }
    }
}
