using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileStraight : PlayerProjectile
{
    public static PlayerProjectileStraight Instantiate(GameObject prefab, Vector3 position, Vector3 direction, AttackProfiles attackProfile)
    {
        PlayerProjectileStraight projectile = Instantiate(prefab).GetComponent<PlayerProjectileStraight>();
        projectile.transform.position = position;
        projectile.transform.LookAt(position + direction);
        projectile.attackProfile = attackProfile;
        return projectile;
    }

    public override void FixedUpdate()
    {
        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }
}
