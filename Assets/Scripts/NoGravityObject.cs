using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGravityObject : MonoBehaviour
{
    float xRotation;
    float yRotation;
    float zRotation;
    float rotateSpeed;
    Vector3 rotateVector;

    // Start is called before the first frame update
    void Start()
    {
        xRotation = Random.Range(-1f, 1f);
        yRotation = Random.Range(-1f, 1f);
        zRotation = Random.Range(-1f, 1f);
        rotateSpeed = Random.Range(1.5f, 2.5f);
        rotateVector = rotateSpeed * Time.deltaTime * new Vector3(xRotation, yRotation, zRotation).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateVector);
    }
}
