using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class DeathTests
{
    PlayerData playerData;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator DeathTest()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.LoseHealth(playerData.health + 1, EnemyAttackType.NONPARRIABLE, null);
        yield return new WaitForSecondsRealtime(4);
    }
}
