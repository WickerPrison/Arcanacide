using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject musicPlayer;
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] PhoneData phoneData;
    [SerializeField] GameObject moneyDropPrefab;
    TutorialManager tutorialManager;
    public List<EnemyScript> enemies = new List<EnemyScript>();
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
        SaveSystem.SaveGame(playerData, mapData, phoneData);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        if(data != null)
        {
            playerData.maxHealCharges = data.maxHealCharges;
            playerData.healCharges = data.healCharges;
            playerData.lastSwordSite = data.lastSwordSite;
            playerData.unlockedAbilities = data.unlockedAbilities;
            playerData.emblems = data.emblems;
            playerData.equippedEmblems = data.equippedEmblems;
            playerData.tutorials = data.tutorials;
            playerData.money = data.money;
            playerData.lostMoney = data.lostMoney;
            playerData.path = data.path;
            playerData.strength = data.strength;
            playerData.dexterity = data.dexterity;
            playerData.vitality = data.vitality;
            playerData.dedication = data.dedication;
            playerData.maxMana = data.maxMana;
            playerData.mana = data.mana;
            playerData.deathNum = data.deathNum;
            playerData.killedEnemiesNum = data.killedEnemiesNum;

            mapData.unlockedDoors = data.unlockedDoors.ToList();
            mapData.visitedRooms = data.visitedRooms.ToList();
            mapData.powerSwitchesFlipped = data.powerSwitchesFlipped.ToList();
            mapData.deathRoom = data.deathRoom;
            mapData.deathPosition = new Vector3(data.deathPosition[0], data.deathPosition[1], data.deathPosition[2]);
            mapData.ticketFiled = data.ticketFiled;
            mapData.fireBossKilled = data.fireBossKilled;
            mapData.secretaryConvo = data.secretaryConvo;

            phoneData.ORTHODOXQueue = data.ORTHODOXQueue;
            phoneData.ORTHODOXPreviousConversations = data.ORTHODOXPreviousConversations;
            phoneData.TRENCHQueue = data.TRENCHQueue;
            phoneData.TRENCHPreviousConversations = data.TRENCHPreviousConversations;
            phoneData.UnknownNumberQueue = data.QuestionMarksQueue;
            phoneData.UnknownNumberPreviousConversations = data.QuestionMarksPreviousConversations;
            phoneData.HeadOfITQueue = data.HeadOfITQueue;
            phoneData.HeadOfITPreviousConversations = data.HeadOfITPreviousConversations;
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        playerData.health = playerData.MaxHealth();
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
        playerData.maxMana = 50;
        playerData.mana = playerData.maxMana;
        playerData.deathNum = 0;
        playerData.killedEnemiesNum = 0;

        mapData.unlockedDoors.Clear();
        mapData.visitedRooms.Clear();
        mapData.powerSwitchesFlipped.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.fireBossKilled = fireBossKilled;
        mapData.secretaryConvo = secretaryConvo;

        phoneData.ORTHODOXQueue.Clear();
        phoneData.ORTHODOXPreviousConversations.Clear();
        phoneData.TRENCHQueue.Clear();
        phoneData.TRENCHPreviousConversations.Clear();
        phoneData.UnknownNumberQueue.Clear();
        phoneData.UnknownNumberPreviousConversations.Clear();
        phoneData.HeadOfITQueue.Clear();
        phoneData.HeadOfITPreviousConversations.Clear();

        phoneData.ORTHODOXQueue.Add(0);
    }
}
