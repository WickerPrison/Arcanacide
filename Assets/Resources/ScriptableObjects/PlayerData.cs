using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<string> unlockedAbilities;
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
