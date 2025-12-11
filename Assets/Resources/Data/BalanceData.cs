using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum BalanceWeaponType
{
    SWORD, LANTERN, KNIFE, CLAWS, FIRESWORD
}

public enum BalanceAttackType
{
    LIGHT, COMBO1, COMBO2, SPECIAL, HEAVY, HEAVY_NO_CHARGE
}

[CreateAssetMenu]
public class BalanceData : ScriptableObject
{
    public AnimationCurve swordLightDps;
    [SerializeField] AnimationCurve lanternLightDps;
    [SerializeField] AnimationCurve knifeLightDps;
    [SerializeField] AnimationCurve clawsLightDps;
    public AnimationCurve fireSwordLightDps;
    public float[] lightMaxDps;
    public float[] lightStamPerSecond;
    public float[] lightHitRate;
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
                    { BalanceWeaponType.FIRESWORD, fireSwordLightDps },
                };
            }
            return _lightDpsCurveDict;
        }
    }


    public AnimationCurve swordHeavyDps;
    public AnimationCurve lanternHeavyDps;
    public AnimationCurve knifeHeavyDps;
    public AnimationCurve clawsHeavyDps;
    public AnimationCurve fireSwordHeavyDps;
    public float[] heavyMaxDps;
    public float[] heavyStamPerSecond;
    public float[] heavyHitRate;
    Dictionary<BalanceWeaponType, AnimationCurve> _heavyDpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> heavyDpsCurveDict
    {
        get
        {
            if (_heavyDpsCurveDict == null)
            {
                _heavyDpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordHeavyDps },
                    { BalanceWeaponType.LANTERN, lanternHeavyDps },
                    { BalanceWeaponType.KNIFE, knifeHeavyDps },
                    { BalanceWeaponType.CLAWS, clawsHeavyDps },
                    { BalanceWeaponType.FIRESWORD, fireSwordHeavyDps },
                };
            }
            return _heavyDpsCurveDict;
        }
    }

    public AnimationCurve swordHeavyDpsNoCharge;
    public AnimationCurve clawsHeavyDpsNoCharge;
    public AnimationCurve fireSwordHeavyDpsNoCharge;
    public float[] heavyNoChargeMaxDps;
    public float[] heavyNoChargeStamPerSecond;
    public float[] heavyNoChargeHitRate;
    Dictionary<BalanceWeaponType, AnimationCurve> _heavyNoChargeDpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> heavyNoChargeDpsCurveDict
    {
        get
        {
            if (_heavyNoChargeDpsCurveDict == null)
            {
                _heavyNoChargeDpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordHeavyDpsNoCharge },
                    { BalanceWeaponType.CLAWS, clawsHeavyDpsNoCharge },
                    { BalanceWeaponType.FIRESWORD, fireSwordHeavyDpsNoCharge },
                };
            }
            return _heavyNoChargeDpsCurveDict;
        }
    }


    [SerializeField] AnimationCurve swordCombo1Dps;
    [SerializeField] AnimationCurve lanternCombo1Dps;
    [SerializeField] AnimationCurve knifeCombo1Dps;
    [SerializeField] AnimationCurve clawsCombo1Dps;
    [SerializeField] AnimationCurve fireSwordCombo1Dps;
    public float[] combo1MaxDps;
    public float[] combo1StamPerSecond;
    public float[] combo1HitRate;
    Dictionary<BalanceWeaponType, AnimationCurve> _combo1DpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> combo1DpsCurveDict
    {
        get
        {
            if (_combo1DpsCurveDict == null)
            {
                _combo1DpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordCombo1Dps },
                    { BalanceWeaponType.LANTERN, lanternCombo1Dps },
                    { BalanceWeaponType.KNIFE, knifeCombo1Dps },
                    { BalanceWeaponType.CLAWS, clawsCombo1Dps },
                    { BalanceWeaponType.FIRESWORD, fireSwordCombo1Dps },
                };
            }
            return _combo1DpsCurveDict;
        }
    }

    [SerializeField] AnimationCurve swordCombo2Dps;
    [SerializeField] AnimationCurve lanternCombo2Dps;
    [SerializeField] AnimationCurve knifeCombo2Dps;
    [SerializeField] AnimationCurve clawsCombo2Dps;
    [SerializeField] AnimationCurve fireSwordCombo2Dps;
    public float[] combo2MaxDps;
    public float[] combo2StamPerSecond;
    public float[] combo2HitRate;
    Dictionary<BalanceWeaponType, AnimationCurve> _combo2DpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> combo2DpsCurveDict
    {
        get
        {
            if (_combo2DpsCurveDict == null)
            {
                _combo2DpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordCombo2Dps },
                    { BalanceWeaponType.LANTERN, lanternCombo2Dps },
                    { BalanceWeaponType.KNIFE, knifeCombo2Dps },
                    { BalanceWeaponType.CLAWS, clawsCombo2Dps },
                    { BalanceWeaponType.FIRESWORD, fireSwordCombo2Dps },
                };
            }
            return _combo2DpsCurveDict;
        }
    }

    public AnimationCurve swordSpecialDps;
    public AnimationCurve knifeSpecialDps;
    public AnimationCurve fireSwordSpecialDps;
    public float[] specialMaxDps;
    public float[] specialStamPerSecond;
    public float[] specialManaPerSecond;
    public float[] specialHitRate;

    Dictionary<BalanceWeaponType, AnimationCurve> _specialDpsCurveDict;
    Dictionary<BalanceWeaponType, AnimationCurve> specialDpsCurveDict
    {
        get
        {
            if (_specialDpsCurveDict == null)
            {
                _specialDpsCurveDict = new Dictionary<BalanceWeaponType, AnimationCurve>()
                {
                    { BalanceWeaponType.SWORD, swordSpecialDps },
                    { BalanceWeaponType.KNIFE, knifeSpecialDps },
                    { BalanceWeaponType.FIRESWORD, fireSwordSpecialDps },
                };
            }
            return _specialDpsCurveDict;
        }
    }

    Dictionary<BalanceAttackType, Dictionary<BalanceWeaponType, AnimationCurve>> _dictDict;
    Dictionary<BalanceAttackType, Dictionary<BalanceWeaponType, AnimationCurve>> dictDict
    {
        get
        {
            if (_dictDict == null)
            {
                _dictDict = new Dictionary<BalanceAttackType, Dictionary<BalanceWeaponType, AnimationCurve>>()
                {
                    { BalanceAttackType.LIGHT, lightDpsCurveDict },
                    { BalanceAttackType.COMBO1, combo1DpsCurveDict },
                    { BalanceAttackType.COMBO2, combo2DpsCurveDict },
                    { BalanceAttackType.SPECIAL, specialDpsCurveDict },
                    { BalanceAttackType.HEAVY, heavyDpsCurveDict },
                    { BalanceAttackType.HEAVY_NO_CHARGE, heavyNoChargeDpsCurveDict },
                };
            }
            return _dictDict;
        }
    }


    public void SetDps(int stat, float dps, BalanceAttackType attackType, BalanceWeaponType weaponType)
    {
        dictDict[attackType][weaponType].AddKey(stat, dps);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void ClearDps(BalanceAttackType attackType, BalanceWeaponType weaponType)
    {
        dictDict[attackType][weaponType].keys = new Keyframe[0];
    }
}
