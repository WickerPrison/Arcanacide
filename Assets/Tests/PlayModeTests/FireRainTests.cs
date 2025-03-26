using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class FireRainTests
{
    PlayerData playerData;
    FireRainTestingTrigger triggerPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        triggerPrefab = Resources.Load<FireRainTestingTrigger>("Prefabs/Testing/FireRainTestingTrigger");
        playerData.ClearData();
        playerData.hasHealthGem = true;

        Time.timeScale = 4;
    }

    [UnityTest]
    public IEnumerator FireRainWithNavmesh()
    {
        playerData.unlockedWeapons.Add(1);
        FireRainTestingTrigger innerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<FireRainTestingTrigger>();
        FireRainTestingTrigger outerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<FireRainTestingTrigger>();
        outerTrigger.transform.position = new Vector3(-28.5f, 1, -28.5f);
        PlayerAnimation playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        WeaponManager weaponManager = playerAnimation.GetComponent<WeaponManager>();
        playerAnimation.transform.position = new Vector3(-13.5f, 0, -13.5f);
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(5);
        Assert.Greater(innerTrigger.counter, 0);
        Assert.AreEqual(0, outerTrigger.counter);
    }

    [UnityTest]
    public IEnumerator FireRainNoNavmesh()
    {
        playerData.unlockedWeapons.Add(1);
        SceneManager.LoadScene("NoNavmesh");
        yield return null;
        FireRainTestingTrigger innerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<FireRainTestingTrigger>();
        FireRainTestingTrigger outerTrigger = GameObject.Instantiate(triggerPrefab).GetComponent<FireRainTestingTrigger>();
        outerTrigger.transform.position = new Vector3(-28.5f, 1, -28.5f);
        PlayerAnimation playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        WeaponManager weaponManager = playerAnimation.GetComponent<WeaponManager>();
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        playerAnimation.transform.position = new Vector3(-13.5f, 0, -13.5f);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(5);
        Assert.Greater(innerTrigger.counter, 0);
        Assert.AreEqual(0, outerTrigger.counter);
    }
}
