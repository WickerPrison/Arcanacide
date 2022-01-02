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
    public bool hasBlock;
    public int money;
    public int lostMoney;

    public int[] unlockedDoors;
    public string deathRoom;
    public float[] deathPosition;
    public bool fireBossKilled;
    public bool boughtDamage;
    public bool boughtHealth;
    public bool boughtStamina;

    public SaveData (PlayerData playerData, MapData mapData)
    {
        maxHealth = playerData.maxHealth;
        maxStamina = playerData.maxStamina;
        attackPower = playerData.attackPower;
        lastAltar = playerData.lastAltar;
        hasBlock = playerData.hasBlock;
        money = playerData.money;
        lostMoney = playerData.lostMoney;

        unlockedDoors = mapData.unlockedDoors.ToArray();
        deathRoom = mapData.deathRoom;
        deathPosition = new float[3];
        deathPosition[0] = mapData.deathPosition.x;
        deathPosition[1] = mapData.deathPosition.y;
        deathPosition[2] = mapData.deathPosition.z;
        fireBossKilled = mapData.fireBossKilled;
        boughtDamage = mapData.boughtDamage;
        boughtHealth = mapData.boughtHealth;
        boughtStamina = mapData.boughtStamina;
    }
}
