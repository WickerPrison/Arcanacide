using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool hasSpawned;
    public int lastSwordSite;
    public List<UnlockableAbilities> unlockedAbilities;
    public List<string> emblems;
    public List<string> equippedEmblems;
    public int maxPatches;
    public List<string> tutorials;
    public List<string> evidenceFound;
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
    public int currentWeapon;

    public float swordSpecialTimer;
    public bool clawSpecialOn;
    public float clawSpecialTimer;

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
}
