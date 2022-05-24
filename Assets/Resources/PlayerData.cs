using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public int health;
    public int maxHealCharges;
    public int healCharges;
    public bool hasSpawned;
    public int lastSwordSite;
    public List<string> unlockedAbilities;
    public List<string> emblems;
    public List<string> equippedEmblems;
    public List<string> tutorials;
    public int money;
    public int lostMoney;

    public string path;

    public int strength;
    public int dexterity;
    public int vitality;
    public int dedication;

    public float maxMana = 50;
    public float mana;

    public Vector2 moveDir;


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

    public int PathDamage()
    {
        int pathDamage;
        switch (path)
        {
            case "Sword":
                pathDamage = 10 + 2 * dedication;
                return pathDamage;
            case "Dying":
                pathDamage = 10 + 3 * dedication;
                return pathDamage;
            case "Path":
                pathDamage = 2 + dedication;
                return pathDamage;
            default:
                pathDamage = 0;
                return pathDamage;
        }
    }

    public int GetLevel()
    {
        int level = strength + dexterity + vitality + dedication - 3;
        return level;
    }
}
