using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BalanceWeaponType
{
    SWORD, LANTERN, KNIFE, CLAWS
}

[CreateAssetMenu]
public class BalanceData : ScriptableObject
{
    [SerializeField] AnimationCurve swordLightDps;
    [SerializeField] AnimationCurve lanternLightDps;
    [SerializeField] AnimationCurve knifeLightDps;
    [SerializeField] AnimationCurve clawsLightDps;
    public float[] maxDps;
    public float[] stamPerSecond;
    public float[] hitRate;

    Dictionary<BalanceWeaponType, AnimationCurve> _dpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> dpsCurveDict
    {
        get
        {
            if(_dpsCurveDict == null)
            {
                _dpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordLightDps },
                    { BalanceWeaponType.LANTERN, lanternLightDps },
                    { BalanceWeaponType.KNIFE, knifeLightDps },
                    { BalanceWeaponType.CLAWS, clawsLightDps },
                };
            }
            return _dpsCurveDict;
        }
    }


    public void SetDps(int stat, float dps, BalanceWeaponType weaponType)
    {
        dpsCurveDict[weaponType].AddKey(stat, dps);
    }

    public void ClearDps(BalanceWeaponType weaponType)
    {
        dpsCurveDict[weaponType].keys = new Keyframe[0];
    }
}
