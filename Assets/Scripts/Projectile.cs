using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    int spellDamage = 30;
    int speed = 12;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.layer == 3)
            {
                PlayerScript playerScript;
                playerScript = collision.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(spellDamage);
                Destroy(gameObject);
            }
            else if(collision.gameObject.layer == 8)
            {
                PlayerController playerController;
                playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.PathOfTheSword();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + direction.normalized * Time.fixedDeltaTime * speed;
    }
}
