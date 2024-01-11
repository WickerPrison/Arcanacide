using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveFile;
    public string date;
    public string time;
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
    public List<string> evidenceFound;
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
    public int killedEnemiesAtGetShield;
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
    public bool whistleblowerArrested;

    public List<string> conversationsHad;
    public int[] directorWilkinsQueue;
    public int[] directorWilkinsPreviousConversations;
    public int[] agentFreiQueue;
    public int[] agentFreiPreviousConversations;
    public int[] bonsaiQueue;
    public int[] bonsaiPreviousConversations;
    public int[] QuestionMarksQueue;
    public int[] QuestionMarksPreviousConversations;
    public int[] patchworkGaryConversations;
    public int[] whistleblowerConversations;

    public SaveData (PlayerData playerData, MapData mapData, DialogueData dialogueData)
    {
        saveFile = playerData.saveFile;
        date = playerData.date;
        time = playerData.time;
        hasHealthGem = playerData.hasHealthGem;
        maxHealCharges = playerData.maxHealCharges;
        healCharges = playerData.healCharges;
        lastSwordSite = playerData.lastSwordSite;
        unlockedAbilities = playerData.unlockedAbilities;
        emblems = playerData.emblems;
        equippedEmblems = playerData.equippedEmblems;
        maxPatches = playerData.maxPatches;
        tutorials = playerData.tutorials;
        evidenceFound = playerData.evidenceFound;
        money = playerData.money;
        lostMoney = playerData.lostMoney;
        strength = playerData.strength;
        dexterity = playerData.dexterity;
        vitality = playerData.vitality;
        dedication = playerData.arcane;
        maxMana = playerData.maxMana;
        mana = playerData.mana;
        deathNum = playerData.deathNum;
        killedEnemiesNum = playerData.killedEnemiesNum;
        killedEnemiesAtGetShield = playerData.killedEnemiesAtGetShield;
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
        iceBossPosition = new float[3];
        iceBossPosition[0] = mapData.iceBossPosition.x;
        iceBossPosition[1] = mapData.iceBossPosition.y;
        iceBossPosition[2] = mapData.iceBossPosition.z;
        iceBossDirection = mapData.iceBossDirection;
        whistleblowerArrested = mapData.whistleblowerArrested;

        ACOn = mapData.ACOn;

        conversationsHad = dialogueData.conversationsHad;
        directorWilkinsQueue = dialogueData.directorQueue.ToArray();
        directorWilkinsPreviousConversations = dialogueData.directorPreviousConversations.ToArray();
        agentFreiQueue = dialogueData.freiQueue.ToArray();
        agentFreiPreviousConversations = dialogueData.freiPreviousConversations.ToArray();
        bonsaiQueue = dialogueData.smackGPTQueue.ToArray();
        bonsaiPreviousConversations = dialogueData.smackGPTPreviousConversations.ToArray();
        QuestionMarksQueue = dialogueData.unknownNumberQueue.ToArray();
        QuestionMarksPreviousConversations = dialogueData.unknownNumberPreviousConversations.ToArray();
        patchworkGaryConversations = dialogueData.patchworkGaryConversations.ToArray();
        whistleblowerConversations = dialogueData.whistleBlowerConversations.ToArray();
    }
}
