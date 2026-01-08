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
    GameObject frankPrefab;
    IceBoss bossController;
    EnemyScript enemyScript;
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
        frankPrefab = Resources.Load<GameObject>("Prefabs/Enemies/IceBoss");
        hud = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<HUD>();
        testingEvents = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TestingEvents>();
        Time.timeScale = 1f;
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
    public IEnumerator Death()
    {
        SpawnFrank();
        hud.EnableBossHealthbar(enemyScript.GetComponent<EnemyEvents>());
        yield return null;
        dialogue.CloseDialogue();
        yield return null;
        yield return BossTransition(3);
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(2);
        enemyScript.GetComponent<Dialogue>().CloseDialogue();
        bossController.attackTime = 60;
        yield return new WaitForSeconds(35);
        Assert.IsTrue(mapData.iceBossKilled);
    }
}
