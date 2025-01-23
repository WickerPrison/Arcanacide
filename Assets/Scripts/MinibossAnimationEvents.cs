using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossAnimationEvents : MonoBehaviour
{
    MinibossAbilities abilities;

    private void Start()
    {
        abilities = GetComponentInParent<MinibossAbilities>();
    }

    public void FireMissile()
    {
        abilities.FireMissiles();
    }
}
