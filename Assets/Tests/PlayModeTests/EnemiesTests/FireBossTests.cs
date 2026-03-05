using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class FireBossTests
{
    PlayerData playerData;
    PlayerStats playerStats;
    DialogueData dialogueData;
    MapData mapData;
    GameObject bossPrefab;
    BossController bossController;
    EnemyScript enemyScript;
    Dialogue dialogue;
    Dialogue endDialogue;

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
        mapData.fireBossKilled = false;
        mapData.carolsDeadFriends.Clear();
        playerStats = Resources.Load<PlayerStats>("Data/PlayerStats");
        playerStats.ClearData();
        dialogueData = Resources.Load<DialogueData>("Data/DialogueData");
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        bossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Boss");
        Time.timeScale = 1f;
    }

    void SpawnBoss()
    {
        enemyScript = GameObject.Instantiate(bossPrefab).GetComponent<EnemyScript>();
        bossController = enemyScript.GetComponent<BossController>();
        dialogue = enemyScript.GetComponentInChildren<DialogueTriggerRoomEntrance>().GetComponent<Dialogue>();
        endDialogue = enemyScript.GetComponentInChildren<BossAnimationEvents>().GetComponent<Dialogue>();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
        GameObject exitDoor = new GameObject();
        bossController.exitDoor = exitDoor.transform;
        exitDoor.transform.position = new Vector3(-7f, 0, -8f);
    }

    [UnityTest]
    public IEnumerator GroundFireAfterDeath()
    {
        SpawnBoss();
        bossController.attackTime = 100f;
        yield return null;
        dialogue.CloseDialogue();
        bossController.StartGroundFire();
        yield return new WaitForSeconds(3f);
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 1);
        int startHealth = playerData.health;
        yield return new WaitForSeconds(2f);
        Assert.LessOrEqual(Mathf.Abs(startHealth - playerData.health), 5);
        endDialogue.CloseDialogue();
        yield return new WaitForSeconds(2f);
        Assert.LessOrEqual(Mathf.Abs(startHealth - playerData.health), 5);
    }

    [UnityTest]
    public IEnumerator FireTrailAfterDeath()
    {
        SpawnBoss();
        playerData.equippedPatches.Add(Patches.ARCANE_STEP);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PatchEffects>().ArcaneStepDodgeThrough();
        bossController.attackTime = 100f;
        yield return null;
        dialogue.CloseDialogue();
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 1);
        int startHealth = playerData.health;
        yield return new WaitForSeconds(2f);
        Assert.LessOrEqual(Mathf.Abs(startHealth - playerData.health), 5);
        endDialogue.CloseDialogue();
        yield return new WaitForSeconds(6f);
        Assert.LessOrEqual(Mathf.Abs(startHealth - playerData.health), 5);
    }

    [UnityTest]
    public IEnumerator TrackPlayerKills()
    {
        SpawnBoss();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.FIRE_BOSS));
        bossController.FireBalls();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(1, playerStats.deathsToEnemies[EnemyType.FIRE_BOSS]);
    }

    [UnityTest]
    public IEnumerator AddSmackGPTHint()
    {
        SpawnBoss();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        Assert.IsFalse(playerStats.deathsToEnemies.ContainsKey(EnemyType.FIRE_BOSS));
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(4));
        playerStats.deathsToEnemies.Add(EnemyType.FIRE_BOSS, 6);
        bossController.FireBalls();
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(7, playerStats.deathsToEnemies[EnemyType.FIRE_BOSS]);
        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(4));
    }

    [UnityTest]
    public IEnumerator RemoveSmackGPTIfUnread()
    {
        SpawnBoss();
        playerData.health = 1;
        yield return null;
        dialogue.CloseDialogue();
        dialogueData.smackGPTQueue.Add(4);
        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(4));
        enemyScript.LoseHealthUnblockable(1000, 1);
        yield return new WaitForSeconds(3f);
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(4));
    }
}
