using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MinibossV4Tests
{
    PlayerData playerData;
    MapData mapData;
    GameObject minibossPrefab;
    GameObject dronePrefab;
    TestingTrigger triggerPrefab;
    GameObject ellipsePrefab;
    GameObject spinPointsPrefab;
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
        mapData.miniboss4Killed = false;
        minibossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/MinibossV4");
        dronePrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/MinibossDrone");

        triggerPrefab = Resources.Load<TestingTrigger>("Prefabs/Testing/TestingTrigger");
        ellipsePrefab = Resources.Load<GameObject>("Prefabs/Layout/EllipseV1");
        spinPointsPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/SpinPoints");
        harpoonPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/TeslaHarpoon");
        boltsPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttacks/Bolts");
        fleePointPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/FleePoint");
        minibossStalagmitesPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Miniboss/MinibossStalagmites");
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator Harpoons()
    {
        SpinPoints spinPoints = GameObject.Instantiate(spinPointsPrefab).GetComponent<SpinPoints>();
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.spinPoints = spinPoints;
        MinibossDroneController droneController0 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController0.droneId = 0;
        MinibossDroneController droneController1 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController1.droneId = 1;
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        GameObject minibossStalagmites = GameObject.Instantiate(minibossStalagmitesPrefab);
        Transform layout = GameObject.Find("Layout").transform;
        minibossStalagmites.transform.SetParent(layout);
        yield return null;
        minibossAbilities.StartTeslaHarpoon();

        yield return new WaitForSeconds(8);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator Death()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        EnemyScript enemyScript = minibossAbilities.GetComponent<EnemyScript>();
        MinibossDroneController droneController0 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController0.droneId = 0;
        droneController0.minibossNum = 4;
        MinibossDroneController droneController1 = GameObject.Instantiate(dronePrefab).GetComponent<MinibossDroneController>();
        droneController1.droneId = 1;
        droneController1.minibossNum = 4;
        minibossAbilities.transform.position = new Vector3(3f, 0, -3f);
        yield return null;
        enemyScript.LoseHealth(enemyScript.maxHealth, 1);
        yield return new WaitForSeconds(1);
        Dialogue dialogue = minibossAbilities.GetComponentInChildren<Dialogue>();
        dialogue.NextLine();
        yield return new WaitForSeconds(3);
        Assert.IsTrue(droneController1 == null);
        Assert.IsTrue(droneController0 == null);
        Assert.IsTrue(minibossAbilities == null);
    }
}
