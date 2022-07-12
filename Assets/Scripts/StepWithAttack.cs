using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepWithAttack : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    Rigidbody rb;
    Vector3 stepDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Step()
    {
        stepDirection = Vector3.Normalize(attackPoint.position - transform.position);
        if (!Physics.Raycast(transform.position, stepDirection.normalized, 1))
        {
            StartCoroutine(StepCoroutine());
        }
    }

    IEnumerator StepCoroutine()
    {
        float moveSpeed = 500;
        float stepTimer = 0.15f;
        

        while (stepTimer > 0)
        {
            stepTimer -= Time.fixedDeltaTime;
            //rb.velocity = new Vector3(stepDirection.x * Time.fixedDeltaTime * moveSpeed, 0, stepDirection.z * Time.fixedDeltaTime * moveSpeed);
            rb.MovePosition(transform.position + stepDirection.normalized * Time.fixedDeltaTime * moveSpeed / 100);
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector3.zero;
    }
}
