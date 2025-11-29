using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class PlayerTools
{
    static PlayerData _playerData;
    static PlayerData playerData
    {
        get
        {
            if (_playerData == null)
            {
                _playerData = Resources.Load<PlayerData>("Data/PlayerData");
            }
            return _playerData;
        }
    }

    [MenuItem("Tools/Unlock All Weapons")]
    public static void UnlockAllWeapons()
    {
        playerData.unlockedWeapons.Clear();
        playerData.unlockedWeapons.Add(0);
        playerData.unlockedWeapons.Add(1);
        playerData.unlockedWeapons.Add(2);
        playerData.unlockedWeapons.Add(3);

        playerData.unlockedSwords.Clear();
        playerData.unlockedSwords.Add(WeaponElement.DEFAULT);
        playerData.unlockedSwords.Add(WeaponElement.FIRE);

        playerData.unlockedLanterns.Clear();
        playerData.unlockedLanterns.Add(WeaponElement.FIRE);

        playerData.unlockedKnives.Clear();
        playerData.unlockedKnives.Add(WeaponElement.ELECTRICITY);

        playerData.unlockedClaws.Clear();
        playerData.unlockedClaws.Add(WeaponElement.CHAOS);

        playerData.equippedElements[0] = WeaponElement.DEFAULT;
        playerData.equippedElements[1] = WeaponElement.FIRE;
        playerData.equippedElements[2] = WeaponElement.ELECTRICITY;
        playerData.equippedElements[3] = WeaponElement.ICE;
    }

    [MenuItem("Tools/Reset Weapons")]
    public static void ResetWeapons()
    {
        playerData.unlockedWeapons.Clear();
        playerData.unlockedWeapons.Add(0);

        playerData.unlockedSwords.Clear();
        playerData.unlockedSwords.Add(WeaponElement.DEFAULT);

        playerData.unlockedLanterns.Clear();
        playerData.unlockedKnives.Clear();
        playerData.unlockedClaws.Clear();

        playerData.equippedElements[0] = WeaponElement.DEFAULT;
        playerData.equippedElements[1] = WeaponElement.FIRE;
        playerData.equippedElements[2] = WeaponElement.ELECTRICITY;
        playerData.equippedElements[3] = WeaponElement.ICE;
        playerData.currentWeapon = 0;
    }
}
