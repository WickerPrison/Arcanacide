using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BalanceTestUtils
{
    public static Dictionary<BalanceWeaponType, int> weaponIndexDict = new Dictionary<BalanceWeaponType, int>
    {
        { BalanceWeaponType.SWORD, 0 },
        { BalanceWeaponType.LANTERN, 1 },
        { BalanceWeaponType.KNIFE, 2 },
        { BalanceWeaponType.CLAWS, 3 },
        { BalanceWeaponType.FIRESWORD, 4 },
        { BalanceWeaponType.ELECTRICLANTERN, 5 },
        { BalanceWeaponType.ICEKNIFE, 6 },
    };

    public static Dictionary<BalanceWeaponType, WeaponElement> weaponElementDict = new Dictionary<BalanceWeaponType, WeaponElement>
    {
        { BalanceWeaponType.SWORD, WeaponElement.DEFAULT },
        { BalanceWeaponType.LANTERN, WeaponElement.FIRE },
        { BalanceWeaponType.KNIFE, WeaponElement.ELECTRICITY },
        { BalanceWeaponType.CLAWS, WeaponElement.ICE },
        { BalanceWeaponType.FIRESWORD, WeaponElement.FIRE },
        { BalanceWeaponType.ELECTRICLANTERN, WeaponElement.ELECTRICITY },
        { BalanceWeaponType.ICEKNIFE, WeaponElement.ICE },
    };
}
