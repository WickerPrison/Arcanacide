using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAlly : MonoBehaviour
{
    public int priorityValue;
    public int damage;
    public float poiseDamage;
    public bool isShielded = false;
    PlayerScript playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
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
