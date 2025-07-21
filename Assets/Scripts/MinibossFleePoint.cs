using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossFleePoint : MonoBehaviour
{
    [System.NonSerialized] public Vector3 direction;

    private void Awake()
    {
        MinibossAbilities minibossAbilities = GameObject.FindObjectOfType<MinibossAbilities>();
        minibossAbilities.fleePoints.Add(transform);
        direction = -transform.position;
    }
}
