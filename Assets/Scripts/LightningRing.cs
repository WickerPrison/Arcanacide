using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRing : LightningBolt
{
    float radius = 1;
    Vector3 targetPos;
    IHaveLightningRings ringsController;
    LineRenderer line;
    bool disabled = true;

    private void Awake()
    {
        ringsController = (IHaveLightningRings)GetComponentInParent(typeof(IHaveLightningRings));
        Debug.Log(ringsController);
        line = GetComponent<LineRenderer>();
    }

    public override void Update()
    {
        if (disabled) return;
        base.Update();
    }

    public override void PlacePoints()
    {
        targetPos = transform.parent.position + Vector3.up * 1.3f;
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
        ringsController.onSetRadius += Rings_onSetRadius;
        ringsController.onShowRings += Rings_onShowRings;
    }

    private void OnDisable()
    {
        ringsController.onSetRadius -= Rings_onSetRadius;
        ringsController.onShowRings -= Rings_onShowRings;
    }

    private void Rings_onSetRadius(object sender, float newRadius)
    {
        radius = newRadius;
    }

    private void Rings_onShowRings(object sender, bool showRing)
    {
        disabled = !showRing;
        line.enabled = showRing;
        foreach(LineRenderer fork in forks)
        {
            fork.enabled = showRing;
        }
    }
}
