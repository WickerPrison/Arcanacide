using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject playAtPointPrefab;
    [System.NonSerialized] public Vector3 direction;
    public int spellDamage;
    public int poiseDamage;
    public int speed;
    [SerializeField] AudioClip playerImpactSFX;
    public AudioClip impactSFX;
    public float impactSFXvolume;
    public float lifetime;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;

    private void OnTriggerEnter(Collider collision)
    {
        Collision(collision);
    }

    public void Collision(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.layer == 3)
            {
                HitPlayer(collision);
            }
            else if (collision.gameObject.layer == 8)
            {
                PerfectDodge(collision);
            }
        }
        else
        {
            HitObject(collision);
        }
    }

    public virtual void HitPlayer(Collider collision)
    {
        PlayerScript playerScript;
        playerScript = collision.gameObject.GetComponent<PlayerScript>();
        playerScript.LoseHealth(spellDamage,EnemyAttackType.PROJECTILE, enemyOfOrigin);
        playerScript.LosePoise(poiseDamage);
        Instantiate(playAtPointPrefab).GetComponent<PlayAtPoint>().PlayClip(playerImpactSFX, impactSFXvolume, transform.position);
        Destroy(gameObject);
    }

    public virtual void PerfectDodge(Collider collision)
    {
        PlayerScript playerScript;
        playerScript = collision.gameObject.GetComponent<PlayerScript>();
        playerScript.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin, gameObject);
    }

    public virtual void HitObject(Collider collision)
    {
        Instantiate(playAtPointPrefab).GetComponent <PlayAtPoint>().PlayClip(impactSFX, impactSFXvolume, transform.position);
        Destroy(gameObject);
    }

    public virtual void FixedUpdate()
    {
        transform.position = transform.position + direction.normalized * Time.fixedDeltaTime * speed;
    }

    public virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
