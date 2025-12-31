using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileStraight : PlayerProjectile
{
    Vector3 direction;

    public static PlayerProjectileStraight Instantiate(GameObject prefab, Vector3 position, Vector3 direction, AttackProfiles attackProfile, PlayerAbilities playerAbilities)
    {
        PlayerProjectileStraight projectile = PlayerProjectile.Instantiate(prefab, attackProfile, playerAbilities) as PlayerProjectileStraight;
        projectile.transform.position = position;
        projectile.transform.LookAt(position + direction);
        projectile.direction = direction;
        return projectile;
    }

    public static PlayerProjectileStraight InstantiateIcicle(GameObject prefab, Vector3 position, Vector3 direction, AttackProfiles attackProfile, PlayerAbilities playerAbilities)
    {
        PlayerProjectileStraight projectile = PlayerProjectile.Instantiate(prefab, attackProfile, playerAbilities) as PlayerProjectileStraight;
        projectile.transform.position = position;
        float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        projectile.transform.rotation = Quaternion.Euler(25, 0, -angle);
        projectile.direction = direction;
        return projectile;
    }

    public override void FixedUpdate()
    {
        transform.position += direction * Time.fixedDeltaTime * speed;
    }
}
