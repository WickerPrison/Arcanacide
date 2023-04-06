using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrap : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    ParticleSystem vfx;
    float damage = 0;
    float damagePerSecond;
    float duration = 3;
    float timer;
    Vector3 away = Vector3.one * 100;

    private void Awake()
    {
        vfx = GetComponent<ParticleSystem>();
        damagePerSecond = playerData.dedication * 4;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0 )
            {
                transform.position = away;
            }
        }
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

    public void StartTimer()
    {
        timer = duration;
    }
}
