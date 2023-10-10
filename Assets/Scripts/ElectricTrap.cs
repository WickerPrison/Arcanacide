using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricTrap : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] EventReference electricDamage;
    float damage = 0;
    float damagePerSecond;
    float duration = 3;
    float timer;
    Vector3 away = Vector3.one * 100;
    bool canMakeDamageSound = true;
    List<EnemyScript> enemiesInRange = new List<EnemyScript>();
    StudioEventEmitter sfx;

    private void Awake()
    {
        damagePerSecond = playerData.arcane * 2;
        sfx = GetComponent<StudioEventEmitter>();
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0 )
            {
                transform.position = away;
                sfx.Stop();
            }
        }

        if(enemiesInRange.Count > 0)
        {
            damage += damagePerSecond * Time.deltaTime;
            if (damage > 1)
            {
                foreach(EnemyScript enemy in enemiesInRange)
                {
                    enemy.LoseHealth(Mathf.FloorToInt(damage), 0);

                    if (playerData.equippedEmblems.Contains(emblemLibrary.rending_blows))
                    {
                        enemy.GainDOT(emblemLibrary.rendingBlowsDuration);
                    }
                }

                if (canMakeDamageSound)
                {
                    RuntimeManager.PlayOneShot(electricDamage, 0.2f, transform.position);
                    StartCoroutine(SFXtimer());
                }
                damage = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<EnemyScript>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.GetComponent<EnemyScript>());
        }
    }

    IEnumerator SFXtimer()
    {
        canMakeDamageSound = false;
        int lengthMilliseconds;
        RuntimeManager.GetEventDescription(electricDamage).getLength(out lengthMilliseconds);
        yield return new WaitForSeconds(lengthMilliseconds * 1000);
        canMakeDamageSound = true;
    }

    public void StartTimer()
    {
        timer = duration;
        sfx.Play();
    }
}
