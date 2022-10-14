using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int maxHealth;
    public float maxStamina;
    public int attackPower;
    public int maxHealCharges;
    public int healCharges;
    public int lastSwordSite;
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
    public float maxMana;
    public float mana;
    public int deathNum;
    public int killedEnemiesNum;

    public int[] unlockedDoors;
    public int[] visitedRooms;
    public string deathRoom;
    public float[] deathPosition;
    public bool ticketFiled;
    public bool fireBossKilled;
    public bool secretaryConvo;

    public List<int> ORTHODOXQueue;
    public List<int> ORTHODOXPreviousConversations;
    public List<int> TRENCHQueue;
    public List<int> TRENCHPreviousConversations;
    public List<int> QuestionMarksQueue;
    public List<int> QuestionMarksPreviousConversations;
    public List<int> HeadOfITQueue;
    public List<int> HeadOfITPreviousConversations;

    public SaveData (PlayerData playerData, MapData mapData, PhoneData phoneData)
    {
        maxHealCharges = playerData.maxHealCharges;
        healCharges = playerData.healCharges;
        lastSwordSite = playerData.lastSwordSite;
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
        maxMana = playerData.maxMana;
        mana = playerData.mana;
        deathNum = playerData.deathNum;
        killedEnemiesNum = playerData.killedEnemiesNum;


        unlockedDoors = mapData.unlockedDoors.ToArray();
        visitedRooms = mapData.visitedRooms.ToArray();
        deathRoom = mapData.deathRoom;
        deathPosition = new float[3];
        deathPosition[0] = mapData.deathPosition.x;
        deathPosition[1] = mapData.deathPosition.y;
        deathPosition[2] = mapData.deathPosition.z;
        ticketFiled = mapData.ticketFiled;
        fireBossKilled = mapData.fireBossKilled;
        secretaryConvo = mapData.secretaryConvo;

        ORTHODOXQueue = phoneData.ORTHODOXQueue;
        ORTHODOXPreviousConversations = phoneData.ORTHODOXPreviousConversations;
        TRENCHQueue = phoneData.TRENCHQueue;
        TRENCHPreviousConversations = phoneData.TRENCHPreviousConversations;
        QuestionMarksQueue = phoneData.UnknownNumberQueue;
        QuestionMarksPreviousConversations = phoneData.UnknownNumberPreviousConversations;
        HeadOfITQueue = phoneData.HeadOfITQueue;
        HeadOfITPreviousConversations = phoneData.HeadOfITPreviousConversations;
    }
}
