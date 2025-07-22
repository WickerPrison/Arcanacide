using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossStalagmiteAttack : MonoBehaviour
{
    StalagmiteAttack stalagmiteAttack;

    private void Start()
    {
        stalagmiteAttack = GetComponentInParent<StalagmiteAttack>();
    }

    public void TriggerStalagmite()
    {
        if (stalagmiteAttack.isTriggered) return;
        stalagmiteAttack.TriggerNoNavmesh();
    }
}
