using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MinibossV2Tests
{
    PlayerData playerData;
    MapData mapData;
    GameObject minibossPrefab;
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
        mapData.miniboss1Killed = false;
        minibossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/MinibossV2");
        triggerPrefab = Resources.Load<TestingTrigger>("Prefabs/Testing/TestingTrigger");
        ellipsePrefab = Resources.Load<GameObject>("Prefabs/Layout/EllipseV1");
        harpoonPrefab = Resources.Load<GameObject>("Prefabs/Enemies/TeslaHarpoon");
        boltsPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttacks/Bolts");
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator Missiles()
    {
        TestingTrigger frontTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<TestingTrigger>();
        frontTrigger.callback = collider => collider.gameObject.GetComponent<Missile>();
        frontTrigger.transform.position = new Vector3(20, 1, 0);
        TestingTrigger outerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<TestingTrigger>();
        outerTrigger.callback = collider => collider.gameObject.GetComponent<Missile>();
        outerTrigger.transform.position = new Vector3(-20, 1, 0);

        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(-3f, 0, -3f);
        yield return null;
        MinibossV2Controller minibossController = minibossAbilities.GetComponent<MinibossV2Controller>();
        minibossController.attackTime = 1000;
        minibossAbilities.MissileAttack(MissilePattern.RADIAL);
        yield return new WaitForSeconds(2);
        Assert.Greater(frontTrigger.counter, 0);
        Assert.Greater(outerTrigger.counter, 0);
    }

    [UnityTest]
    public IEnumerator CircleLaser()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(2, 0, 2);
        Ellipse ellipse = GameObject.Instantiate(ellipsePrefab).GetComponent<Ellipse>();
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.ellipse = ellipse;
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;

        minibossAbilities.Circle(CircleType.LASER);
        MinibossV2Controller minibossController = minibossAbilities.GetComponent<MinibossV2Controller>();
        minibossController.attackTime = 7;
        yield return new WaitForSeconds(3);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator TeslaHarpoons()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(2, 0, 2);
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;

        minibossAbilities.StartTeslaHarpoon();
        MinibossV2Controller minibossController = minibossAbilities.GetComponent<MinibossV2Controller>();
        minibossController.attackTime = 7;
        yield return new WaitForSeconds(20);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator HarpoonsCount()
    {
        HarpoonManager harpoonManager = new GameObject("harpoonManager").AddComponent<HarpoonManager>();
        harpoonManager.boltsPrefab = boltsPrefab;
        TeslaHarpoon teslaHarpoon1 = GameObject.Instantiate(harpoonPrefab).GetComponent<TeslaHarpoon>();
        teslaHarpoon1.transform.position = new Vector3(-3f, 0, 3f);
        teslaHarpoon1.harpoonManager = harpoonManager;
        TeslaHarpoon teslaHarpoon2 = GameObject.Instantiate(harpoonPrefab).GetComponent<TeslaHarpoon>();
        teslaHarpoon2.transform.position = new Vector3(3f, 0, 3f);
        teslaHarpoon2.harpoonManager = harpoonManager;
        TeslaHarpoon teslaHarpoon3 = GameObject.Instantiate(harpoonPrefab).GetComponent<TeslaHarpoon>();
        teslaHarpoon3.transform.position = new Vector3(-3f, 0, -3f);
        teslaHarpoon3.harpoonManager = harpoonManager;
        TeslaHarpoon teslaHarpoon4 = GameObject.Instantiate(harpoonPrefab).GetComponent<TeslaHarpoon>();
        teslaHarpoon4.transform.position = new Vector3(3f, 0, -3f);
        teslaHarpoon4.harpoonManager = harpoonManager;

        yield return new WaitForSeconds(5);
        Assert.AreEqual(harpoonManager.GetCount(), 6);

        teslaHarpoon4.StartDying();
        yield return new WaitForSeconds(3);
        Assert.AreEqual(harpoonManager.GetCount(), 3);
    }
}
