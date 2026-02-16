using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHallOfDeath : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    PlayerScript playerScript;
    bool isHurtingPlayer;
    int damagePerTick;
    float tickRate = 0.1f;
    float timer = 0;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        damagePerTick = Mathf.CeilToInt(playerData.MaxHealth() * 0.1f);
    }


    private void Update()
    {
        if (!isHurtingPlayer) return;
        timer += Time.deltaTime;
        if (timer > tickRate)
        {
            playerScript.LoseHealth(damagePerTick, EnemyAttackType.NONPARRIABLE, null);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isHurtingPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isHurtingPlayer = false;
    }
}
