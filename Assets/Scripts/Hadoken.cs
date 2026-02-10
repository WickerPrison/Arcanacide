using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hadoken : Projectile
{
    [SerializeField] GameObject smallerProjectilePrefab;
    [SerializeField] int projectileNum;
    [SerializeField] float spawnRadius;
    public float friendshipPower;

    public override void HitObject(Collider collision)
    {
        Explode();
        base.HitObject(collision);
    }

    void Explode()
    {
        FmodUtils.PlayOneShot(impactSFX, impactSFXvolume, transform.position);
        for (int i = 0; i < projectileNum; i++)
        {
            Projectile projectile = Instantiate(smallerProjectilePrefab).GetComponent<Projectile>();
            float angle = 360 / projectileNum * i * Mathf.Deg2Rad;
            direction = RotateByAngle(Vector3.forward, angle);
            projectile.transform.position = transform.position + direction * spawnRadius;
            projectile.direction = direction;
            projectile.spellDamage = Mathf.RoundToInt(friendshipPower * projectile.spellDamage);
        }
    }

    public override void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    public Vector3 RotateByAngle(Vector3 direction, float angle)
    {
        float x = Mathf.Cos(angle) * direction.x - Mathf.Sin(angle) * direction.z;
        float z = Mathf.Sin(angle) * direction.x + Mathf.Cos(angle) * direction.z;
        return new Vector3(x, 0, z).normalized;
    }
}
