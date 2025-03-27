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
        currentWeapon = 0;
        swordSpecialTimer = 0;
        clawSpecialOn = false;
        clawSpecialTimer = 0;
    }
}
