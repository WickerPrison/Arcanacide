using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject musicPlayer;
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
    public int awareEnemies;


    //These are the set of saved values that are used when creating a new game
    int lastAltar = 1;
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
        SaveData data = SaveSystem.LoadGame();
        if (data == null || data.fullscreenMode)
        {
            Screen.fullScreen = true;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else Screen.fullScreen = false;
    }

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("MusicPlayer") == null)
        {
            Instantiate(musicPlayer);
        }
    }

    private void Start()
    {
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
                dialogueData.bonsaiQueue.Add(1);
                break;
            case 15:
                dialogueData.bonsaiQueue.Add(2);
                break;
        }

        if(playerData.unlockedAbilities.Contains("Block") && playerData.killedEnemiesNum - playerData.killedEnemiesAtGetShield == 11)
        {
            dialogueData.bonsaiQueue.Add(3);
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
        SaveSystem.SaveGame(playerData, mapData, dialogueData, settingsData);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        if(data != null)
        {
            playerData.hasHealthGem = data.hasHealthGem;
            playerData.maxHealCharges = data.maxHealCharges;
            playerData.healCharges = data.healCharges;
            playerData.lastSwordSite = data.lastSwordSite;
            playerData.unlockedAbilities = data.unlockedAbilities;
            playerData.emblems = data.emblems;
            playerData.equippedEmblems = data.equippedEmblems;
            playerData.maxPatches = data.maxPatches;
            playerData.tutorials = data.tutorials;
            playerData.evidenceFound = data.evidenceFound;
            playerData.money = data.money;
            playerData.lostMoney = data.lostMoney;
            playerData.strength = data.strength;
            playerData.dexterity = data.dexterity;
            playerData.vitality = data.vitality;
            playerData.arcane = data.dedication;
            playerData.maxMana = data.maxMana;
            playerData.mana = data.mana;
            playerData.deathNum = data.deathNum;
            playerData.killedEnemiesNum = data.killedEnemiesNum;
            playerData.killedEnemiesAtGetShield = data.killedEnemiesAtGetShield;
            playerData.unlockedWeapons = data.unlockedWeapons;
            playerData.currentWeapon = data.currentWeapon;

            mapData.unlockedDoors = data.unlockedDoors.ToList();
            mapData.visitedRooms = data.visitedRooms.ToList();
            mapData.powerSwitchesFlipped = data.powerSwitchesFlipped.ToList();
            mapData.deathRoom = data.deathRoom;
            mapData.deathPosition = new Vector3(data.deathPosition[0], data.deathPosition[1], data.deathPosition[2]);
            mapData.ticketFiled = data.ticketFiled;
            mapData.fireBossKilled = data.fireBossKilled;
            mapData.secretaryConvo = data.secretaryConvo;
            mapData.electricBossKilled = data.electricBossKilled;
            mapData.iceBossKilled = data.iceBossKilled;
            mapData.iceBossPosition = new Vector3(data.iceBossPosition[0], data.iceBossPosition[1], data.iceBossPosition[2]);
            mapData.iceBossDirection = data.iceBossDirection;

            dialogueData.conversationsHad = data.conversationsHad;
            dialogueData.directorQueue = data.directorWilkinsQueue.ToList();
            dialogueData.directorPreviousConversations = data.directorWilkinsPreviousConversations.ToList();
            dialogueData.freiQueue = data.agentFreiQueue.ToList();
            dialogueData.freiPreviousConversations = data.agentFreiPreviousConversations.ToList();
            dialogueData.bonsaiQueue = data.bonsaiQueue.ToList();
            dialogueData.bonsaiPreviousConversations = data.bonsaiPreviousConversations.ToList();
            dialogueData.unknownNumberQueue = data.QuestionMarksQueue.ToList();
            dialogueData.unknownNumberPreviousConversations = data.QuestionMarksPreviousConversations.ToList();
            dialogueData.patchworkGaryConversations = data.patchworkGaryConversations.ToList();
            dialogueData.whistleBlowerConversations = data.whistleblowerConversations.ToList();

            settingsData.CreateBindingDictionary(data.bindingDictionaryKeys, data.bindingDictionaryValues);
            settingsData.showArrow = data.showArrow;
            settingsData.SetVolume(VolumeChannel.MASTER, data.masterVol);
            settingsData.SetVolume(VolumeChannel.SFX, data.sfxVol);
            settingsData.SetVolume(VolumeChannel.MUSIC, data.musicVol);
            settingsData.fullscreenMode = data.fullscreenMode;
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        playerData.hasHealthGem = false;
        playerData.maxHealCharges = 1;
        playerData.healCharges = 1;
        playerData.lastSwordSite = lastAltar;
        playerData.unlockedAbilities.Clear();
        playerData.emblems.Clear();
        playerData.equippedEmblems.Clear();
        playerData.maxPatches = 2;
        playerData.tutorials = tutorialManager.allTutorials;
        playerData.evidenceFound.Clear();
        playerData.money = money;
        playerData.lostMoney = lostMoney;
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
        playerData.currentWeapon = 0;

        mapData.doorNumber = 0;
        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.fireBossKilled = fireBossKilled;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = false;
        mapData.iceBossKilled = false;
        mapData.iceBossPosition = Vector3.zero;
        mapData.iceBossDirection = 0;

        mapData.ACOn = true;

        dialogueData.conversationsHad.Clear();
        dialogueData.directorQueue.Clear();
        dialogueData.directorPreviousConversations.Clear();
        dialogueData.freiQueue.Clear();
        dialogueData.freiPreviousConversations.Clear();
        dialogueData.bonsaiQueue.Clear();
        dialogueData.bonsaiPreviousConversations.Clear();
        dialogueData.unknownNumberQueue.Clear();
        dialogueData.unknownNumberPreviousConversations.Clear();
        dialogueData.directorQueue.Add(0);
        dialogueData.patchworkGaryConversations.Clear();
        dialogueData.whistleBlowerConversations.Clear();

        settingsData.bindings.Clear();
        settingsData.showArrow = true;
        settingsData.SetVolume(VolumeChannel.MASTER, 1);
        settingsData.SetVolume(VolumeChannel.SFX, 1);
        settingsData.SetVolume(VolumeChannel.MUSIC, 1);
    }

    public void StartAtFloor2()
    {
        playerData.hasHealthGem = true;
        playerData.maxHealCharges = 2;
        playerData.healCharges = 2;
        playerData.lastSwordSite = 4;
        playerData.unlockedAbilities.Clear();
        playerData.unlockedAbilities.Add("Block");
        playerData.emblems.Clear();
        foreach (string patch in emblemLibrary.firstFloorPatches)
        {
            playerData.emblems.Add(patch);
        }
        playerData.equippedEmblems.Clear();
        playerData.maxPatches = 2;
        playerData.tutorials.Clear();
        playerData.evidenceFound.Clear();
        playerData.money = 280;
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
        playerData.currentWeapon = 0;

        mapData.doorNumber = 1;
        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.fireBossKilled = true;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = false;
        mapData.iceBossKilled = false;
        mapData.iceBossPosition = Vector3.zero;
        mapData.iceBossDirection = 0;
        mapData.ACOn = true;

        dialogueData.conversationsHad.Clear();
        dialogueData.directorQueue.Clear();
        dialogueData.directorPreviousConversations.Clear();
        dialogueData.freiQueue.Clear();
        dialogueData.freiPreviousConversations.Clear();
        dialogueData.bonsaiQueue.Clear();
        dialogueData.bonsaiPreviousConversations.Clear();
        dialogueData.unknownNumberQueue.Clear();
        dialogueData.unknownNumberPreviousConversations.Clear();
        dialogueData.directorQueue.Add(0);
        dialogueData.patchworkGaryConversations.Clear();
        dialogueData.whistleBlowerConversations.Clear();

        settingsData.bindings.Clear();
        settingsData.showArrow = true;
        settingsData.SetVolume(VolumeChannel.MASTER, 1);
        settingsData.SetVolume(VolumeChannel.SFX, 1);
        settingsData.SetVolume(VolumeChannel.MUSIC, 1);
    }

    public void StartAtFloor3()
    {
        playerData.hasHealthGem = true;
        playerData.maxHealCharges = 3;
        playerData.healCharges = 3;
        playerData.lastSwordSite = 5;
        playerData.unlockedAbilities.Clear();
        playerData.unlockedAbilities.Add("Block");
        playerData.unlockedAbilities.Add("Special Attack");
        playerData.emblems.Clear();
        foreach (string patch in emblemLibrary.firstFloorPatches)
        {
            playerData.emblems.Add(patch);
        }
        foreach(string patch in emblemLibrary.secondFloorPatches)
        {
            playerData.emblems.Add(patch);
        }
        playerData.equippedEmblems.Clear();
        playerData.maxPatches = 2;
        playerData.tutorials.Clear();
        playerData.evidenceFound.Clear();
        playerData.money = 2000;
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

        mapData.doorNumber = 0;
        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.fireBossKilled = true;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = true;
        mapData.iceBossKilled = false;
        mapData.iceBossPosition = Vector3.zero;
        mapData.iceBossDirection = 0;
        mapData.ACOn = true;

        dialogueData.conversationsHad.Clear();
        dialogueData.directorQueue.Clear();
        dialogueData.directorPreviousConversations.Clear();
        dialogueData.freiQueue.Clear();
        dialogueData.freiPreviousConversations.Clear();
        dialogueData.bonsaiQueue.Clear();
        dialogueData.bonsaiPreviousConversations.Clear();
        dialogueData.unknownNumberQueue.Clear();
        dialogueData.unknownNumberPreviousConversations.Clear();
        dialogueData.directorQueue.Add(0);
        dialogueData.patchworkGaryConversations.Clear();
        dialogueData.whistleBlowerConversations.Clear();

        settingsData.bindings.Clear();
        settingsData.showArrow = true;
        settingsData.SetVolume(VolumeChannel.MASTER, 1);
        settingsData.SetVolume(VolumeChannel.SFX, 1);
        settingsData.SetVolume(VolumeChannel.MUSIC, 1);
    }
}
