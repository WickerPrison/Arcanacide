using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpecialAttackData : BalanceAttackData
{
    public float[] manaPerSecond;

    public void SetManaPerSec(float value, int weaponType)
    {
        manaPerSecond[weaponType] = value;
    }
}
