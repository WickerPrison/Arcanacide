using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosSummonAnimEvents : MonoBehaviour
{
    ChaosSummon chaosSummon;

    private void Start()
    {
        chaosSummon = GetComponentInParent<ChaosSummon>();
    }

    public void ShowIndicator()
    {
        chaosSummon.ShowIndicator();
    }

    public void HideIndicator()
    {
        chaosSummon.HideIndicator();
    }

    public void Attack()
    {
        chaosSummon.Attack();
    }

    public void DestroySummon()
    {
        chaosSummon.GoAway();
    }
}
