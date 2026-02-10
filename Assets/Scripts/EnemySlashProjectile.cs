using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EnemySlashProjectile : Projectile
{
    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public override void HitObject(Collider collision)
    {
        
    }

    public override void HitPlayer(Collider collision)
    {
        PlayerScript playerScript;
        playerScript = collision.gameObject.GetComponent<PlayerScript>();
        playerScript.LoseHealth(spellDamage, EnemyAttackType.PROJECTILE, enemyOfOrigin);
        playerScript.LosePoise(poiseDamage);
        FmodUtils.PlayOneShot(playerImpactSFX, impactSFXvolume, transform.position);
    }
}
