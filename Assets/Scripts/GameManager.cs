using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
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
        SaveSystem.SaveGame(playerData, mapData);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        if(data != null)
        {
            playerData.lastAltar = data.lastAltar;
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

            mapData.unlockedDoors = data.unlockedDoors.ToList();
            mapData.deathRoom = data.deathRoom;
            mapData.deathPosition = new Vector3(data.deathPosition[0], data.deathPosition[1], data.deathPosition[2]);
            mapData.ticketFiled = data.ticketFiled;
            mapData.fireBossKilled = data.fireBossKilled;
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        playerData.health = playerData.MaxHealth();
        playerData.hasHealed = false;
        playerData.lastAltar = lastAltar;
        playerData.unlockedAbilities.Clear();
        playerData.unlockedAbilities.Add("Heal");
        playerData.equippedAbility = "Heal";
        playerData.emblems.Clear();
        playerData.equippedEmblems.Clear();
        playerData.tutorials = tutorialManager.allTutorials;
        playerData.money = money;
        playerData.lostMoney = lostMoney;
        playerData.strength = 1;
        playerData.dexterity = 1;
        playerData.vitality = 1;
        playerData.dedication = 1;

        mapData.unlockedDoors.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.ticketFiled = ticketFiled;
        mapData.fireBossKilled = fireBossKilled;
    }
}
