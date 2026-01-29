using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class LockOnTests
{
    PlayerData playerData;
    GameObject testDummyPrefab;
    LockOn lockOn;

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

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator LockOnToClosest()
    {
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-3f, 0, -1.5f);
        EnemyScript testDummy3 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy3.transform.position = new Vector3(4f, 0, 0);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(5);
        Assert.AreEqual(testDummy1, lockOn.target);
    }

    [UnityTest]
    public IEnumerator SwapRight()
    {
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-3f, 0, -1.5f);
        EnemyScript testDummy3 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy3.transform.position = new Vector3(4f, 0, 0);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy1, lockOn.target);
        lockOn.SwapTarget(true);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy3, lockOn.target);
        lockOn.SwapTarget(true);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy2, lockOn.target);
    }

    [UnityTest]
    public IEnumerator SwapLeft()
    {
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-3f, 0, -1.5f);
        EnemyScript testDummy3 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy3.transform.position = new Vector3(4f, 0, 0);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy1, lockOn.target);
        lockOn.SwapTarget(false);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy2, lockOn.target);
        lockOn.SwapTarget(false);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy3, lockOn.target);
    }

    [UnityTest]
    public IEnumerator SwapAfterDeath()
    {
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-3f, 0, -1.5f);
        EnemyScript testDummy3 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy3.transform.position = new Vector3(4f, 0, 0);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy1, lockOn.target);
        testDummy1.LoseHealthUnblockable(testDummy1.maxHealth * 2, 1);
        yield return new WaitForSeconds(2);
        Assert.AreEqual(testDummy2, lockOn.target);
    }

    [UnityTest]
    public IEnumerator NewTargetAfterDeath()
    {
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-15f, 0, -15f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy1, lockOn.target);
        testDummy1.LoseHealthUnblockable(testDummy1.maxHealth * 2, 1);
        yield return new WaitForSeconds(2);
        Assert.AreEqual(null, lockOn.target);

        testDummy2.transform.position = new Vector3(1.5f, 0, 1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy2, lockOn.target);
    }

    [UnityTest]
    public IEnumerator DistanceTest()
    {
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(15f, 0, -15f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(1);
        Assert.AreEqual(null, lockOn.target);
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return new WaitForSeconds(1);
        Assert.AreEqual(testDummy1, lockOn.target);
    }
}
