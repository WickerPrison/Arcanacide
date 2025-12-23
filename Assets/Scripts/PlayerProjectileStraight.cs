using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileStraight : PlayerProjectile
{
    public static PlayerProjectileStraight Instantiate(GameObject prefab, Vector3 position, Vector3 direction, AttackProfiles attackProfile, PlayerAbilities playerAbilities)
    {
        PlayerProjectileStraight projectile = PlayerProjectile.Instantiate(prefab, attackProfile, playerAbilities) as PlayerProjectileStraight;
        projectile.transform.position = position;
        projectile.transform.LookAt(position + direction);
        return projectile;
    }

    public override void FixedUpdate()
    {
        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }
}
