using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ClawTests
{
    PlayerData playerData;
    GameObject testDummyPrefab;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    WeaponManager weaponManager;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.unlockedWeapons.Add(3);
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator Combo2()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(5f, 0, -5f);
        yield return null;
        weaponManager.SwitchWeapon(3);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(3);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
    }

    [UnityTest]
    public IEnumerator Heavy()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(2f, 0, -2f);
        testDummy1.maxHealth *= 10;
        testDummy1.health = testDummy1.maxHealth;
        yield return null;
        weaponManager.SwitchWeapon(3);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("HeavyAttack");
        yield return new WaitForSeconds(10);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
    }
}
