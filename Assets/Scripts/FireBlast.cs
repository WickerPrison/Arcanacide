using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlast : MonoBehaviour
{
    EnemyController enemyController;
    int spellDamage;

    private void Start()
    {
        enemyController = GetComponentInParent<BossController>();
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
            playerController.stagger += 1;
            playerRB = playerController.gameObject.GetComponent<Rigidbody>();
            Vector3 pushDirection = playerController.transform.position - transform.position;
            playerRB.AddForce(pushDirection.normalized * 15, ForceMode.VelocityChange);
        }
    }
}
