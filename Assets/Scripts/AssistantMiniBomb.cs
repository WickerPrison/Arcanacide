using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantMiniBomb : ArcProjectile
{
    [SerializeField] GameObject fireCirclePrefab;

    public override void Explosion()
    {
        GameObject fireCircle = Instantiate(fireCirclePrefab);
        fireCircle.transform.position = transform.position;
        base.Explosion();
    }
}
