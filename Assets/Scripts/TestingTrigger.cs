using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestingTrigger : MonoBehaviour
{
    [System.NonSerialized] public Func<Collider, bool> callback;
    [System.NonSerialized] public int counter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(callback == null)
        {
            throw new Exception("A callback of type Func<Collider, bool> must be provided");
        }

        if (callback(other))
        {
            counter++;
        }
    }
}
