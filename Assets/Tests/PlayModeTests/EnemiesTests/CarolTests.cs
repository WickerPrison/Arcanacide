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
        carolPrefab = Resources.Load<GameObject>("Prefabs/Enemies/ElectricBoss");
        filingCabinetPrefab = Resources.Load<GameObject>("Prefabs/Layout/FilingCabinet");
        Time.timeScale = 4f;
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
}
