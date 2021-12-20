using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : MonoBehaviour
{
    float moveSpeed = 7;
    public Vector3 target;
    Vector3 moveDirection;

    private void Start()
    {
        transform.LookAt(target);
        moveDirection = target - transform.position;
    }

    void FixedUpdate()
    {
        transform.position += (moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
