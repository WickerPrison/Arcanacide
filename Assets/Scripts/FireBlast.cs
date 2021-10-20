using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlast : MonoBehaviour
{
    BossController bossController;
    int spellDamage;

    private void Start()
    {
        bossController = GetComponentInParent<BossController>();
        spellDamage = bossController.fireBlastDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerScript playerScript;
            playerScript = other.gameObject.GetComponent<PlayerScript>();
            playerScript.LoseHealth(spellDamage);
        }
    }
}
