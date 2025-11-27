using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class FourthFloorTests
{
    PlayerData playerData;
    MapData mapData;
    PlayerScript playerScript;
    GameManager gm;
    ResetPassword resetPassword;
    Dialogue dialogue;

    [SetUp]
    public void Setup()
    {
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        mapData = Resources.Load<MapData>("Data/MapData");
        mapData.itWorkerQuestStarted = false;
        mapData.itWorkerQuestComplete = false;
        mapData.resetPasswords.Clear();
    }

    [UnityTest]
    public IEnumerator ResetPasswords()
    {
        Assert.IsFalse(mapData.itWorkerQuestStarted);
        Assert.IsFalse(mapData.itWorkerQuestComplete);
        Assert.Zero(mapData.resetPasswords.Count);
        SceneManager.LoadScene("CPath1.2.1");
        yield return null;
        KillAllEnemies();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(3f, 0, 3);
        yield return null;
        resetPassword = GameObject.FindObjectOfType<ResetPassword>();
        resetPassword.Investigate();
        yield return null;
        dialogue = resetPassword.GetComponent<Dialogue>();
        yield return ProgressDialogue(1);
        Assert.AreEqual(0, mapData.resetPasswords.Count);

        SceneManager.LoadScene("CPath2.1");
        yield return null;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(-1.5f, 0, -3);
        yield return null;
        NPCDialogue npcDialogue = GameObject.FindObjectOfType<NPCDialogue>();
        npcDialogue.StartConversation();
        yield return null;
        for(int i = 0; i < 14; i++)
        {
            npcDialogue.NextLine();
            yield return null;
        }
        Assert.IsTrue(mapData.itWorkerQuestStarted);

        SceneManager.LoadScene("CPath1.2.1");
        yield return null;
        KillAllEnemies();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(3f, 0, 3);
        yield return null;
        resetPassword = GameObject.FindObjectOfType<ResetPassword>();
        resetPassword.Investigate();
        dialogue = resetPassword.GetComponent<Dialogue>();
        yield return ProgressDialogue(4);
        Assert.AreEqual(1, mapData.resetPasswords.Count);  

        SceneManager.LoadScene("CPath4.4.3");
        yield return null;
        KillAllEnemies();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(1.5f, 0, 10);
        yield return null;
        resetPassword = GameObject.FindObjectOfType<ResetPassword>();
        resetPassword.Investigate();
        dialogue = resetPassword.GetComponent<Dialogue>();
        yield return ProgressDialogue(4);
        Assert.AreEqual(2, mapData.resetPasswords.Count);

        SceneManager.LoadScene("CPath5.2");
        yield return null;
        KillAllEnemies();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(-4f, 0, 5.5f);
        yield return null;
        resetPassword = GameObject.FindObjectOfType<ResetPassword>();
        resetPassword.Investigate();
        dialogue = resetPassword.GetComponent<Dialogue>();
        yield return ProgressDialogue(4);
        Assert.AreEqual(3, mapData.resetPasswords.Count);

        SceneManager.LoadScene("CPath6.3");
        yield return null;
        KillAllEnemies();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(7f, 0, 5f);
        yield return null;
        resetPassword = GameObject.FindObjectOfType<ResetPassword>();
        resetPassword.Investigate();
        dialogue = resetPassword.GetComponent<Dialogue>();
        yield return ProgressDialogue(4);
        Assert.AreEqual(4, mapData.resetPasswords.Count);

        SceneManager.LoadScene("CPath2.1");
        yield return null;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        dialogue = GameObject.FindObjectOfType<Dialogue>();
        yield return ProgressDialogue(5);
        yield return new WaitForSeconds(4);
        Assert.Less(playerData.health, playerData.MaxHealth());
        KillAllEnemies();
        yield return new WaitForSeconds(3);
        Assert.IsTrue(mapData.itWorkerQuestComplete);
    }

    void KillAllEnemies()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        foreach (EnemyScript enemy in gm.enemies)
        {
            enemy.LoseHealthUnblockable(enemy.maxHealth, 2);
        }
    }

    IEnumerator ProgressDialogue(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            dialogue.NextLine();
            yield return null;
        }
    }
}
