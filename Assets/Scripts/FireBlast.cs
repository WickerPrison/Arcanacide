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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerScript playerScript;
            PlayerController playerController;
            Rigidbody playerRB;
            playerScript = other.gameObject.GetComponent<PlayerScript>();
            playerController = other.gameObject.GetComponent<PlayerController>();
            playerScript.LoseHealth(spellDamage);
            playerController.stunned += 1;
            playerRB = playerController.gameObject.GetComponent<Rigidbody>();
            Vector3 pushDirection = playerController.transform.position - transform.position;
            playerRB.AddForce(pushDirection.normalized * 15, ForceMode.VelocityChange);
        }
    }
}
