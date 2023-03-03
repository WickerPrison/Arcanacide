using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FastProjectile : Projectile
{
    LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Player", "IFrames");
    }

    public override void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, Time.fixedDeltaTime * speed, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.gameObject.name);
            Collision(hit.collider);
        }

        base.FixedUpdate();
    }
}
