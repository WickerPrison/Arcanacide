using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinMovement : MonoBehaviour
{
    Vector3 startingPoint;
    [SerializeField] float perlinSpeed = 0.3f;
    [SerializeField] float speed = 0.1f;
    [SerializeField] float damping = 0.1f;
    [SerializeField] Vector2 perlinOffsets;

    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = startingPoint - transform.position;
        float xMag = GetAxisMovement(perlinOffsets.x, direction.x);
        float yMag = GetAxisMovement(perlinOffsets.y, direction.y);
        transform.position = new Vector3(transform.position.x + xMag, transform.position.y + yMag, transform.position.z);
    }

    float GetAxisMovement(float offset, float direction)
    {
        float mag = Mathf.PerlinNoise(Time.time * perlinSpeed + offset, Time.time * perlinSpeed);
        mag += Mathf.Sign(direction) * damping - 0.5f;
        mag *= Time.fixedDeltaTime * speed;
        return mag;
    }
}
