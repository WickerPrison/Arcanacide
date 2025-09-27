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
    public int currentGemShards;
    public string[] gemShards;
    public int lastSwordSite;
    public List<string> unlockedAbilities;
    public List<string> patches;
    public List<string> equippedPatches;
    public int maxPatches;
    public List<string> tutorials;
    public List<string> evidenceFound;
    public bool hasWayfaerie;
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
    public bool newWeapon;

    public int[] unlockedDoors;
    public int[] visitedRooms;
    public string fireSuppressionState;
    public int[] powerSwitchesFlipped;
    public string deathRoom;
    public float[] deathPosition;
    public bool ticketFiled;
    public bool miniboss1Killed;
    public bool miniboss2Killed;
    public bool miniboss3Killed;
    public bool fireBossKilled;
    public bool secretaryConvo;
    public bool electricBossKilled;
    public string[] carolsDeadFriends;
    public bool iceBossKilled;
    public float[] iceBossPosition;
    public int iceBossDirection;
    public bool ACOn;
    public bool hasRemoteAC;
    public bool outsideFrankBossfight;
    public int[] resetPasswords;
    public bool itWorkerQuestComplete;
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

    public SaveData (PlayerData playerData, MapData mapData, DialogueData dialogueData, EmblemLibrary emblemLibrary)
    {
        saveFile = playerData.saveFile;
        date = playerData.date;
        time = playerData.time;
        hasHealthGem = playerData.hasHealthGem;
        maxHealCharges = playerData.maxHealCharges;
        healCharges = playerData.healCharges;
        currentGemShards = playerData.currentGemShards;
        gemShards = playerData.gemShards.ToArray();
        lastSwordSite = playerData.lastSwordSite;
        unlockedAbilities = playerData.GetUnlockedStrings();
        patches = emblemLibrary.GetStringsFromPatches(playerData.patches);
        equippedPatches = emblemLibrary.GetStringsFromPatches(playerData.equippedPatches);
        maxPatches = playerData.maxPatches;
        tutorials = playerData.tutorials;
        evidenceFound = playerData.evidenceFound;
        hasWayfaerie = playerData.hasWayfaerie;
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
        newWeapon = playerData.newWeapon;


        unlockedDoors = mapData.unlockedDoors.ToArray();
        visitedRooms = mapData.visitedRooms.ToArray();
        fireSuppressionState = mapData.fireSuppressionState.ToString();
        powerSwitchesFlipped = mapData.powerSwitchesFlipped.ToArray();
        deathRoom = mapData.deathRoom;
        deathPosition = new float[3];
        deathPosition[0] = mapData.deathPosition.x;
        deathPosition[1] = mapData.deathPosition.y;
        deathPosition[2] = mapData.deathPosition.z;
        ticketFiled = mapData.ticketFiled;
        miniboss1Killed = mapData.miniboss1Killed;
        miniboss2Killed = mapData.miniboss2Killed;
        miniboss3Killed = mapData.miniboss3Killed;
        fireBossKilled = mapData.fireBossKilled;
        secretaryConvo = mapData.secretaryConvo;
        electricBossKilled = mapData.electricBossKilled;
        carolsDeadFriends = mapData.carolsDeadFriends.ToArray();
        iceBossKilled = mapData.iceBossKilled;
        iceBossPosition = new float[3];
        iceBossPosition[0] = mapData.iceBossPosition.x;
        iceBossPosition[1] = mapData.iceBossPosition.y;
        iceBossPosition[2] = mapData.iceBossPosition.z;
        iceBossDirection = mapData.iceBossDirection;
        if(mapData.resetPasswords != null)
        {
            resetPasswords = mapData.resetPasswords.ToArray();
        }
        else
        {
            resetPasswords = null;
        }
        itWorkerQuestComplete = mapData.itWorkerQuestComplete;
        whistleblowerArrested = mapData.whistleblowerArrested;

        ACOn = mapData.ACOn;
        hasRemoteAC = mapData.hasRemoteAC;
        outsideFrankBossfight = mapData.outsideFrankBossfight;

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
