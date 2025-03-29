using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Missile : ArcProjectile
{
    [SerializeField] SpriteRenderer sprite;
    bool hasExploded = false;

    public override void Start()
    {
        if(NavMesh.SamplePosition(endPoint, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
        {
            base.Start();
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
