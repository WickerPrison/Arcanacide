using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int maxHealth;
    public float maxStamina;
    public int attackPower;
    public bool hasHealthGem;
    public int maxHealCharges;
    public int healCharges;
    public int lastSwordSite;
    public List<string> unlockedAbilities;
    public List<string> emblems;
    public List<string> equippedEmblems;
    public int maxPatches;
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
    public List<int> unlockedWeapons;
    public int currentWeapon;

    public int[] unlockedDoors;
    public int[] visitedRooms;
    public int[] powerSwitchesFlipped;
    public string deathRoom;
    public float[] deathPosition;
    public bool ticketFiled;
    public bool fireBossKilled;
    public bool secretaryConvo;
    public bool electricBossKilled;
    public bool iceBossKilled;
    public float[] iceBossPosition;
    public int iceBossDirection;
    public bool ACOn;

    public int[] ORTHODOXQueue;
    public int[] ORTHODOXPreviousConversations;
    public int[] TRENCHQueue;
    public int[] TRENCHPreviousConversations;
    public int[] QuestionMarksQueue;
    public int[] QuestionMarksPreviousConversations;
    public int[] HeadOfITQueue;
    public int[] HeadOfITPreviousConversations;
    public int[] conversationsHad;
    public int[] patchworkGaryConversations;

    public SaveData (PlayerData playerData, MapData mapData, DialogueData dialogueData)
    {
        hasHealthGem = playerData.hasHealthGem;
        maxHealCharges = playerData.maxHealCharges;
        healCharges = playerData.healCharges;
        lastSwordSite = playerData.lastSwordSite;
        unlockedAbilities = playerData.unlockedAbilities;
        emblems = playerData.emblems;
        equippedEmblems = playerData.equippedEmblems;
        maxPatches = playerData.maxPatches;
        tutorials = playerData.tutorials;
        money = playerData.money;
        lostMoney = playerData.lostMoney;
        strength = playerData.strength;
        dexterity = playerData.dexterity;
        vitality = playerData.vitality;
        dedication = playerData.dedication;
        maxMana = playerData.maxMana;
        mana = playerData.mana;
        deathNum = playerData.deathNum;
        killedEnemiesNum = playerData.killedEnemiesNum;
        unlockedWeapons = playerData.unlockedWeapons;
        currentWeapon = playerData.currentWeapon;


        unlockedDoors = mapData.unlockedDoors.ToArray();
        visitedRooms = mapData.visitedRooms.ToArray();
        powerSwitchesFlipped = mapData.powerSwitchesFlipped.ToArray();
        deathRoom = mapData.deathRoom;
        deathPosition = new float[3];
        deathPosition[0] = mapData.deathPosition.x;
        deathPosition[1] = mapData.deathPosition.y;
        deathPosition[2] = mapData.deathPosition.z;
        ticketFiled = mapData.ticketFiled;
        fireBossKilled = mapData.fireBossKilled;
        secretaryConvo = mapData.secretaryConvo;
        electricBossKilled = mapData.electricBossKilled;
        iceBossKilled = mapData.iceBossKilled;
        iceBossPosition[0] = mapData.iceBossPosition.x;
        iceBossPosition[1] = mapData.iceBossPosition.y;
        iceBossPosition[2] = mapData.iceBossPosition.z;
        iceBossDirection = mapData.iceBossDirection;

        ACOn = mapData.ACOn;

        ORTHODOXQueue = dialogueData.ORTHODOXQueue.ToArray();
        ORTHODOXPreviousConversations = dialogueData.ORTHODOXPreviousConversations.ToArray();
        TRENCHQueue = dialogueData.TRENCHQueue.ToArray();
        TRENCHPreviousConversations = dialogueData.TRENCHPreviousConversations.ToArray();
        QuestionMarksQueue = dialogueData.UnknownNumberQueue.ToArray();
        QuestionMarksPreviousConversations = dialogueData.UnknownNumberPreviousConversations.ToArray();
        conversationsHad = dialogueData.conversationsHad.ToArray();
        patchworkGaryConversations = dialogueData.patchworkGaryConversations.ToArray();
    }
}
