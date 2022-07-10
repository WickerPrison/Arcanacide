using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public int spellDamage;
    [SerializeField] int speed;
    [SerializeField] AudioClip playerImpactSFX;
    [SerializeField] AudioClip impactSFX;
    [SerializeField] float impactSFXvolume;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.layer == 3)
            {
                PlayerScript playerScript;
                playerScript = collision.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(spellDamage);
                AudioSource.PlayClipAtPoint(playerImpactSFX, transform.position, impactSFXvolume);
                Destroy(gameObject);
            }
            else if(collision.gameObject.layer == 8)
            {
                PlayerController playerController;
                playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.PerfectDodge();
            }
        }
        else
        {
            AudioSource.PlayClipAtPoint(impactSFX, transform.position, impactSFXvolume);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + direction.normalized * Time.fixedDeltaTime * speed;
    }
}
