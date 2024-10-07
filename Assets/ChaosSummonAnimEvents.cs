using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosSummonAnimEvents : MonoBehaviour
{
    [SerializeField] GameObject summonObject;
    IGetSummoned summonScript;

    private void Start()
    {
        summonScript = summonObject.GetComponent<IGetSummoned>();
    }

    public void ShowIndicator()
    {
        summonScript.ShowIndicator();
    }

    public void HideIndicator()
    {
        summonScript.HideIndicator();
    }

    public void Attack()
    {
        summonScript.Attack();
    }

    public void DestroySummon()
    {
        summonScript.GoAway();
    }
}
