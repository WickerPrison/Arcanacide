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
    LIGHT, COMBO1, COMBO2
}

[CreateAssetMenu]
public class BalanceData : ScriptableObject
{
    [SerializeField] AnimationCurve swordLightDps;
    [SerializeField] AnimationCurve lanternLightDps;
    [SerializeField] AnimationCurve knifeLightDps;
    [SerializeField] AnimationCurve clawsLightDps;
    [SerializeField] AnimationCurve fireSwordLightDps;
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


    Dictionary<BalanceAttackType, Dictionary<BalanceWeaponType, AnimationCurve>> _dictDict;
    Dictionary<BalanceAttackType, Dictionary<BalanceWeaponType, AnimationCurve>> dictDict
    {
        get
        {
            if(_dictDict == null)
            {
                _dictDict = new Dictionary<BalanceAttackType, Dictionary<BalanceWeaponType, AnimationCurve>>()
                {
                    { BalanceAttackType.LIGHT, lightDpsCurveDict },
                    { BalanceAttackType.COMBO1, combo1DpsCurveDict },
                    { BalanceAttackType.COMBO2, combo2DpsCurveDict },
                };
            }
            return _dictDict;
        }
    }

    public AnimationCurve knifeSpecialDps;
    public float knifeSpecialMaxDps;
    public float knifeSpecialStamPerSecond;
    public float knifeSpecialManaPerSecond;


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
