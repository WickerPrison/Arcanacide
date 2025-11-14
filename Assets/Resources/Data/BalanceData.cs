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
    public float[] lightMaxDps;
    public float[] lightStamPerSecond;
    public float[] lightHitRate;

    public AnimationCurve swordHeavyDps;
    public AnimationCurve lanternHeavyDps;
    public AnimationCurve knifeHeavyDps;
    public AnimationCurve clawsHeavyDps;
    public float[] heavyMaxDps;
    public float[] heavyStamPerSecond;
    public float[] heavyHitRate;

    Dictionary<BalanceWeaponType, AnimationCurve> _lightDpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> lightDpsCurveDict
    {
        get
        {
            if(_lightDpsCurveDict == null)
            {
                _lightDpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordLightDps },
                    { BalanceWeaponType.LANTERN, lanternLightDps },
                    { BalanceWeaponType.KNIFE, knifeLightDps },
                    { BalanceWeaponType.CLAWS, clawsLightDps },
                };
            }
            return _lightDpsCurveDict;
        }
    }

    public void SetDps(int stat, float dps, BalanceWeaponType weaponType)
    {
        lightDpsCurveDict[weaponType].AddKey(stat, dps);
    }

    public void ClearDps(BalanceWeaponType weaponType)
    {
        lightDpsCurveDict[weaponType].keys = new Keyframe[0];
    }
}
