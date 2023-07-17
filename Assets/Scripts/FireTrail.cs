using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    AudioSource sfx;
    public float duration = 5;
    public float damagePerSecond = 5;
    float damage;


    private void Start()
    {
        sfx = GetComponent<AudioSource>();
        sfx.time += Random.Range(0, 0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.layer == 3)
        {
            PlayerAbilities playerAbilities;
            playerAbilities = other.gameObject.GetComponent<PlayerAbilities>();
            if (playerAbilities.shield)
            {
                return;
            }
            damage += damagePerSecond * Time.deltaTime;
            if(damage > 1)
            {
                PlayerScript playerScript = playerAbilities.GetComponent<PlayerScript>();
                playerScript.LoseHealth(Mathf.FloorToInt(damage), EnemyAttackType.NONPARRIABLE, null);
                damage = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
