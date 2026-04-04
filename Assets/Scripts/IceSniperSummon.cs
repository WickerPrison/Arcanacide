using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSniperSummon : ChaosSummon
{
    public void GetSummoned(int whichSide, Vector3 direction)
    {
        Vector3 perp = Vector3.Cross(direction, Vector3.up).normalized;
        transform.position = enemyScript.transform.position + direction * 3f + perp * 4f * whichSide;
    }
}
