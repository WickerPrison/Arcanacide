using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBox : MonoBehaviour
{
    [SerializeField] AudioClip impactSFX;
    [SerializeField] int damage;
    [SerializeField] float poiseDamage;
    FireWave fireWave;

    private void Start()
    {
        fireWave = GetComponentInParent<FireWave>();
    }


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
                playerController.PerfectDodge();
            }
        }
        else
        {
            fireWave.boxNum -= 1;
            Destroy(gameObject);
        }
    }
}
