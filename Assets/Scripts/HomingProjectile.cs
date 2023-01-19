using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HomingProjectile : Projectile
{
    public Transform target;
    public float turnAngle;
    Vector3 attackOffset = new Vector3(0, 0.8f, 0);

    public override void FixedUpdate()
    {
        Vector3 targetPosition = target.position + attackOffset;
        Vector3 rayDirection = transform.position - targetPosition;
        float angleToTarget = Mathf.Acos(Vector3.Dot(-rayDirection, transform.forward) / (rayDirection.magnitude * transform.forward.magnitude));
        angleToTarget *= Mathf.Rad2Deg;
        if(angleToTarget <= turnAngle * Time.fixedDeltaTime)
        {
            transform.LookAt(target);
        }
        else
        {
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, turnAngle * Mathf.Deg2Rad * Time.fixedDeltaTime, 0);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }

        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }
}
