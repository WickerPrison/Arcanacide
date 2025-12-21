using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum BalanceWeaponType
{
    SWORD, LANTERN, KNIFE, CLAWS, FIRESWORD, ELECTRICLANTERN
}

public enum BalanceAttackType
{
    LIGHT, COMBO1, COMBO2, SPECIAL, HEAVY, HEAVY_NO_CHARGE
}

[CreateAssetMenu]
public class BalanceData : ScriptableObject
{
    [SerializeField] BalanceAttackData light;
    [SerializeField] BalanceAttackData heavy;
    [SerializeField] BalanceAttackData heavyNoCharge;
    [SerializeField] BalanceAttackData combo1;
    [SerializeField] BalanceAttackData combo2;
    [SerializeField] BalanceAttackData special;
    Dictionary<BalanceAttackType, BalanceAttackData> _typeDict;
    Dictionary<BalanceAttackType, BalanceAttackData> typeDict
    {
        get
        {
            if (_typeDict == null)
            {
                _typeDict = new Dictionary<BalanceAttackType, BalanceAttackData>()
                {
                    { BalanceAttackType.LIGHT, light },
                    { BalanceAttackType.HEAVY, heavy },
                    { BalanceAttackType.HEAVY_NO_CHARGE, heavyNoCharge },
                    { BalanceAttackType.COMBO1, combo1 },
                    { BalanceAttackType.COMBO2, combo2 },
                    { BalanceAttackType.SPECIAL, special },
                };
            }
            return _typeDict;
        }
    }

    public void SetDps(int stat, float dps, BalanceAttackType attackType, BalanceWeaponType weaponType)
    {
        typeDict[attackType].SetDps(stat, dps, weaponType);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void ClearDps(BalanceAttackType attackType, BalanceWeaponType weaponType)
    {
        typeDict[attackType].ClearDps(weaponType);
    }

    public void SetMaxDps(float value, int weaponType, BalanceAttackType attackType)
    {
        typeDict[attackType].SetMaxDps(value, weaponType);
    }

    public void SetStamPerSecond(float value, int weaponType, BalanceAttackType attackType)
    {
        typeDict[attackType].SetStamPerSecond(value, weaponType);
    }

    public void SetHitRate(float value, int weaponType, BalanceAttackType attackType)
    {
        typeDict[attackType].SetHitRate(value, weaponType);
    }

    public void SetSpecialManaPerSec(float value, int weaponType)
    {
        ((SpecialAttackData)typeDict[BalanceAttackType.SPECIAL]).SetManaPerSec(value, weaponType);
    }
}
