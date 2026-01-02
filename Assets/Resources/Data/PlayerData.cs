using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public enum UnlockableAbilities
{
    BLOCK, SPECIAL_ATTACK, MORE_PATCHES_1, MORE_PATCHES_2
}

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public string saveFile;
    public string date;
    public string time;

    public int health;
    public bool hasHealthGem;
    public int maxHealCharges;
    public int healCharges;
    public int currentGemShards;
    public List<string> gemShards;
    public bool hasSpawned;
    public int lastSwordSite;
    public List<UnlockableAbilities> unlockedAbilities;
    public List<Patches> patches;
    public List<Patches> equippedPatches;
    public int maxPatches;
    public List<string> tutorials;
    public List<string> evidenceFound;
    public bool hasWayfaerie;
    public int money;
    public int lostMoney;

    public int strength;
    public int dexterity;
    public int vitality;
    public int arcane;

    public float maxMana = 50;
    public float mana;

    public int deathNum;
    public int killedEnemiesNum;
    public int killedEnemiesAtGetShield;

    public Vector2 moveDir;

    public List<int> unlockedWeapons;
    public List<WeaponElement> unlockedSwords;
    public List<WeaponElement> unlockedLanterns;
    public List<WeaponElement> unlockedKnives;
    public List<WeaponElement> unlockedClaws;

    public int currentWeapon;
    public bool newWeapon;
    public WeaponElement[] equippedElements;

    public float swordSpecialTimer;

    public Dictionary<UnlockableAbilities, string> unlockToString = new Dictionary<UnlockableAbilities, string>
    {
        { UnlockableAbilities.BLOCK, "Block" },
        {UnlockableAbilities.SPECIAL_ATTACK, "Special Attack" },
        {UnlockableAbilities.MORE_PATCHES_1, "More Patches 1" },
        {UnlockableAbilities.MORE_PATCHES_2, "More Patches 2" },
    };

    public Dictionary<string, UnlockableAbilities> stringToUnlock = new Dictionary<string, UnlockableAbilities>
    {
        { "Block", UnlockableAbilities.BLOCK },
        { "Special Attack", UnlockableAbilities.SPECIAL_ATTACK },
        { "More Patches 1" , UnlockableAbilities.MORE_PATCHES_1},
        { "More Patches 2", UnlockableAbilities.MORE_PATCHES_2 },
    };

    public Dictionary<WeaponElement, string> elementToString = new Dictionary<WeaponElement, string>
    {
        { WeaponElement.DEFAULT , "default" },
        { WeaponElement.FIRE, "fire" },
        { WeaponElement.ELECTRICITY, "electricity" },
        { WeaponElement.ICE, "ice" },
        { WeaponElement.CHAOS, "chaos" },
    };

    public Dictionary<string, WeaponElement> stringToElement = new Dictionary<string, WeaponElement>
    {
        { "default", WeaponElement.DEFAULT },
        { "fire", WeaponElement.FIRE },
        { "electricity", WeaponElement.ELECTRICITY },
        { "ice", WeaponElement.ICE },
        { "chaos", WeaponElement.CHAOS },
    };

    public List<string> GetUnlockedStrings()
    {
        List<string> unlockedStrings = new List<string>();
        foreach(UnlockableAbilities ability in unlockedAbilities)
        {
            unlockedStrings.Add(unlockToString[ability]);
        }
        return unlockedStrings;
    }

    public void SetUnlocksWithStrings(List<string> unlockedStrings)
    {
        unlockedAbilities.Clear();
        foreach(string unlockedString in unlockedStrings)
        {
            unlockedAbilities.Add(stringToUnlock[unlockedString]);
        }
    }

    public List<WeaponElement> GetElementList(int index)
    {
        switch (index)
        {
            case 0: return unlockedSwords;
            case 1: return unlockedLanterns;
            case 2: return unlockedKnives;
            case 3: return unlockedClaws;
            default: return null;
        }
    }

    public List<WeaponElement> GetWeaponUnlockList(int weapon)
    {
        switch (weapon)
        {
            case 0: return unlockedSwords;
            case 1: return unlockedLanterns;
            case 2: return unlockedKnives;
            case 3: return unlockedClaws;
            default: return null;
        }
    }

    public List<string> GetStringsFromElements(List<WeaponElement> weaponElements)
    {
        return weaponElements.Select(element => elementToString[element]).ToList();
    }

    public List<WeaponElement> GetElementsFromStrings(List<string> strings)
    {
        return strings.Select(elementString => stringToElement[elementString]).ToList();
    }

    public int MaxHealth()
    {
        int maxHealth = 80 + vitality * 5;
        return maxHealth;
    }

    public int MaxStamina()
    {
        int maxStamina = 80 + dexterity * 5;
        return maxStamina;
    }

    public int PhysicalDamage()
    {
        int attackPower = 18 + strength;
        return attackPower;
    }

    public int ArcaneDamage()
    {
        int pathDamage = 18 + arcane;
        return pathDamage;
    }

    public int GetLevel()
    {
        int level = strength + dexterity + vitality + arcane - 3;
        return level;
    }

    // TODO: refactor tutorial list so it can be reset from here. For now tutorials must be reset outside this SO
    public void ClearData()
    {
        hasHealthGem = false;
        maxHealCharges = 1;
        healCharges = 1;
        currentGemShards = 0;
        gemShards.Clear();
        lastSwordSite = 1;
        unlockedAbilities.Clear();
        patches.Clear();
        equippedPatches.Clear();
        maxPatches = 2;
        evidenceFound.Clear();
        hasWayfaerie = false;
        money = 0;
        lostMoney = 0;
        strength = 1;
        dexterity = 1;
        vitality = 1;
        arcane = 1;
        health = MaxHealth();
        maxMana = 50;
        mana = maxMana;
        deathNum = 0;
        killedEnemiesNum = 0;
        killedEnemiesAtGetShield = 0;
        unlockedWeapons.Clear();
        unlockedWeapons.Add(0);
        unlockedSwords.Clear();
        unlockedSwords.Add(WeaponElement.DEFAULT);
        unlockedLanterns.Clear();
        unlockedKnives.Clear();
        unlockedClaws.Clear();
        newWeapon = true;
        currentWeapon = 0;
        equippedElements = new WeaponElement[4];
        swordSpecialTimer = 0;
    }

    public void UnlockAllWeapons()
    {
        unlockedWeapons.Clear();
        unlockedWeapons.Add(0);
        unlockedWeapons.Add(1);
        unlockedWeapons.Add(2);
        unlockedWeapons.Add(3);

        unlockedSwords.Clear();
        unlockedSwords.Add(WeaponElement.DEFAULT);
        unlockedSwords.Add(WeaponElement.FIRE);

        unlockedLanterns.Clear();
        unlockedLanterns.Add(WeaponElement.FIRE);
        unlockedLanterns.Add(WeaponElement.ELECTRICITY);

        unlockedKnives.Clear();
        unlockedKnives.Add(WeaponElement.ELECTRICITY);
        unlockedKnives.Add(WeaponElement.ICE);

        unlockedClaws.Clear();
        unlockedClaws.Add(WeaponElement.ICE);
        unlockedClaws.Add(WeaponElement.CHAOS);

        equippedElements[0] = WeaponElement.DEFAULT;
        equippedElements[1] = WeaponElement.FIRE;
        equippedElements[2] = WeaponElement.ELECTRICITY;
        equippedElements[3] = WeaponElement.ICE;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
