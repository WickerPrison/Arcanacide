using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerProjectile : MonoBehaviour
{
    public int speed;
    public PlayerController playerController;
    [SerializeField] AudioClip enemyImpactSFX;
    [SerializeField] AudioClip impactSFX;
    [SerializeField] float impactSFXvolume;
    [SerializeField] float lifetime;
    [SerializeField] PlayerData playerData;
    [System.NonSerialized] public AttackProfiles attackProfile;
    public Transform target;
    public float turnAngle;
    Vector3 offset = new Vector3(0, 1, 0);

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(collision);
        }
        else
        {
            HitObject(collision);
        }
    }

    public virtual void HitEnemy(Collider collision)
    {
        EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();
        int damage = Mathf.RoundToInt(playerData.ArcaneDamage() * attackProfile.magicDamageMultiplier);
        int poiseDamage = Mathf.RoundToInt(playerData.ArcaneDamage() * attackProfile.poiseDamageMultiplier);
        enemyScript.LoseHealth(damage, poiseDamage);
        enemyScript.GainDOT(attackProfile.durationDOT);
        AudioSource.PlayClipAtPoint(enemyImpactSFX, transform.position, impactSFXvolume);
        Destroy(gameObject);
    }

    public virtual void HitObject(Collider collision)
    {
        AudioSource.PlayClipAtPoint(impactSFX, transform.position, impactSFXvolume);
        Destroy(gameObject);
    }

    public virtual void FixedUpdate()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 rayDirection = transform.position - target.position + offset;
        float angleToTarget = Mathf.Acos(Vector3.Dot(-rayDirection, transform.forward) / (rayDirection.magnitude * transform.forward.magnitude));
        angleToTarget *= Mathf.Rad2Deg;
        if (angleToTarget <= turnAngle * Time.fixedDeltaTime)
        {
            transform.LookAt(target);
        }
        else
        {
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, target.position + offset - transform.position, turnAngle * Mathf.Deg2Rad * Time.fixedDeltaTime, 0);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }

        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
