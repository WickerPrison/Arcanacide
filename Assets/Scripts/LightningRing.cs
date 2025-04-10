using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRing : LightningBolt
{
    Transform target;
    float radius = 1;
    Vector3 targetPos;
    LightningRings rings;

    private void Awake()
    {
        rings = GetComponentInParent<LightningRings>();
    }

    public override void PlacePoints()
    {
        targetPos = target.position + Vector3.up * 1.3f;
        float angle = 360 / pointNum;
        startPoint.position = targetPos + Vector3.right * radius;

        points[0] = startPoint.position + Noise(noiseAmp);
        for (int i = 1; i < pointNum; i++)
        {
            Vector3 direction = Utils.RotateDirection(Vector3.right, angle * i);
            Vector3 position = targetPos + direction.normalized * radius + Noise(noiseAmp);
            points[i] = position;
        }
    }

    private void OnEnable()
    {
        rings.onSetRadius += Rings_onSetRadius;
        rings.onSetTarget += Rings_onSetTarget;
    }

    private void OnDisable()
    {
        rings.onSetRadius -= Rings_onSetRadius;
        rings.onSetTarget -= Rings_onSetTarget;
    }

    private void Rings_onSetTarget(object sender, Transform newTarget)
    {
        target = newTarget;
    }

    private void Rings_onSetRadius(object sender, float newRadius)
    {
        radius = newRadius;
    }
}
