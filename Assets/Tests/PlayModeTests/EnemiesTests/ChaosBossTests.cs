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
    GameObject bossPrefab;
    ChaosBossController bossController;
    EnemyScript enemyScript;
    Dialogue dialogue;
    HUD hud;

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
        bossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/ChaosBoss");

        enemyScript = GameObject.Instantiate(bossPrefab).GetComponent<EnemyScript>();
        bossController = enemyScript.GetComponent<ChaosBossController>();
        dialogue = enemyScript.GetComponentInChildren<DialogueTriggerRoomEntrance>().GetComponent<Dialogue>();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator Death()
    {
        yield return null;
        dialogue.CloseDialogue();
        Debug.Log(dialogue.GetType());
        bossController.attackTime = 60;
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(2);
        enemyScript.GetComponent<Dialogue>().CloseDialogue();
        yield return new WaitForSeconds(1.9f);
        Debug.Log(bossController);
    }
}
