using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System;

public class CarolTests
{
    PlayerData playerData;
    MapData mapData;
    PlayerStats playerStats;
    DialogueData dialogueData;
    GameObject carolPrefab;
    GameObject filingCabinetPrefab;
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
        playerStats = Resources.Load<PlayerStats>("Data/PlayerStats");
        playerStats.ClearData();
        dialogueData = Resources.Load<DialogueData>("Data/DialogueData");
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        carolPrefab = Resources.Load<GameObject>("Prefabs/Enemies/ElectricBoss");
        filingCabinetPrefab = Resources.Load<GameObject>("Prefabs/Layout/FilingCabinet");
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

    [UnityTest]
    public IEnumerator Beams()
    {
        mapData.carolsDeadFriends.Add("Jeff");
        SpawnCarol();
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        bossController.StartBeams();
        yield return new WaitForSeconds(2.5f);
        int beamHits = 2;
        ElectricBeams beams = bossController.GetComponentInChildren<ElectricBeams>();
        float expectedDamage = beams.beamDamage * 1.66666f * beamHits;
        float expectedHealth = playerData.MaxHealth() - expectedDamage;
        Assert.LessOrEqual(Mathf.Abs(playerData.health - expectedHealth), 2);
    }

    [UnityTest]
    public IEnumerator Hadoken()
    {
        SpawnCarol();
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        bossController.abilityTime = 1000;
        bossController.attackTime = 1000;
        bossController.StartHadoken();
        yield return new WaitForSeconds(1.2f);
        float expectedDamage = 40 * 2;
        float expectedHealth = playerData.MaxHealth() - expectedDamage;
        Assert.LessOrEqual(Mathf.Abs(playerData.health - expectedHealth), 2);
    }

    [UnityTest]
    public IEnumerator Phase2Hadoken()
    {
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Jeff");
        Transform filingCabinet = GameObject.Instantiate(filingCabinetPrefab).transform;
        filingCabinet.position = new Vector3(5, 1.2f, 0);
        SpawnCarol();
        bossController.phase2 = true;
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        bossController.abilityTime = 1000;
        bossController.attackTime = 1000;
        bossController.StartHadoken();
        yield return new WaitForSeconds(1.5f);
        float expectedDamage = 40 * 1.333333f + 25 * 1.3333333f;
        float expectedHealth = playerData.MaxHealth() - expectedDamage;
        Assert.LessOrEqual(Mathf.Abs(playerData.health - expectedHealth), 2);
    }

    [UnityTest]
    public IEnumerator Charge()
    {
        mapData.carolsDeadFriends.Add("Jeff");
        SpawnCarol();
        enemyScript.transform.position = new Vector3(4f, 0, 3f);
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        bossController.abilityTime = 1000;
        bossController.attackTime = 1000;
        bossController.Charge();
        yield return new WaitForSeconds(2.5f);
        float expectedDamage = bossController.chargeDamage * 1.666666f + bossController.chargeBurstDamage * 1.6666666f;
        float expectedHealth = playerData.MaxHealth() - expectedDamage;
        Assert.LessOrEqual(Mathf.Abs(playerData.health - expectedHealth), 2);
    }

    [UnityTest]
    public IEnumerator ChargeBounce()
    {
        mapData.carolsDeadFriends.Add("Jeff");
        SpawnCarol();
        enemyScript.transform.position = new Vector3(4f, 0, 3f);
        yield return null;
        Transform filingCabinet = GameObject.Instantiate(filingCabinetPrefab).transform;
        filingCabinet.position = new Vector3(-4f, 1.2f, -3f);
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        bossController.abilityTime = 1000;
        bossController.attackTime = 1000;
        bossController.Charge();
        yield return new WaitForSeconds(2.5f);
        float expectedDamage = bossController.chargeDamage * 1.666666f + bossController.chargeBurstDamage * 1.6666666f;
        float expectedHealth = playerData.MaxHealth() - expectedDamage;
        Assert.LessOrEqual(Mathf.Abs(playerData.health - expectedHealth), 2);
    }

    [UnityTest]
    public IEnumerator TrackPlayerKills()
    {
        SpawnCarol();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.ELECTRIC_BOSS));
        bossController.Attack();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(1, playerStats.deathsToEnemies[EnemyType.ELECTRIC_BOSS]);
    }

    [UnityTest]
    public IEnumerator AddSmackGPTHint()
    {
        SpawnCarol();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.ELECTRIC_BOSS));
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(5));
        playerStats.deathsToEnemies.Add(EnemyType.ELECTRIC_BOSS, 6);
        bossController.Attack();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(7, playerStats.deathsToEnemies[EnemyType.ELECTRIC_BOSS]);
        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(5));
    }

    [UnityTest]
    public IEnumerator DontAddHintIfFriendsDead()
    {
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Jeff");
        SpawnCarol();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.ELECTRIC_BOSS));
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(5));
        playerStats.deathsToEnemies.Add(EnemyType.ELECTRIC_BOSS, 6);
        bossController.Attack();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(7, playerStats.deathsToEnemies[EnemyType.ELECTRIC_BOSS]);
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(5));
    }

    [UnityTest]
    public IEnumerator RemoveSmackGPTIfUnread()
    {
        SpawnCarol();
        bossController.attackTime = 1000;
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        dialogueData.smackGPTQueue.Add(5);
        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(5));
        enemyScript.LoseHealthUnblockable(1500, 1);
        Debug.Log(enemyScript.health);
        yield return new WaitForSeconds(3f);
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(5));
    }

    [UnityTest]
    public IEnumerator Death()
    {
        SpawnCarol();
        enemyScript.transform.position = new Vector3(2f, 0, 2f);
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 1);
        yield return new WaitForSeconds(3f);
    }
}
