using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWaveBox : MonoBehaviour
{
    int damage = 10;
    float poiseDamage = 20;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerScript playerScript;
            playerScript = other.gameObject.GetComponent<PlayerScript>();
            playerScript.LoseHealth(damage);
            playerScript.LosePoise(poiseDamage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
