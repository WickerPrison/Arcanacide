using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : ArcProjectile
{
    [SerializeField] SpriteRenderer sprite;
    bool hasExploded = false;

    public override void FixedUpdate()
    {
        if (hasExploded) return;
        base.FixedUpdate();
    }

    public override void DestroyProjectile()
    {
        hasExploded = true;
        sprite.enabled = false;
        StartCoroutine(WaitToKill());
    }

    IEnumerator WaitToKill()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
