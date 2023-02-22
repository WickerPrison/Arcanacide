using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hadoken : Projectile
{
    [SerializeField] GameObject smallerProjectilePrefab;
    [SerializeField] int projectileNum;
    [SerializeField] float spawnRadius;


    public override void HitObject(Collider collision)
    {
        Explode();
        base.HitObject(collision);
    }

    void Explode()
    {
        AudioSource.PlayClipAtPoint(impactSFX, transform.position, impactSFXvolume);
        for (int i = 0; i < projectileNum; i++)
        {
            Projectile projectile = Instantiate(smallerProjectilePrefab).GetComponent<Projectile>();
            float angle = 360 / projectileNum * i * Mathf.Deg2Rad;
            Vector3 direction = Vector3.forward;
            float x = Mathf.Cos(angle) * direction.x - Mathf.Sin(angle) * direction.z;
            float z = Mathf.Sin(angle) * direction.x + Mathf.Cos(angle) * direction.z;
            direction = new Vector3(x, 0, z).normalized; 
            projectile.transform.position = transform.position + direction * spawnRadius;
            projectile.direction = direction;
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
}
