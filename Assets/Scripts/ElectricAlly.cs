using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAlly : MonoBehaviour
{
    int damage = 20;
    float poiseDamage = 20;
    public bool isShielded = false;
    [SerializeField] SpriteRenderer shield;
    PlayerScript playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    public void ShieldOnOff(bool onOrOff)
    {
        shield.enabled = onOrOff;
        isShielded = onOrOff;
    }

    public void OnHit()
    {
        if (!isShielded)
        {
            return;
        }

        playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
        playerScript.LosePoise(poiseDamage);
        playerScript.StartStagger(1);
    }
}
