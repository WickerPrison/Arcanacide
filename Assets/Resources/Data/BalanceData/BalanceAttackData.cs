using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class BalanceAttackData : ScriptableObject
{
    public AnimationCurve swordDps;
    public AnimationCurve lanternDps;
    public AnimationCurve knifeDps;
    public AnimationCurve clawsDps;
    public AnimationCurve fireSwordDps;
    public AnimationCurve electricLanternDps;
    public AnimationCurve iceKnifeDps;
    public AnimationCurve chaosClawsDps;
    public float[] maxDps;
    public float[] stamPerSecond;
    public float[] hitRate;
    Dictionary<BalanceWeaponType, AnimationCurve> _dpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> dpsCurveDict
    {
        get
        {
            if (_dpsCurveDict == null)
            {
                _dpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordDps },
                    { BalanceWeaponType.LANTERN, lanternDps },
                    { BalanceWeaponType.KNIFE, knifeDps },
                    { BalanceWeaponType.CLAWS, clawsDps },
                    { BalanceWeaponType.FIRESWORD, fireSwordDps },
                    { BalanceWeaponType.ELECTRICLANTERN, electricLanternDps },
                    { BalanceWeaponType.ICEKNIFE, iceKnifeDps },
                    { BalanceWeaponType.CHAOSCLAWS, chaosClawsDps },
                };
            }
            return _dpsCurveDict;
        }
    }

    public void SetDps(int stat, float dps, BalanceWeaponType weaponType)
    {
        dpsCurveDict[weaponType].AddKey(stat, dps);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void ClearDps(BalanceWeaponType weaponType)
    {
        dpsCurveDict[weaponType].keys = new Keyframe[0];
    }

    public void SetMaxDps(float value, int weaponType)
    {
        maxDps[weaponType] = value;
    }

    public void SetStamPerSecond(float value, int weaponType)
    {
        stamPerSecond[weaponType] = value;
    }

    public void SetHitRate(float value, int weaponType)
    {
        hitRate[weaponType] = value;
    }
}
