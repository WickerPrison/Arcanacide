using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TutorialTests
{
    PlayerData playerData;
    TutorialManager tutorialManager;
    GameObject remnantPrefab;
    GameObject tutorialTriggerPrefab;

    [UnitySetUp]
    public IEnumerator UnitySetup()
    {
        SceneManager.LoadScene("Testing");
        yield return null;
        tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
    }

    [SetUp]
    public void Setup()
    {
        remnantPrefab = Resources.Load<GameObject>("Prefabs/Player/Remnant");
        tutorialTriggerPrefab = Resources.Load<GameObject>("Prefabs/TutorialMessages/TutorialTrigger");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.tutorials = tutorialManager.allTutorials;
        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator RemnantTutorial()
    {
        Assert.IsTrue(playerData.tutorials.Contains("Remnant"));
        GameObject remnant = GameObject.Instantiate(remnantPrefab);
        remnant.transform.position = new Vector3(2, 0, 2);
        yield return new WaitForSecondsRealtime(2);
        tutorialManager.NextMessage();
        yield return new WaitForSeconds(1);
        Assert.IsFalse(playerData.tutorials.Contains("Remnant"));
    }

    [UnityTest]
    public IEnumerator MapTutorial()
    {
        Assert.IsTrue(playerData.tutorials.Contains("Map"));
        TutorialTrigger trigger = GameObject.Instantiate(tutorialTriggerPrefab).GetComponent<TutorialTrigger>();
        trigger.tutorialName = "Map";
        trigger.playerData = playerData;
        trigger.transform.position = Vector3.zero;
   
        yield return new WaitForSecondsRealtime(2);
        tutorialManager.NextMessage();
        yield return new WaitForSeconds(1);
        Assert.IsFalse(playerData.tutorials.Contains("Map"));
    }
}
