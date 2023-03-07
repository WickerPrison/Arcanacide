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
    [SerializeField] GameObject moneyDropPrefab;
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

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("MusicPlayer") == null)
        {
            Instantiate(musicPlayer);
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == mapData.deathRoom)
        {
            GameObject moneyDrop;
            moneyDrop = Instantiate(moneyDropPrefab);
            moneyDrop.transform.position = mapData.deathPosition;
        }
        tutorialManager = gameObject.GetComponent<TutorialManager>();
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(playerData, mapData, dialogueData);
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
            playerData.tutorials = data.tutorials;
            playerData.money = data.money;
            playerData.lostMoney = data.lostMoney;
            playerData.strength = data.strength;
            playerData.dexterity = data.dexterity;
            playerData.vitality = data.vitality;
            playerData.dedication = data.dedication;
            playerData.maxMana = data.maxMana;
            playerData.mana = data.mana;
            playerData.deathNum = data.deathNum;
            playerData.killedEnemiesNum = data.killedEnemiesNum;
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

            dialogueData.ORTHODOXQueue = data.ORTHODOXQueue.ToList();
            dialogueData.ORTHODOXPreviousConversations = data.ORTHODOXPreviousConversations.ToList();
            dialogueData.TRENCHQueue = data.TRENCHQueue.ToList();
            dialogueData.TRENCHPreviousConversations = data.TRENCHPreviousConversations.ToList();
            dialogueData.UnknownNumberQueue = data.QuestionMarksQueue.ToList();
            dialogueData.UnknownNumberPreviousConversations = data.QuestionMarksPreviousConversations.ToList();
            dialogueData.conversationsHad = data.conversationsHad.ToList();
            dialogueData.patchworkGaryConversations = data.patchworkGaryConversations.ToList();
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
        playerData.tutorials = tutorialManager.allTutorials;
        playerData.money = money;
        playerData.lostMoney = lostMoney;
        playerData.strength = 1;
        playerData.dexterity = 1;
        playerData.vitality = 1;
        playerData.dedication = 1;
        playerData.health = playerData.MaxHealth();
        playerData.maxMana = 50;
        playerData.mana = playerData.maxMana;
        playerData.deathNum = 0;
        playerData.killedEnemiesNum = 0;
        playerData.unlockedWeapons.Clear();
        playerData.unlockedWeapons.Add(0);
        playerData.currentWeapon = 0;

        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.fireBossKilled = fireBossKilled;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = false;

        dialogueData.ORTHODOXQueue.Clear();
        dialogueData.ORTHODOXPreviousConversations.Clear();
        dialogueData.TRENCHQueue.Clear();
        dialogueData.TRENCHPreviousConversations.Clear();
        dialogueData.UnknownNumberQueue.Clear();
        dialogueData.UnknownNumberPreviousConversations.Clear();
        dialogueData.ORTHODOXQueue.Add(0);
        dialogueData.conversationsHad.Clear();
        dialogueData.patchworkGaryConversations.Clear();
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
        playerData.tutorials.Clear();
        playerData.money = 280;
        playerData.lostMoney = 0;
        playerData.strength = 1;
        playerData.dexterity = 1;
        playerData.vitality = 1;
        playerData.dedication = 1;
        playerData.health = playerData.MaxHealth();
        playerData.maxMana = 50;
        playerData.mana = playerData.maxMana;
        playerData.deathNum = 0;
        playerData.killedEnemiesNum = 0;
        playerData.unlockedWeapons.Clear();
        playerData.unlockedWeapons.Add(0);
        playerData.unlockedWeapons.Add(1);
        playerData.currentWeapon = 0;

        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.fireBossKilled = true;
        mapData.secretaryConvo = secretaryConvo;
        mapData.electricBossKilled = false;

        dialogueData.ORTHODOXQueue.Clear();
        dialogueData.ORTHODOXPreviousConversations.Clear();
        dialogueData.TRENCHQueue.Clear();
        dialogueData.TRENCHPreviousConversations.Clear();
        dialogueData.UnknownNumberQueue.Clear();
        dialogueData.UnknownNumberPreviousConversations.Clear();
        dialogueData.ORTHODOXQueue.Add(0);
        dialogueData.conversationsHad.Clear();
        dialogueData.patchworkGaryConversations.Clear();
    }
}
