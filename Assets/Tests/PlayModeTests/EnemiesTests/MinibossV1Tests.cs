using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MinibossV1Tests
{
    PlayerData playerData;
    MapData mapData;
    GameObject minibossPrefab;
    TestingTrigger triggerPrefab;
    GameObject ellipsePrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        mapData = Resources.Load<MapData>("Data/MapData");
        mapData.miniboss1Killed = false;
        minibossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/MinibossV1");
        triggerPrefab = Resources.Load<TestingTrigger>("Prefabs/Testing/TestingTrigger");
        ellipsePrefab = Resources.Load<GameObject>("Prefabs/Layout/EllipseV1");
        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator Missiles()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(-13.5f, 0, -13.5f);
        TestingTrigger innerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<TestingTrigger>();
        innerTrigger.callback = collider => collider.gameObject.GetComponent<Missile>();
        TestingTrigger outerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<TestingTrigger>();
        outerTrigger.callback = collider => collider.gameObject.GetComponent<Missile>();
        outerTrigger.transform.position = new Vector3(-28.5f, 1, -28.5f);

        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(-11.5f, 0, -7.5f);
        yield return null;

        minibossAbilities.MissileAttack();
        yield return new WaitForSeconds(2);
        Assert.Greater(innerTrigger.counter, 0);
        Assert.AreEqual(0, outerTrigger.counter);
    }

    [UnityTest]
    public IEnumerator SingleMissile()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        yield return null;

        minibossAbilities.SingleMissile(playerScript.transform.position, 0.1f);
        yield return new WaitForSeconds(0.2f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator ChestLaser()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        playerData.health += 100;
        yield return null;

        minibossAbilities.ChestLaser(2);
        yield return new WaitForSeconds(3);
        Assert.Less(playerData.health, playerData.MaxHealth() + 100);
    }

    [UnityTest]
    public IEnumerator LaserFromOffscreenZ()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(0, 0, 6.1f);
        TestingTrigger innerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<TestingTrigger>();
        innerTrigger.callback = collider => collider.gameObject.GetComponent<Missile>();
        playerData.health += 100;
        yield return null;

        minibossAbilities.ChestLaser(2);
        yield return new WaitForSeconds(3);
        Assert.Greater(innerTrigger.counter, 0);
    }

    [UnityTest]
    public IEnumerator LaserFromOffscreenX()
    {
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.transform.position = new Vector3(8.1f, 0, 0);
        TestingTrigger innerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<TestingTrigger>();
        innerTrigger.callback = collider => collider.gameObject.GetComponent<Missile>();
        playerData.health += 100;
        yield return null;

        minibossAbilities.ChestLaser(2);
        yield return new WaitForSeconds(3);
        Assert.Greater(innerTrigger.counter, 0);
    }

    [UnityTest]
    public IEnumerator Circle()
    {
        Ellipse ellipse = GameObject.Instantiate(ellipsePrefab).GetComponent<Ellipse>();
        MinibossAbilities minibossAbilities = GameObject.Instantiate(minibossPrefab).GetComponent<MinibossAbilities>();
        minibossAbilities.ellipse = ellipse;
        minibossAbilities.transform.position = new Vector3(3f, 0, 3f);
        playerData.health += 300;
        yield return null;

        minibossAbilities.Circle();
        MinibossV1Controller minibossController = minibossAbilities.GetComponent<MinibossV1Controller>();
        minibossController.attackTime = 7;
        yield return new WaitForSeconds(5);
        Assert.Less(playerData.health, playerData.MaxHealth() + 300);
    }
}
