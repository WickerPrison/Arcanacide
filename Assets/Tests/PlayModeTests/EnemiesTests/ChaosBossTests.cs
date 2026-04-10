using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ChaosBossTests
{
    PlayerData playerData;
    MapData mapData;
    PlayerStats playerStats;
    ChaosBossController bossController;
    EnemyScript enemyScript;
    Dialogue dialogue;
    HUD hud;
    AssistantController assistantController;
    PlayerScript playerScript;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("ChaosBossfight");
        yield return null;
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.tutorials.Clear();
        playerData.hasHealthGem = true;
        playerData.vitality = 60;
        playerData.health = playerData.MaxHealth();
        playerStats = Resources.Load<PlayerStats>("Data/PlayerStats");
        playerStats.ClearData();
        bossController = GameObject.FindObjectOfType<ChaosBossController>();
        enemyScript = bossController.GetComponent<EnemyScript>();
        dialogue = enemyScript.GetComponentInChildren<DialogueTriggerRoomEntrance>().GetComponent<Dialogue>();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = Vector3.zero;

        yield return null;
        Time.timeScale = 1f;
    }

    IEnumerator BossSetup()
    {
        
        enemyScript.transform.position = new Vector3(6f, 0, 3f);
        yield return null;
        dialogue.CloseDialogue();
        bossController.attackTime = 60;
        yield return new WaitForSeconds(0.3f);
    }

    [UnityTest]
    public IEnumerator Death()
    {
        enemyScript.transform.position = new Vector3(6f, 0, 4f);
        yield return null;
        dialogue.CloseDialogue();
        bossController.attackTime = 60;
        yield return new WaitForSeconds(0.3f);
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(2);
        enemyScript.GetComponent<Dialogue>().CloseDialogue();
        yield return new WaitForSeconds(1.9f);
    }

    [UnityTest]
    public IEnumerator KnightsAttack()
    {
        enemyScript.transform.position = new Vector3(6f, 0, 3f);
        yield return null;
        dialogue.CloseDialogue();
        bossController.attackTime = 60;
        yield return new WaitForSeconds(0.3f);
        bossController.StartKnightsAttack();
        yield return new WaitForSeconds(3);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator IceRings()
    {
        yield return BossSetup();
        bossController.IceRings();
        yield return new WaitForSeconds(15);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator TrackPlayerKills()
    {
        enemyScript.transform.position = new Vector3(6f, 0, 3f);
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        bossController.attackTime = 60;
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.CHAOS_BOSS));
        bossController.StartKnightsAttack();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(1, playerStats.deathsToEnemies[EnemyType.CHAOS_BOSS]);
    }

    [UnityTest]
    public IEnumerator FireWaves()
    {
        yield return BossSetup();
        playerScript.transform.position = new Vector3(3f, 0, 3f);
        enemyScript.transform.position = new Vector3(-3f, 0, 3f);
        yield return null;
        bossController.StartFireWaves();
        yield return new WaitForSeconds(10f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator DoubleFireWaves()
    {
        yield return BossSetup();
        playerScript.transform.position = new Vector3(3f, 0, 3f);
        enemyScript.transform.position = new Vector3(0.8f, 0, 0);
        yield return null;
        bossController.StartFireWaves();
        yield return new WaitForSeconds(15f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator FireWavesStagger()
    {
        yield return BossSetup();
        playerScript.transform.position = new Vector3(3f, 0, 3f);
        enemyScript.transform.position = new Vector3(-3f, 0, 3f);
        yield return null;
        bossController.StartFireWaves();
        yield return new WaitForSeconds(0.5f);
        enemyScript.LoseHealthUnblockable(1, 10);
        yield return new WaitForSeconds(15f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator Combo()
    {
        yield return BossSetup();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        bossController.Combo();
        bossController.attackTime = 100f;
        yield return new WaitForSeconds(5f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator SummonSnipers()
    {
        yield return BossSetup();
        bossController.attackTime = 100;
        enemyScript.transform.position = new Vector3(3f, 0, -3f);
        yield return null;
        bossController.StartSummonSnipers();
        yield return new WaitForSeconds(7f);
        int health = playerData.health;
        Assert.Less(playerData.health, playerData.MaxHealth());
        bossController.StartSummonSnipers();
        yield return new WaitForSeconds(7f);
        Assert.Less(playerData.health, health);
    }

    [UnityTest]
    public IEnumerator Bolts()
    {
        yield return BossSetup();
        bossController.attackTime = 100;
        enemyScript.transform.position = new Vector3(3f, 0, -3f);
        yield return null;
        bossController.Bolts();
        yield return new WaitForSeconds(15f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator ThrowBombs()
    {
        yield return BossSetup();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        bossController.ThrowBombs();
        bossController.attackTime = 100f;
        yield return new WaitForSeconds(5f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator ThrowBombsPhase2()
    {
        yield return BossSetup();
        bossController.phase = 2;
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        bossController.ThrowBombs();
        bossController.attackTime = 100f;
        yield return new WaitForSeconds(5f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }
}
