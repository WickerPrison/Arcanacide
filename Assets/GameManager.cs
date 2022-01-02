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
    public int numberOfEnemies;
    public int awareEnemies;


    //These are the set of saved values that are used when creating a new game
    int maxHealth = 100;
    float maxStamina = 100;
    int attackPower = 10;
    int lastAltar = 1;
    bool hasBlock = false;
    int money = 0;
    int lostMoney = 0;
    string deathRoom = "none";
    Vector3 deathPosition = Vector3.zero;
    bool fireBossKilled = false;
    bool boughtDamage = false;
    bool boughtHealth = false;
    bool boughtStamina = false;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == mapData.deathRoom)
        {
            GameObject moneyDrop;
            moneyDrop = Instantiate(moneyDropPrefab);
            moneyDrop.transform.position = mapData.deathPosition;
        }
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
            playerData.maxHealth = data.maxHealth;
            playerData.maxStamina = data.maxStamina;
            playerData.attackPower = data.attackPower;
            playerData.lastAltar = data.lastAltar;
            playerData.hasBlock = data.hasBlock;
            playerData.money = data.money;
            playerData.lostMoney = data.lostMoney;

            mapData.unlockedDoors = data.unlockedDoors.ToList();
            mapData.deathRoom = data.deathRoom;
            mapData.deathPosition = new Vector3(data.deathPosition[0], data.deathPosition[1], data.deathPosition[2]);
            mapData.fireBossKilled = data.fireBossKilled;
            mapData.boughtDamage = data.boughtDamage;
            mapData.boughtHealth = data.boughtHealth;
            mapData.boughtStamina = data.boughtStamina;
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        playerData.maxHealth = maxHealth;
        playerData.maxStamina = maxStamina;
        playerData.attackPower = attackPower;
        playerData.lastAltar = lastAltar;
        playerData.hasBlock = hasBlock;
        playerData.money = money;
        playerData.lostMoney = lostMoney;

        mapData.unlockedDoors.Clear();
        mapData.deathRoom = deathRoom;
        mapData.deathPosition = deathPosition;
        mapData.fireBossKilled = fireBossKilled;
        mapData.boughtDamage = boughtDamage;
        mapData.boughtHealth = boughtHealth;
        mapData.boughtStamina = boughtStamina;
    }
}
