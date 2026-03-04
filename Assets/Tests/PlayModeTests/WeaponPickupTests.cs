using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class WeaponPickupTests
{
    PlayerData playerData;
    GameObject weaponPickupPrefab;
    DialogueData dialogueData;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        dialogueData = Resources.Load<DialogueData>("Data/DialogueData");
        dialogueData.smackGPTQueue.Clear();
        dialogueData.smackGPTPreviousConversations.Clear();
        weaponPickupPrefab = Resources.Load<GameObject>("Prefabs/Layout/WeaponPickup");

        Time.timeScale = 4;
    }

    [UnityTest]
    public IEnumerator GetLantern()
    {
        SceneManager.LoadScene("BlockRoom");

        yield return null;
        PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player.transform.position = new Vector3(-1, 0, -1);
        yield return null;

        WeaponPickup weaponPickup = GameObject.FindObjectOfType<WeaponPickup>();
        weaponPickup.PerformPickup();

        Assert.Contains(1, playerData.unlockedWeapons);
    }

    [UnityTest]
    public IEnumerator AddSmackGptHintToQueue()
    {
        SceneManager.LoadScene("BlockRoom");

        yield return null;
        PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player.transform.position = new Vector3(-1, 0, -1);
        yield return null;

        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(7));
        WeaponPickup weaponPickup = GameObject.FindObjectOfType<WeaponPickup>();
        weaponPickup.PerformPickup();

        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(7));
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(8));
    }

    [UnityTest]
    public IEnumerator DontAddSmackGptHintToQueueOnLaterPickups()
    {
        SceneManager.LoadScene("BlockRoom");

        yield return null;
        PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player.transform.position = new Vector3(-1, 0, -1);
        dialogueData.smackGPTPreviousConversations.Add(7);
        yield return null;

        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(7));
        WeaponPickup weaponPickup = GameObject.FindObjectOfType<WeaponPickup>();
        weaponPickup.PerformPickup();

        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(7));
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(8));
    }

    [UnityTest]
    public IEnumerator AddSmackGptHintForWeaponTypes()
    {
        SceneManager.LoadScene("BlockRoom");

        yield return null;
        PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player.transform.position = new Vector3(-1, 0, -1);
        yield return null;
        playerData.unlockedWeapons.Add(1);
        playerData.unlockedLanterns.Add(WeaponElement.ELECTRICITY);
        Assert.IsFalse(dialogueData.smackGPTQueue.Contains(8));
        WeaponPickup weaponPickup = GameObject.FindObjectOfType<WeaponPickup>();
        weaponPickup.PerformPickup();

        Assert.IsTrue(dialogueData.smackGPTQueue.Contains(8));
    }
}