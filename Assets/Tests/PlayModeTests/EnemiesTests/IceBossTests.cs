using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class IceBossTests
{
    PlayerData playerData;
    MapData mapData;
    PlayerStats playerStats;
    DialogueData dialogueData;
    GameObject frankPrefab;
    IceBoss bossController;
    EnemyScript enemyScript;
    LockOn lockOn;
    Dialogue dialogue;
    HUD hud;
    TestingEvents testingEvents;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Testing");
        yield return null;
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.tutorials.Clear();
        playerData.hasHealthGem = true;
        playerData.vitality = 30;
        playerData.health = playerData.MaxHealth();
        mapData = Resources.Load<MapData>("Data/MapData");
        mapData.iceBossKilled = false;
        playerStats = Resources.Load<PlayerStats>("Data/PlayerStats");
        playerStats.ClearData();
        dialogueData = Resources.Load<DialogueData>("Data/DialogueData");
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        frankPrefab = Resources.Load<GameObject>("Prefabs/Enemies/IceBoss");
        hud = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<HUD>();
        testingEvents = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TestingEvents>();
        Time.timeScale = 4f;

        lockOn = GameObject.FindGameObjectWithTag("Player").GetComponent<LockOn>();
        yield return null;
    }

    void SpawnFrank()
    {
        enemyScript = GameObject.Instantiate(frankPrefab).GetComponent<EnemyScript>();
        bossController = enemyScript.GetComponent<IceBoss>();
        dialogue = enemyScript.GetComponentInChildren<DialogueTriggerRoomEntrance>().GetComponent<Dialogue>();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
    }

    IEnumerator BossTransition(int steps)
    {
        float[] damages = { 0.11f, 0.21f, 0.31f };
        for(int i = 0; i < steps; i++)
        {
            enemyScript.LoseHealthUnblockable(Mathf.CeilToInt(enemyScript.maxHealth * damages[i]), 2);
            yield return new WaitForSeconds(1);
        }
    }

    [UnityTest]
    public IEnumerator RingBlast()
    {
        SpawnFrank();
        hud.EnableBossHealthbar(enemyScript.GetComponent<EnemyEvents>());
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        yield return BossTransition(3);
        bossController.StartRingBlast();
        bossController.attackTime = 1000;
        yield return new WaitForSeconds(3);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator RangedStuff()
    {
        SpawnFrank();
        enemyScript.transform.position = new Vector3(7.5f, 0, -5f);
        bossController.attackTime = 100f;
        bossController.navAgent.speed = 0;
        Time.timeScale = 1;
        hud.EnableBossHealthbar(enemyScript.GetComponent<EnemyEvents>());
        yield return null;
        dialogue.CloseDialogue();
        yield return new WaitForSeconds(5);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator BreathAttack()
    {
        Time.timeScale = 1f;
        SpawnFrank();
        enemyScript.transform.position = new Vector3(3f, 0, 1f);
        hud.EnableBossHealthbar(enemyScript.GetComponent<EnemyEvents>());
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        bossController.BreathAttack();
        bossController.attackTime = 1000;
        yield return new WaitForSeconds(3);
        enemyScript.transform.position = new Vector3(3f, 0, -1f);
        bossController.BreathAttack();
        yield return new WaitForSeconds(3);
        yield return BossTransition(3);
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        bossController.attackTime = 1000;
        yield return new WaitForSeconds(2);
        enemyScript.GetComponent<Dialogue>().CloseDialogue();
        bossController.BreathAttack();
        yield return new WaitForSeconds(3);
        enemyScript.transform.position = new Vector3(3f, 0, 1f);
        bossController.BreathAttack();
        yield return new WaitForSeconds(3);
        enemyScript.transform.position = new Vector3(-3f, 0, 1f);
        bossController.BreathAttack();
        yield return new WaitForSeconds(3);
        enemyScript.transform.position = new Vector3(-3f, 0, -1f);
        bossController.BreathAttack();
        yield return new WaitForSeconds(3);
    }

    [UnityTest]
    public IEnumerator Death()
    {
        SpawnFrank();
        hud.EnableBossHealthbar(enemyScript.GetComponent<EnemyEvents>());
        yield return null;
        lockOn.ToggleLockOn();
        dialogue.CloseDialogue();
        yield return null;
        Assert.AreEqual(enemyScript, lockOn.target);
        yield return BossTransition(3);
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(2);
        Assert.AreEqual(enemyScript, lockOn.target);
        enemyScript.GetComponent<Dialogue>().CloseDialogue();
        bossController.attackTime = 60;
        yield return new WaitForSeconds(35);
        Assert.IsTrue(mapData.iceBossKilled);
    }

    [UnityTest]
    public IEnumerator TrackPlayerKills()
    {
        SpawnFrank();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.ICE_BOSS));
        bossController.BreathAttack();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(1, playerStats.deathsToEnemies[EnemyType.ICE_BOSS]);
    }

    [UnityTest]
    public IEnumerator AddSmackGPTHint()
    {
        SpawnFrank();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.ICE_BOSS));
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(6));
        playerStats.deathsToEnemies.Add(EnemyType.ICE_BOSS, 6);
        bossController.BreathAttack();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(7, playerStats.deathsToEnemies[EnemyType.ICE_BOSS]);
        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(6));
    }

    [UnityTest]
    public IEnumerator RemoveSmackGPTIfUnread()
    {
        SpawnFrank();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        dialogueData.smackGPTQueue.Add(6);
        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(6));
        yield return BossTransition(3);
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(2);
        enemyScript.GetComponent<Dialogue>().CloseDialogue();
        bossController.attackTime = 60;
        yield return new WaitForSeconds(35);
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(6));
    }
}
