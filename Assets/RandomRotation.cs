using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] bool randomize;
    float rotation;

    private void OnDrawGizmosSelected()
    {
        if (randomize)
        {
            rotation = Random.Range(0, 360);
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotation, transform.rotation.z);
            randomize = false;
        }
    }
}
