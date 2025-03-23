using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int speed;
    public PlayerMovement playerMovement;
    [SerializeField] EventReference enemyImpactSFX;
    [SerializeField] EventReference impactSFX;
    [SerializeField] float impactSFXvolume;
    [SerializeField] float lifetime;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [System.NonSerialized] public AttackProfiles attackProfile;
    public Transform target;
    public float turnAngle;
    [System.NonSerialized] public Vector3 offset = new Vector3(0, 1, 0);
    float addedDOT;

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
        if(attackProfile.attackType == AttackType.SPECIAL && playerData.equippedPatches.Contains(Patches.ARCANE_MASTERY))
        {
            damage += Mathf.RoundToInt(damage * emblemLibrary.arcaneMastery.value);
        }
        enemyScript.LoseHealth(damage, damage * attackProfile.poiseDamageMultiplier);
        enemyScript.ImpactVFX();
        if (attackProfile.attackType == AttackType.DEFLECT && playerData.equippedPatches.Contains(Patches.BURNING_REFLECTION))
        {
            addedDOT = 10;
        }
        else addedDOT = 0;
        enemyScript.GainDOT(attackProfile.durationDOT + addedDOT);
        RuntimeManager.PlayOneShot(impactSFX, impactSFXvolume, transform.position);
        KillProjectile();
    }

    public virtual void HitObject(Collider collision)
    {
        RuntimeManager.PlayOneShot(impactSFX, impactSFXvolume, transform.position);
        KillProjectile();
    }

    public virtual void FixedUpdate()
    {
        if(target == null)
        {
            KillProjectile();
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

    public virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            KillProjectile();
        }
    }

    public virtual void KillProjectile()
    {
        Destroy(gameObject);
    }
}
