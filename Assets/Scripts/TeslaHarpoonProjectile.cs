using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TeslaHarpoonProjectile : MonoBehaviour
{
    [SerializeField] GameObject parentObject;
    [SerializeField] float speed;
    [SerializeField] EventReference impactSFX;
    [SerializeField] float impactSFXVolume;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;
    [SerializeField] float initialDelay;
    [SerializeField] TouchingCollider touchingCollider;
    [SerializeField] int damage;
    [SerializeField] float poiseDamage;
    [SerializeField] GameObject teslaHarpoonPrefab;

    private void FixedUpdate()
    {
        if(initialDelay > 0)
        {
            initialDelay -= Time.fixedDeltaTime;
            return;
        }

        transform.position -= Vector3.up * speed * Time.fixedDeltaTime;
        if(transform.position.y <= 0)
        {
            RuntimeManager.PlayOneShot(impactSFX, impactSFXVolume, transform.position);
            Collision();
            Transform harpoon = Instantiate(teslaHarpoonPrefab).transform;
            harpoon.position = transform.position;
            Destroy(parentObject);
        }
    }

    private void Collision()
    {
        List<Collider> touchingObjects = touchingCollider.GetTouchingObjects();
        foreach (Collider collision in touchingObjects)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.layer == 3)
            {
                PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(damage, EnemyAttackType.PROJECTILE, enemyOfOrigin);
                playerScript.LosePoise(poiseDamage);
            }
        }
    }
}
