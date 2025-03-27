using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MinibossV1Tests
{
    GameObject testDummyPrefab;
    PlayerData playerData;
    EmblemLibrary emblemLibrary;
    GameObject minibossPrefab;
    TestingTrigger triggerPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        emblemLibrary = Resources.Load<EmblemLibrary>("Data/EmblemLibrary");

        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        minibossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/MinibossV1");
        triggerPrefab = Resources.Load<TestingTrigger>("Prefabs/Testing/TestingTrigger");
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
}
