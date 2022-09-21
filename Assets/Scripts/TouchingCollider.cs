using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingCollider : MonoBehaviour
{
    List<Collider> touchingObjects = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        touchingObjects.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        touchingObjects.Remove(other);
    }

    public List<Collider> GetTouchingObjects()
    {
        return touchingObjects;
    }
}
