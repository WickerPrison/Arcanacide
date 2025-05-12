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
    HarpoonManager harpoonManager;

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
            TeslaHarpoon harpoon = Instantiate(teslaHarpoonPrefab).GetComponent<TeslaHarpoon>();
            harpoon.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            harpoon.harpoonManager = harpoonManager;
            harpoonManager.harpoonProjectiles.Remove(this);
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

    public void SetupHarpoon(EnemyScript enemyScript)
    {
        enemyOfOrigin = enemyScript;
        harpoonManager = enemyOfOrigin.GetComponent<HarpoonManager>();
        transform.parent.position = harpoonManager.GetHarpoonPosition();
        harpoonManager.harpoonProjectiles.Add(this);
    }
}
