using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTrail : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    AudioSource sfx;
    float duration = 3;
    float damagePerSecond;
    float damage;

    private void Start()
    {
        damagePerSecond = playerData.PathDamage();
        sfx = GetComponent<AudioSource>();
        sfx.time += Random.Range(0, 0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemyScript;
            enemyScript = other.gameObject.GetComponent<EnemyScript>();
            damage += damagePerSecond * Time.deltaTime;
            if (damage > 1)
            {
                enemyScript.LoseHealth(Mathf.FloorToInt(damage), 0);
                damage = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
