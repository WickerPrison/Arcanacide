using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
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
    public int money;
    public int lostMoney;

    public int strength;
    public int dexterity;
    public int vitality;
    public int dedication;

    public float maxMana = 50;
    public float mana;

    public int deathNum;
    public int killedEnemiesNum;

    public Vector2 moveDir;

    public List<int> unlockedWeapons;
    public int currentWeapon;

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

    public int AttackPower()
    {
        int attackPower = 18 + strength;
        return attackPower;
    }

    public int ArcaneDamage()
    {
        int pathDamage = 10 + 2 * dedication;
        return pathDamage;
    }

    public int GetLevel()
    {
        int level = strength + dexterity + vitality + dedication - 3;
        return level;
    }
}
