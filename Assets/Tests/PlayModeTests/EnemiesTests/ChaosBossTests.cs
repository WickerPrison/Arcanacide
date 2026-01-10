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
    ChaosBossController bossController;
    EnemyScript enemyScript;
    Dialogue dialogue;
    HUD hud;
    AssistantController assistantController;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("FinalBossTests");
        yield return null;
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.tutorials.Clear();
        playerData.hasHealthGem = true;
        playerData.vitality = 30;
        playerData.health = playerData.MaxHealth();

        bossController = GameObject.FindObjectOfType<ChaosBossController>();
        enemyScript = bossController.GetComponent<EnemyScript>();
        dialogue = enemyScript.GetComponentInChildren<DialogueTriggerRoomEntrance>().GetComponent<Dialogue>();
        enemyScript.transform.position = new Vector3(3f, 0, 3f);


        yield return null;
        Time.timeScale = 1f;
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
}
