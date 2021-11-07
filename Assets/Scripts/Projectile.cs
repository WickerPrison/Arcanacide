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
            PlayerScript playerScript;
            playerScript = collision.gameObject.GetComponent<PlayerScript>();
            playerScript.LoseHealth(spellDamage);
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + direction.normalized * Time.fixedDeltaTime * speed;
    }
}
