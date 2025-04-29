using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningHalo : LightningBolt
{
    [SerializeField] float radius = 0.3f;
    LineRenderer line;
    bool disabled = false;
    [SerializeField] Transform frontHalo;
    [SerializeField] Transform backHalo;
    EnemyController enemyController;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public override void Start()
    {
        base.Start();
        enemyController = GetComponentInParent<EnemyController>();
    }

    public override void Update()
    {
        if (disabled) return;
        base.Update();
    }

    public override void PlacePoints()
    {
        Vector3 targetPos = GetTarget();
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

    Vector3 GetTarget()
    {
        if (enemyController.facingFront)
        {
            return frontHalo.position;
        }
        else
        {
            return backHalo.position;
        }
    }

    public void ShowRings(bool showRing)
    {
        disabled = !showRing;
        line.enabled = showRing;
        foreach (LineRenderer fork in forks)
        {
            fork.enabled = showRing;
        }
    }
}
