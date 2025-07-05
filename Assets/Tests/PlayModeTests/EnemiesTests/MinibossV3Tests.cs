using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MinibossV3Tests
{
    PlayerData playerData;
    MapData mapData;
    GameObject minibossPrefab;
    GameObject dronePrefab;
    TestingTrigger triggerPrefab;
    GameObject ellipsePrefab;
    GameObject harpoonPrefab;
    GameObject boltsPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.vitality = 30;
        playerData.health = playerData.MaxHealth();
        mapData = Resources.Load<MapData>("Data/MapData");
        mapData.miniboss2Killed = false;
        minibossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/MinibossV3");
        dronePrefab = Resources.Load<GameObject>("Prefabs/Enemies/MinibossDrone");

        triggerPrefab = Resources.Load<TestingTrigger>("Prefabs/Testing/TestingTrigger");
        ellipsePrefab = Resources.Load<GameObject>("Prefabs/Layout/EllipseV1");
        harpoonPrefab = Resources.Load<GameObject>("Prefabs/Enemies/TeslaHarpoon");
        boltsPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttacks/Bolts");
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator DronePlasmaShots()
    {
        MinibossDroneController droneController = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController.transform.position = new Vector3(3f, 1.5f, 3f);
        yield return null;

        droneController.FirePlasmaShots();
        yield return new WaitForSeconds(10);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator PlasmaShots()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        MinibossDroneController droneController = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        droneController.transform.position = new Vector3(-3f, 1.5f, 3f);
        yield return null;

        minibossAbilities.PlasmaShots();
        MinibossV3Controller minibossController = minibossAbilities.GetComponent<MinibossV3Controller>();
        minibossController.attackTime = 7;
        yield return new WaitForSeconds(3);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }
}
