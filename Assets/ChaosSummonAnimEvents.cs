using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosSummonAnimEvents : MonoBehaviour
{
    ChaosSummon chaosSummon;
    SpriteEffects effects;

    private void Start()
    {
        chaosSummon = GetComponentInParent<ChaosSummon>();
        effects = GetComponentInParent<SpriteEffects>();
        effects.SetDissolve(0);
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
        effects.SetDissolve(0);
    }

    public void DissolveIn()
    {
        StartCoroutine(effects.UnDissolve(0.25f));
    }

    public void DissolveOut()
    {
        StartCoroutine(effects.Dissolve(0.25f));
    }
}
