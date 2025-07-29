using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepWithAttack : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask ignoreLayers;
    Rigidbody rb;
    Vector3 stepDirection;
    float raycastDistance;
    float moveSpeed = 5;
    [System.NonSerialized] public float maxStepTimer = 0.15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider myCollider = GetComponent<CapsuleCollider>();
        raycastDistance = myCollider.radius + moveSpeed * maxStepTimer + 0.1f;
    }


    public void Step(float stepWithAttackTimer)
    {
        stepDirection = Vector3.Normalize(attackPoint.position - transform.position);
        if(!Physics.Raycast(transform.position, stepDirection, raycastDistance, ~ignoreLayers))
        {
            maxStepTimer = stepWithAttackTimer;
            StartCoroutine(StepCoroutine());
        }
    }

    IEnumerator StepCoroutine()
    {
        moveSpeed = 5;
        float stepTimer = maxStepTimer;
        

        while (stepTimer > 0)
        {
            stepTimer -= Time.fixedDeltaTime;
            //rb.velocity = new Vector3(stepDirection.x * Time.fixedDeltaTime * moveSpeed, 0, stepDirection.z * Time.fixedDeltaTime * moveSpeed);
            rb.MovePosition(transform.position + stepDirection.normalized * Time.fixedDeltaTime * moveSpeed);
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector3.zero;
    }
}
