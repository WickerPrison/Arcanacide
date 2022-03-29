using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public Transform target;
    public int spellDamage;
    public int poiseDamage;
    public float turnAngle;
    int speed = 12;
    [SerializeField] float lifetime;
    [SerializeField] AudioClip impactSFX;
    [SerializeField] float impactSFXvolume;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.layer == 3)
            {
                PlayerScript playerScript;
                playerScript = collision.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(spellDamage);
                playerScript.LosePoise(poiseDamage);
                AudioSource.PlayClipAtPoint(impactSFX, transform.position, impactSFXvolume);
                Destroy(gameObject);
            }
            else if (collision.gameObject.layer == 8)
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
        Vector3 rayDirection = transform.position - target.position;
        float angleToTarget = Mathf.Acos(Vector3.Dot(-rayDirection, transform.forward) / (rayDirection.magnitude * transform.forward.magnitude));
        angleToTarget *= Mathf.Rad2Deg;
        if(angleToTarget <= turnAngle * Time.fixedDeltaTime)
        {
            transform.LookAt(target);
        }
        else
        {
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, target.position - transform.position, turnAngle * Mathf.Deg2Rad * Time.fixedDeltaTime, 0);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }

        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
