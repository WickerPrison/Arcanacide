using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefundShardRotation : MonoBehaviour
{
    [SerializeField] float speed;
    float rotation;
    private void Start()
    {
        rotation = Random.Range(-1f, 1f);
    }

    private void FixedUpdate()
    {
        transform.Rotate(transform.forward, rotation * speed * Time.fixedDeltaTime, Space.World);
    }
}
