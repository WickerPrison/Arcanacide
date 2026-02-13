using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerStaggerTests
{
    PlayerData playerData;
    GameObject testDummyPrefab;
    LockOn lockOn;
    PlayerScript playerScript;
    PlayerAbilities playerAbilities;
    PlayerMovement playerMovement;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        playerData.ClearData();
        playerData.hasHealthGem = true;

        yield return null;
        lockOn = GameObject.FindGameObjectWithTag("Player").GetComponent<LockOn>();
        playerScript = lockOn.GetComponent<PlayerScript>();
        playerAbilities = lockOn.GetComponent<PlayerAbilities>();
        playerMovement = lockOn.GetComponent<PlayerMovement>();

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator StaggerWhileHeal()
    {
        playerAbilities.Heal();
        yield return new WaitForSeconds(0.5f);
        playerScript.LosePoise(1000);
        yield return new WaitForSeconds(2);
        Assert.IsFalse(playerMovement.canWalk);
    }
}
