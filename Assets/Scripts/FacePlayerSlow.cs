using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacePlayerSlow : FacePlayer
{
    [SerializeField] Transform trackingPoint;
    [SerializeField] Transform attackAnchor;
    [SerializeField] float radius = 1;
    public float rotateSpeed;

    public override void Start()
    {
        base.Start();
        trackingPoint.position = transform.position + Vector3.forward * radius;
        attackPoint.position = trackingPoint.position;
        AttackPoint();
    }

    public override void AttackPoint()
    {
        float angle = GetAngle();

        if (Mathf.Abs(angle) > rotateSpeed * Time.deltaTime)
        {
            attackAnchor.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime * angle / Mathf.Abs(angle), 0));
        }
        else
        {
            attackAnchor.Rotate(new Vector3(0, angle, 0));
        }
    }

    public void FacePlayerFast()
    {
        attackAnchor.Rotate(new Vector3(0, GetAngle(), 0));  
        FaceAttackPoint();
    }

    float GetAngle()
    {
        Vector3 direction = player.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        trackingPoint.position = transform.position + direction.normalized * radius;

        Vector2 trackingVector = new Vector2(direction.x, direction.z);
        Vector3 attackVector3 = attackPoint.position - transform.position;
        Vector2 attackVector = new Vector2(attackVector3.x, attackVector3.z);

        float angle = Vector2.SignedAngle(trackingVector, attackVector);
        return angle;
    }
}
