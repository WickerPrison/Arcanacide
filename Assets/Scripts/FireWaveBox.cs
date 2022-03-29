using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWaveBox : MonoBehaviour
{
    [SerializeField] AudioClip impactSFX;
    int damage = 10;
    float poiseDamage = 20;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.layer == 3)
            {
                PlayerScript playerScript;
                playerScript = other.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(damage);
                playerScript.LosePoise(poiseDamage);
                AudioSource.PlayClipAtPoint(impactSFX, transform.position, 1);
                Destroy(gameObject);
            }
            else if(other.gameObject.layer == 8)
            {
                PlayerController playerController;
                playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.PathOfTheSword();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
