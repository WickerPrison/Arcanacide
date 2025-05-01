using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CarolTests
{
    PlayerData playerData;
    MapData mapData;
    GameObject carolPrefab;
    ElectricBossController bossController;
    EnemyScript enemyScript;
    Dialogue dialogue;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.vitality = 30;
        playerData.health = playerData.MaxHealth();
        mapData = Resources.Load<MapData>("Data/MapData");
        mapData.electricBossKilled = false;
        mapData.carolsDeadFriends.Clear();
        carolPrefab = Resources.Load<GameObject>("Prefabs/Enemies/ElectricBoss");
        Time.timeScale = 1f;
    }

    void SpawnCarol()
    {
        enemyScript = GameObject.Instantiate(carolPrefab).GetComponent<EnemyScript>();
        bossController = enemyScript.GetComponent<ElectricBossController>();
        dialogue = enemyScript.GetComponentInChildren<Dialogue>();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
    }


    [UnityTest]
    public IEnumerator CarolsHealth()
    {
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Jeff");
        SpawnCarol();
        yield return null;
        bossController.EndDialogue();
        float baseMaxHealth = enemyScript.maxHealth;

        Setup();
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Jeff");
        SpawnCarol();
        yield return null;
        Assert.LessOrEqual(Mathf.Abs(enemyScript.maxHealth - baseMaxHealth * 1.3333f), 2);

        Setup();
        SpawnCarol();
        yield return null;
        Assert.LessOrEqual(Mathf.Abs(enemyScript.maxHealth - baseMaxHealth * 2), 2);
    }

    [UnityTest]
    public IEnumerator SwordAttack()
    {
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Jeff");
        SpawnCarol();
        enemyScript.transform.position = new Vector3(2f, 0, 2f);
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        bossController.Attack();
        yield return new WaitForSeconds(0.8f);
        float expectedDamage = bossController.hitDamage * 1.33333f;
        float expectedHealth = playerData.MaxHealth() - expectedDamage;
        Assert.LessOrEqual(Mathf.Abs(playerData.health - expectedHealth), 2);
    }
}
