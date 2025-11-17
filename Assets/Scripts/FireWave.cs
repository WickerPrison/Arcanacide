using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : MonoBehaviour
{
    public int boxNum;
    public float moveSpeed = 7;
    public Vector3 target;
    Vector3 moveDirection;
    float lifetime = 10;
    public EnemyScript enemyOfOrigin;

    private void Start()
    {
        transform.LookAt(target);
        moveDirection = target - transform.position;
    }

    private void Update()
    {
        if(lifetime > 0 && boxNum > 0)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        transform.position += (moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
