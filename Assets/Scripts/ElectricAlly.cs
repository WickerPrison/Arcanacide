using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAlly : MonoBehaviour
{
    int damage = 50;
    float poiseDamage = 70;
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

        playerScript.LoseHealth(damage);
        playerScript.LosePoise(poiseDamage);
        playerScript.StartStagger(1);
    }
}
