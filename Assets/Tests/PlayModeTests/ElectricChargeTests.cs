using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ElectricChargeTests
{
    GameObject testDummyPrefab;
    PlayerData playerData;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;

        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator Shock()
    {
        GameObject testDummy = GameObject.Instantiate(testDummyPrefab);
        testDummy.transform.position = Vector3.right * 2f;
        EnemyScript enemyScript = testDummy.GetComponent<EnemyScript>();
        yield return null;

        enemyScript.GainElectricCharge(15);
        yield return new WaitForSeconds(2);
    }

}
