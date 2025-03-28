using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsVelocity : MonoBehaviour
{
    ArcProjectile arcProjectile;
    SpriteRenderer spriteRenderer;
    bool firstFrame = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        arcProjectile = GetComponentInParent<ArcProjectile>();
    }

    private void FixedUpdate()
    {
        if (firstFrame)
        {
            firstFrame = false;
            StartCoroutine(WaitUntilStabilized());
            return;
        }

        Vector3 direction = Vector3.Normalize(arcProjectile.GetNextPosition(transform.position) - transform.position);
        float angle = Utils.GetAngle(new Vector2(direction.x, direction.y));
        transform.rotation = Quaternion.Euler(25, 0, Mathf.Sign(-direction.x) * angle + 90);
    }

    IEnumerator WaitUntilStabilized()
    {
        int frame = 0;
        while(frame < 3)
        {
            frame += 1;
            yield return null;
        }

        spriteRenderer.enabled = true;
    }
}
