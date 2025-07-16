using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossFleePoint : MonoBehaviour
{
    private void Awake()
    {
        MinibossAbilities minibossAbilities = GameObject.FindObjectOfType<MinibossAbilities>();
        minibossAbilities.fleePoints.Add(transform);
    }
}
