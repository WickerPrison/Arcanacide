using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    float duration = 5;
    float damagePerSecond = 5;
    float damage;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerScript playerScript;
            playerScript = other.gameObject.GetComponent<PlayerScript>();
            damage += damagePerSecond * Time.deltaTime;
            if(damage > 1)
            {
                playerScript.LoseHealth(Mathf.FloorToInt(damage));
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
