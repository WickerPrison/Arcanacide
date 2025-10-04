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
    GameObject fleePointPrefab;
    GameObject minibossStalagmitesPrefab;

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
        mapData.miniboss3Killed = false;
        minibossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/MinibossV3");
        dronePrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/MinibossDrone");

        triggerPrefab = Resources.Load<TestingTrigger>("Prefabs/Testing/TestingTrigger");
        ellipsePrefab = Resources.Load<GameObject>("Prefabs/Layout/EllipseV1");
        harpoonPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/TeslaHarpoon");
        boltsPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttacks/Bolts");
        fleePointPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/FleePoint");
        minibossStalagmitesPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/MinibossStalagmites");
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator PlasmaShots()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        MinibossDroneController droneController = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        GameObject minibossStalagmites = GameObject.Instantiate(minibossStalagmitesPrefab);
        Transform layout = GameObject.Find("Layout").transform;
        minibossStalagmites.transform.SetParent(layout);
        yield return null;
        droneController.transform.position = droneController.HoverPosition();

        minibossAbilities.PlasmaShots();
        MinibossV3Controller minibossController = minibossAbilities.GetComponent<MinibossV3Controller>();
        minibossController.attackTime = 7;
        yield return new WaitForSeconds(3);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator DroneLaser()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        MinibossDroneController droneController = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        droneController.transform.position = droneController.HoverPosition();

        droneController.StartLaser();
        MinibossV3Controller minibossController = minibossAbilities.GetComponent<MinibossV3Controller>();
        minibossController.attackTime = 70;
        yield return new WaitForSeconds(15);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator DroneLasers()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        MinibossDroneController droneController0 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController0.droneId = 0;
        MinibossDroneController droneController1 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController1.droneId = 1;
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        Transform fleePoint = GameObject.Instantiate(fleePointPrefab).transform;
        fleePoint.position = new Vector3(-8f, 0, 0);
        yield return null;
        droneController0.transform.position = droneController0.HoverPosition();
        droneController1.transform.position = droneController1.HoverPosition();

        MinibossV3Controller minibossController = minibossAbilities.GetComponent<MinibossV3Controller>();
        minibossController.StartLasers();
        minibossController.attackTime = 70;
        playerData.health = 1000;
        yield return new WaitForSeconds(12);
        Assert.Less(playerData.health, 1000);
    }

    [UnityTest]
    public IEnumerator Circle()
    {
        Ellipse ellipse = GameObject.Instantiate(ellipsePrefab).GetComponent<Ellipse>();
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.ellipse = ellipse;
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        MinibossDroneController droneController0 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController0.droneId = 0;
        MinibossDroneController droneController1 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController1.droneId = 1;
        yield return null;
        droneController0.transform.position = droneController0.HoverPosition();
        droneController1.transform.position = droneController1.HoverPosition();

        minibossAbilities.Circle(CircleType.SHOOT);
        MinibossV3Controller minibossController = minibossAbilities.GetComponent<MinibossV3Controller>();
        minibossController.attackTime = 7;
        playerData.health = 2000;
        yield return new WaitForSeconds(5);
        Assert.Less(playerData.health, 2000);
    }

    [UnityTest]
    public IEnumerator DroneCharge()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        MinibossDroneController droneController0 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController0.droneId = 0;
        MinibossDroneController droneController1 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController1.droneId = 1;
        GameObject minibossStalagmites = GameObject.Instantiate(minibossStalagmitesPrefab);
        Transform layout = GameObject.Find("Layout").transform;
        minibossStalagmites.transform.SetParent(layout);
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        droneController0.transform.position = droneController0.HoverPosition();

        droneController0.StartCharge();
        droneController1.StartCharge();
        MinibossV3Controller minibossController = minibossAbilities.GetComponent<MinibossV3Controller>();
        minibossController.attackTime = 70;
        yield return new WaitForSeconds(5);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator Death()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        MinibossDroneController droneController0 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController0.droneId = 0;
        MinibossDroneController droneController1 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController1.droneId = 1;
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        droneController0.transform.position = droneController0.HoverPosition();

        EnemyScript enemyScript = minibossAbilities.GetComponent<EnemyScript>();
        enemyScript.LoseHealth(enemyScript.health, 1);
        yield return new WaitForSeconds(1);
        Dialogue dialogue = minibossAbilities.GetComponentInChildren<Dialogue>();
        dialogue.NextLine();
        yield return new WaitForSeconds(5);
        Assert.IsTrue(droneController1 == null);
        Assert.IsTrue(droneController0 == null);
        Assert.IsTrue(minibossAbilities == null);
    }

    [UnityTest]
    public IEnumerator DeathDuringDroneAttack()
    {
        Ellipse ellipse = GameObject.Instantiate(ellipsePrefab).GetComponent<Ellipse>();
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.ellipse = ellipse;
        MinibossDroneController droneController0 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController0.droneId = 0;
        MinibossDroneController droneController1 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController1.droneId = 1;
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        droneController0.transform.position = droneController0.HoverPosition();

        minibossAbilities.Circle(CircleType.SHOOT);
        yield return new WaitForSeconds(1);

        EnemyScript enemyScript = minibossAbilities.GetComponent<EnemyScript>();
        enemyScript.LoseHealth(enemyScript.health, 1);
        yield return new WaitForSeconds(1);
        Dialogue dialogue = minibossAbilities.GetComponentInChildren<Dialogue>();
        dialogue.NextLine();
        yield return new WaitForSeconds(5);
        Assert.IsTrue(droneController1 == null);
        Assert.IsTrue(droneController0 == null);
        Assert.IsTrue(minibossAbilities == null);
    }
}
