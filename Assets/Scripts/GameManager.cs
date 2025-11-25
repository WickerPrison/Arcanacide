using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] PlayerData playerData;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] SettingsData settingsData;
    [SerializeField] GameObject moneyDropPrefab;
    [SerializeField] string[] sceneNames;
    TutorialManager tutorialManager;
    public List<EnemyScript> enemies = new List<EnemyScript>();
    [System.NonSerialized] public List<EnemyScript> enemiesInRange = new List<EnemyScript>();
    int _awareEnemies;
    string saveFileString = "saveFile";
    public int awareEnemies
    {
        get { return _awareEnemies; }
        set
        {
            _awareEnemies = value;
            GlobalEvents.instance.AwareEnemiesChange(value);
        }
    }


    //These are the set of saved values that are used when creating a new game
    int money = 0;
    int lostMoney = 0;
    string deathRoom = "none";
    Vector3 deathPosition = Vector3.zero;
    bool ticketFiled = false;
    bool fireBossKilled = false;
    bool secretaryConvo = false;

    //This function is called very first, before the splash screen
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void SetFullscreenMode()
    {
        SettingsSaveData data = SaveSystem.LoadSettings();
        if (data == null || data.fullscreenMode)
        {
            Screen.fullScreen = true;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    private void Start()
    {;
        if (SceneManager.GetActiveScene().name == mapData.deathRoom)
        {
            GameObject moneyDrop;
            moneyDrop = Instantiate(moneyDropPrefab);
            moneyDrop.transform.position = mapData.deathPosition;
        }
        tutorialManager = gameObject.GetComponent<TutorialManager>();
    }

    private void onEnemyKilled(object sender, System.EventArgs e)
    {
        playerData.killedEnemiesNum++;
        switch (playerData.killedEnemiesNum)
        {
            case 5:
                dialogueData.smackGPTQueue.Add(1);
                break;
            case 15:
                dialogueData.smackGPTQueue.Add(2);
                break;
        }

        if(playerData.unlockedAbilities.Contains(UnlockableAbilities.BLOCK) && playerData.killedEnemiesNum - playerData.killedEnemiesAtGetShield == 11)
        {
            dialogueData.smackGPTQueue.Add(3);
        }
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onEnemyKilled += onEnemyKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onEnemyKilled -= onEnemyKilled;
    }


    public string GetSceneName(int swordSiteNumber) => sceneNames[swordSiteNumber];

    public void SaveGame()
    {
        DateTime dateTime = DateTime.Now;
        playerData.date = dateTime.GetDateTimeFormats('d')[0];
        playerData.time = dateTime.GetDateTimeFormats('t')[0];
        SaveSystem.SaveGame(playerData.saveFile, playerData, mapData, dialogueData, emblemLibrary);
        SaveSystem.SaveSettings(settingsData);
    }

    public void LoadGame(string saveFile)
    {
        SaveData data = SaveSystem.LoadGame(saveFile);

        if(data != null)
        {
            playerData.saveFile = data.saveFile;
            playerData.hasHealthGem = data.hasHealthGem;
            playerData.maxHealCharges = data.maxHealCharges;
            playerData.healCharges = data.healCharges;
            playerData.currentGemShards = data.currentGemShards;
            playerData.gemShards = data.gemShards.ToList();
            playerData.lastSwordSite = data.lastSwordSite;
            playerData.SetUnlocksWithStrings(data.unlockedAbilities);
            playerData.patches = emblemLibrary.GetPatchesFromStrings(data.patches);
            playerData.equippedPatches = emblemLibrary.GetPatchesFromStrings(data.equippedPatches);
            playerData.maxPatches = data.maxPatches;
            playerData.tutorials = data.tutorials;
            playerData.evidenceFound = data.evidenceFound;
            playerData.hasWayfaerie = data.hasWayfaerie;
            playerData.money = data.money;
            playerData.lostMoney = data.lostMoney;
            playerData.strength = data.strength;
            playerData.dexterity = data.dexterity;
            playerData.vitality = data.vitality;
            playerData.arcane = data.dedication;
            playerData.health = playerData.MaxHealth();
            playerData.maxMana = data.maxMana;
            playerData.mana = data.mana;
            playerData.deathNum = data.deathNum;
            playerData.killedEnemiesNum = data.killedEnemiesNum;
            playerData.killedEnemiesAtGetShield = data.killedEnemiesAtGetShield;
            playerData.unlockedWeapons = data.unlockedWeapons;
            playerData.currentWeapon = data.currentWeapon;
            playerData.newWeapon = data.newWeapon;
            playerData.swordSpecialTimer = 0;
            playerData.clawSpecialOn = false;
            playerData.clawSpecialTimer = 0;

            mapData.unlockedDoors = data.unlockedDoors.ToList();
            mapData.visitedRooms = data.visitedRooms.ToList();
            mapData.fireSuppressionState = (FireSuppressionState)Enum.Parse(typeof(FireSuppressionState), data.fireSuppressionState);
            mapData.powerSwitchesFlipped = data.powerSwitchesFlipped.ToList();
            mapData.deathRoom = data.deathRoom;
            mapData.deathPosition = new Vector3(data.deathPosition[0], data.deathPosition[1], data.deathPosition[2]);
            mapData.ticketFiled = data.ticketFiled;
            mapData.miniboss1Killed = data.miniboss1Killed;
            mapData.miniboss2Killed = data.miniboss2Killed;
            mapData.miniboss3Killed = data.miniboss3Killed;
            mapData.miniboss4Killed = data.miniboss4Killed;
            mapData.fireBossKilled = data.fireBossKilled;
            mapData.secretaryConvo = data.secretaryConvo;
            mapData.electricBossKilled = data.electricBossKilled;
            mapData.carolsDeadFriends = data.carolsDeadFriends.ToList();
            mapData.ACOn = data.ACOn;
            mapData.hasRemoteAC = data.hasRemoteAC;
            mapData.outsideFrankBossfight = data.outsideFrankBossfight;
            mapData.itWorkerQuestStarted = data.itWorkerQuestStarted;
            mapData.resetPasswords = data.resetPasswords.ToList();
            mapData.itWorkerQuestComplete = data.itWorkerQuestComplete;
            mapData.iceBossKilled = data.iceBossKilled;
            mapData.iceBossPosition = new Vector3(data.iceBossPosition[0], data.iceBossPosition[1], data.iceBossPosition[2]);
            mapData.iceBossDirection = data.iceBossDirection;
            mapData.whistleblowerArrested = data.whistleblowerArrested;

            dialogueData.conversationsHad = data.conversationsHad;
            dialogueData.directorQueue = data.directorWilkinsQueue.ToList();
            dialogueData.directorPreviousConversations = data.directorWilkinsPreviousConversations.ToList();
            dialogueData.freiQueue = data.agentFreiQueue.ToList();
            dialogueData.freiPreviousConversations = data.agentFreiPreviousConversations.ToList();
            dialogueData.smackGPTQueue = data.bonsaiQueue.ToList();
            dialogueData.smackGPTPreviousConversations = data.bonsaiPreviousConversations.ToList();
            dialogueData.unknownNumberQueue = data.QuestionMarksQueue.ToList();
            dialogueData.unknownNumberPreviousConversations = data.QuestionMarksPreviousConversations.ToList();
            dialogueData.patchworkGaryConversations = data.patchworkGaryConversations.ToList();
            dialogueData.whistleBlowerConversations = data.whistleblowerConversations.ToList();
        }
    }

    public void LoadSettings()
    {
        SettingsSaveData settingsSaveData = SaveSystem.LoadSettings();
        if(settingsSaveData != null)
        {
            settingsData.CreateBindingDictionary(settingsSaveData.bindingDictionaryKeys, settingsSaveData.bindingDictionaryValues);
            settingsData.showArrow = settingsSaveData.showArrow;
            settingsData.SetVolume(VolumeChannel.MASTER, settingsSaveData.masterVol);
            settingsData.SetVolume(VolumeChannel.SFX, settingsSaveData.sfxVol);
            settingsData.SetVolume(VolumeChannel.MUSIC, settingsSaveData.musicVol);
            settingsData.fullscreenMode = settingsSaveData.fullscreenMode;
        }
        else
        {
            settingsData.bindings.Clear();
            settingsData.showArrow = true;
            settingsData.SetVolume(VolumeChannel.MASTER, 1);
            settingsData.SetVolume(VolumeChannel.SFX, 1);
            settingsData.SetVolume(VolumeChannel.MUSIC, 1);
            SaveSystem.SaveSettings(settingsData);
        }
    }

    public void NewGame(int saveFileId)
    {
        playerData.saveFile = saveFileString + saveFileId.ToString();
        playerData.tutorials = tutorialManager.allTutorials;
        playerData.ClearData();

        mapData.doorNumber = 0;
        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.fireSuppressionState = FireSuppressionState.ON;
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.miniboss1Killed = false;
        mapData.miniboss2Killed = false;
        mapData.miniboss3Killed = false;
        mapData.miniboss4Killed = false;
        mapData.fireBossKilled = fireBossKilled;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = false;
        mapData.carolsDeadFriends.Clear();
        mapData.ACOn = true;
        mapData.hasRemoteAC = false;
        mapData.outsideFrankBossfight = false;
        mapData.itWorkerQuestStarted = false;
        mapData.resetPasswords.Clear();
        mapData.itWorkerQuestComplete = false;
        mapData.iceBossKilled = false;
        mapData.iceBossPosition = Vector3.zero;
        mapData.iceBossDirection = 0;
        mapData.whistleblowerArrested = false;

        dialogueData.conversationsHad.Clear();
        dialogueData.directorQueue.Clear();
        dialogueData.directorPreviousConversations.Clear();
        dialogueData.freiQueue.Clear();
        dialogueData.freiPreviousConversations.Clear();
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        dialogueData.unknownNumberQueue.Clear();
        dialogueData.unknownNumberPreviousConversations.Clear();
        dialogueData.directorQueue.Add(0);
        dialogueData.patchworkGaryConversations.Clear();
        dialogueData.whistleBlowerConversations.Clear();
    }

    public void StartAtFloor2()
    {
        playerData.saveFile = saveFileString + "4";
        playerData.hasHealthGem = true;
        playerData.maxHealCharges = 2;
        playerData.healCharges = 2;
        playerData.currentGemShards = 0;
        playerData.gemShards.Clear();
        playerData.lastSwordSite = 4;
        playerData.unlockedAbilities.Clear();
        playerData.unlockedAbilities.Add(UnlockableAbilities.BLOCK);
        playerData.patches.Clear();
        foreach (Patches patch in emblemLibrary.firstFloorPatches)
        {
            playerData.patches.Add(patch);
        }
        playerData.equippedPatches.Clear();
        playerData.maxPatches = 2;
        playerData.tutorials.Clear();
        playerData.evidenceFound.Clear();
        playerData.hasWayfaerie = false;
        playerData.money = 500;
        playerData.lostMoney = 0;
        playerData.strength = 1;
        playerData.dexterity = 1;
        playerData.vitality = 1;
        playerData.arcane = 1;
        playerData.health = playerData.MaxHealth();
        playerData.maxMana = 75;
        playerData.mana = playerData.maxMana;
        playerData.deathNum = 0;
        playerData.killedEnemiesNum = 0;
        playerData.killedEnemiesAtGetShield = 0;
        playerData.unlockedWeapons.Clear();
        playerData.unlockedWeapons.Add(0);
        playerData.unlockedWeapons.Add(1);
        playerData.currentWeapon = 0;
        playerData.newWeapon = true;
        playerData.swordSpecialTimer = 0;
        playerData.clawSpecialOn = false;
        playerData.clawSpecialTimer = 0;

        mapData.doorNumber = 1;
        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.fireSuppressionState = FireSuppressionState.FIXED;
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.miniboss1Killed = true;
        mapData.miniboss2Killed = false;
        mapData.miniboss3Killed = false;
        mapData.miniboss4Killed = false;
        mapData.fireBossKilled = true;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = false;
        mapData.carolsDeadFriends.Clear();
        mapData.iceBossKilled = false;
        mapData.iceBossPosition = Vector3.zero;
        mapData.iceBossDirection = 0;
        mapData.ACOn = true;
        mapData.hasRemoteAC = false;
        mapData.outsideFrankBossfight = false;
        mapData.itWorkerQuestStarted = false;
        mapData.resetPasswords = null;
        mapData.itWorkerQuestComplete = false;
        mapData.whistleblowerArrested = false;

        dialogueData.conversationsHad.Clear();
        dialogueData.directorQueue.Clear();
        dialogueData.directorPreviousConversations.Clear();
        dialogueData.freiQueue.Clear();
        dialogueData.freiPreviousConversations.Clear();
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        dialogueData.unknownNumberQueue.Clear();
        dialogueData.unknownNumberPreviousConversations.Clear();
        dialogueData.directorQueue.Add(0);
        dialogueData.patchworkGaryConversations.Clear();
        dialogueData.whistleBlowerConversations.Clear();
    }

    public void StartAtFloor3()
    {
        playerData.saveFile = saveFileString + "4";
        playerData.hasHealthGem = true;
        playerData.maxHealCharges = 3;
        playerData.healCharges = 3;
        playerData.currentGemShards = 0;
        playerData.gemShards.Clear();
        playerData.lastSwordSite = 5;
        playerData.unlockedAbilities.Clear();
        playerData.unlockedAbilities.Add(UnlockableAbilities.BLOCK);
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);
        playerData.patches.Clear();
        foreach (Patches patch in emblemLibrary.firstFloorPatches)
        {
            playerData.patches.Add(patch);
        }
        foreach(Patches patch in emblemLibrary.secondFloorPatches)
        {
            playerData.patches.Add(patch);
        }
        playerData.equippedPatches.Clear();
        playerData.maxPatches = 2;
        playerData.tutorials.Clear();
        playerData.evidenceFound.Clear();
        playerData.hasWayfaerie = false;
        playerData.money = 5000;
        playerData.lostMoney = 0;
        playerData.strength = 1;
        playerData.dexterity = 1;
        playerData.vitality = 1;
        playerData.arcane = 1;
        playerData.health = playerData.MaxHealth();
        playerData.maxMana = 50;
        playerData.mana = playerData.maxMana;
        playerData.deathNum = 0;
        playerData.killedEnemiesNum = 0;
        playerData.killedEnemiesAtGetShield = 0;
        playerData.unlockedWeapons.Clear();
        playerData.unlockedWeapons.Add(0);
        playerData.unlockedWeapons.Add(1);
        playerData.unlockedWeapons.Add(2);
        playerData.currentWeapon = 0;
        playerData.newWeapon = true;
        playerData.swordSpecialTimer = 0;
        playerData.clawSpecialOn = false;
        playerData.clawSpecialTimer = 0;

        mapData.doorNumber = 0;
        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.fireSuppressionState = FireSuppressionState.FIXED;
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.miniboss1Killed = true;
        mapData.miniboss2Killed = true;
        mapData.miniboss3Killed = false;
        mapData.miniboss4Killed = false;
        mapData.fireBossKilled = true;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = true;
        mapData.carolsDeadFriends.Clear();
        mapData.iceBossKilled = false;
        mapData.iceBossPosition = Vector3.zero;
        mapData.iceBossDirection = 0;
        mapData.ACOn = true;
        mapData.hasRemoteAC = false;
        mapData.outsideFrankBossfight = false;
        mapData.itWorkerQuestStarted = false;
        mapData.resetPasswords = null;
        mapData.itWorkerQuestComplete = false;
        mapData.whistleblowerArrested = false;

        dialogueData.conversationsHad.Clear();
        dialogueData.directorQueue.Clear();
        dialogueData.directorPreviousConversations.Clear();
        dialogueData.freiQueue.Clear();
        dialogueData.freiPreviousConversations.Clear();
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        dialogueData.unknownNumberQueue.Clear();
        dialogueData.unknownNumberPreviousConversations.Clear();
        dialogueData.directorQueue.Add(0);
        dialogueData.patchworkGaryConversations.Clear();
        dialogueData.whistleBlowerConversations.Clear();
    }

    public void StartAtFloor4()
    {
        playerData.saveFile = saveFileString + "4";
        playerData.hasHealthGem = true;
        playerData.maxHealCharges = 4;
        playerData.healCharges = 4;
        playerData.currentGemShards = 0;
        playerData.gemShards.Clear();
        playerData.lastSwordSite = 8;
        playerData.unlockedAbilities.Clear();
        playerData.unlockedAbilities.Add(UnlockableAbilities.BLOCK);
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);
        playerData.unlockedAbilities.Add(UnlockableAbilities.MORE_PATCHES_1);
        playerData.patches.Clear();
        foreach (Patches patch in emblemLibrary.firstFloorPatches)
        {
            playerData.patches.Add(patch);
        }
        foreach (Patches patch in emblemLibrary.secondFloorPatches)
        {
            playerData.patches.Add(patch);
        }
        foreach(Patches patch in emblemLibrary.thirdFloorPatches)
        {
            playerData.patches.Add(patch);
        }
        playerData.equippedPatches.Clear();
        playerData.maxPatches = 3;
        playerData.tutorials.Clear();
        playerData.evidenceFound.Clear();
        playerData.hasWayfaerie = false;
        playerData.money = 15000;
        playerData.lostMoney = 0;
        playerData.strength = 1;
        playerData.dexterity = 1;
        playerData.vitality = 1;
        playerData.arcane = 1;
        playerData.health = playerData.MaxHealth();
        playerData.maxMana = 50;
        playerData.mana = playerData.maxMana;
        playerData.deathNum = 0;
        playerData.killedEnemiesNum = 0;
        playerData.killedEnemiesAtGetShield = 0;
        playerData.unlockedWeapons.Clear();
        playerData.unlockedWeapons.Add(0);
        playerData.unlockedWeapons.Add(1);
        playerData.unlockedWeapons.Add(2);
        playerData.unlockedWeapons.Add(3);
        playerData.currentWeapon = 0;
        playerData.newWeapon = true;
        playerData.swordSpecialTimer = 0;
        playerData.clawSpecialOn = false;
        playerData.clawSpecialTimer = 0;

        mapData.doorNumber = 0;
        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.fireSuppressionState = FireSuppressionState.FIXED;
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.miniboss1Killed = true;
        mapData.miniboss2Killed = true;
        mapData.miniboss3Killed = true;
        mapData.miniboss4Killed = false;
        mapData.fireBossKilled = true;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = true;
        mapData.carolsDeadFriends.Clear();
        mapData.iceBossKilled = true;
        mapData.iceBossPosition = Vector3.zero;
        mapData.iceBossDirection = 0;
        mapData.ACOn = false;
        mapData.hasRemoteAC = true;
        mapData.outsideFrankBossfight = true;
        mapData.itWorkerQuestStarted = false;
        mapData.resetPasswords = null;
        mapData.itWorkerQuestComplete = false;
        mapData.whistleblowerArrested = false;

        dialogueData.conversationsHad.Clear();
        dialogueData.directorQueue.Clear();
        dialogueData.directorPreviousConversations.Clear();
        dialogueData.freiQueue.Clear();
        dialogueData.freiPreviousConversations.Clear();
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        dialogueData.unknownNumberQueue.Clear();
        dialogueData.unknownNumberPreviousConversations.Clear();
        dialogueData.directorQueue.Add(0);
        dialogueData.patchworkGaryConversations.Clear();
        dialogueData.whistleBlowerConversations.Clear();
    }
}
