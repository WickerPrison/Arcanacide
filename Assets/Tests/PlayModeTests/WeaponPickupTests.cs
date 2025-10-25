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

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;

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
}