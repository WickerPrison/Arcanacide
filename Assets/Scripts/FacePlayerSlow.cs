using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacePlayerSlow : FacePlayer
{
    [SerializeField] Transform trackingPoint;
    [SerializeField] Transform attackAnchor;
    [SerializeField] float rotateSpeed;

    public override void Start()
    {
        base.Start();
        AttackPoint();
        attackPoint.position = trackingPoint.position;
    }

    public override void AttackPoint()
    {
        Vector3 direction = playerController.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        trackingPoint.position = transform.position + direction.normalized;

        Quaternion neededRotation = Quaternion.LookRotation(attackAnchor.position - trackingPoint.position);
        attackAnchor.rotation = Quaternion.RotateTowards(attackAnchor.rotation, neededRotation, rotateSpeed * Time.deltaTime);

        if(attackPoint.position != transform.position)
        {
            attackPoint.transform.rotation = Quaternion.LookRotation(attackPoint.position - transform.position);
        }
    }

    public void FacePlayerFast()
    {
        attackPoint.position = trackingPoint.position;
        AttackPoint();
        FacePlayerSprite();
    }
}
