using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRainTestingTrigger : MonoBehaviour
{

    [System.NonSerialized] public int counter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FireRain>())
        {
            counter++;
        }
    }
}
