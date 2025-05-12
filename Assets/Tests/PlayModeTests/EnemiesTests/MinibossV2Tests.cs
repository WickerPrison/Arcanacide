using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Linq;

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
        Time.timeScale = 4f;
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
        playerScript.transform.position = new Vector3(0, 0, 3);
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;

        minibossAbilities.StartTeslaHarpoon();
        MinibossV2Controller minibossController = minibossAbilities.GetComponent<MinibossV2Controller>();
        minibossController.attackTime = 7;
        yield return new WaitForSeconds(10);
        HarpoonManager harpoonManager = minibossAbilities.GetComponent<HarpoonManager>();
        List<float> distances = harpoonManager.GetDistances();
        Assert.GreaterOrEqual(distances.Min(), harpoonManager.spacing);
    }

    [UnityTest]
    public IEnumerator HarpoonsCount()
    {
        HarpoonManager harpoonManager = new GameObject("harpoonManager").AddComponent<HarpoonManager>();
        harpoonManager.SetupTest(boltsPrefab, 5, 5, 0.2f);

        Vector3[] positions =
        {
             new Vector3(3f, 0, 3f),
             new Vector3(-3f, 0, 3f),
             new Vector3(3f, 0, -3f),
             new Vector3(-3f, 0, -3f),
        };

        List<TeslaHarpoon> harpoons = new List<TeslaHarpoon>();
        for (int i = 0; i < 4; i++)
        {
            harpoons.Add(SpawnTeslaHarpoon(positions[i], harpoonManager));
        }

        yield return new WaitForSeconds(1);
        Assert.AreEqual(6, harpoonManager.GetCount());
        harpoons[0].StartDying();
        harpoons.RemoveAt(0);

        yield return new WaitForSeconds(1);
        Assert.AreEqual(3, harpoonManager.GetCount());

        SpawnTeslaHarpoon(new Vector3(-5, 0, 0), harpoonManager);
        SpawnTeslaHarpoon(new Vector3(5, 0, 0), harpoonManager);

        yield return new WaitForSeconds(1);
        Assert.AreEqual(10, harpoonManager.GetCount());
    }

    TeslaHarpoon SpawnTeslaHarpoon(Vector3 position, HarpoonManager harpoonManager)
    {
        TeslaHarpoon teslaHarpoon = GameObject.Instantiate(harpoonPrefab).GetComponent<TeslaHarpoon>();
        teslaHarpoon.transform.position = position;
        teslaHarpoon.harpoonManager = harpoonManager;
        return teslaHarpoon;
    }
}
