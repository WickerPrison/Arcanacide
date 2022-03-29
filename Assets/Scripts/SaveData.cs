using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int maxHealth;
    public float maxStamina;
    public int attackPower;
    public int lastAltar;
    public List<string> unlockedAbilities;
    public List<string> emblems;
    public List<string> equippedEmblems;
    public List<string> tutorials;
    public int money;
    public int lostMoney;
    public string path;
    public int pathDamage;
    public int strength;
    public int dexterity;
    public int vitality;
    public int dedication;

    public int[] unlockedDoors;
    public string deathRoom;
    public float[] deathPosition;
    public bool ticketFiled;
    public bool fireBossKilled;
    public bool boughtDamage;
    public bool boughtHealth;
    public bool boughtStamina;

    public SaveData (PlayerData playerData, MapData mapData)
    {
        lastAltar = playerData.lastAltar;
        unlockedAbilities = playerData.unlockedAbilities;
        emblems = playerData.emblems;
        equippedEmblems = playerData.equippedEmblems;
        tutorials = playerData.tutorials;
        money = playerData.money;
        lostMoney = playerData.lostMoney;
        path = playerData.path;
        strength = playerData.strength;
        dexterity = playerData.dexterity;
        vitality = playerData.vitality;
        dedication = playerData.dedication;


        unlockedDoors = mapData.unlockedDoors.ToArray();
        deathRoom = mapData.deathRoom;
        deathPosition = new float[3];
        deathPosition[0] = mapData.deathPosition.x;
        deathPosition[1] = mapData.deathPosition.y;
        deathPosition[2] = mapData.deathPosition.z;
        ticketFiled = mapData.ticketFiled;
        fireBossKilled = mapData.fireBossKilled;
    }
}
