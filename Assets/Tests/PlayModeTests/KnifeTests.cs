using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class KnifeTests
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
        playerData.UnlockAllWeapons();
        playerData.equippedElements[2] = WeaponElement.ELECTRICITY;
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator Heavy()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAbilities.HeavyAttack();
        yield return new WaitForSeconds(4);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
        Assert.Greater(testDummy1.electricCharge, 0);
    }

    [UnityTest]
    public IEnumerator HeavyEnemyDeath()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAbilities.HeavyAttack();
        yield return new WaitForSeconds(2);
        testDummy1.LoseHealthUnblockable(10000, 2);
        KnifeTrap trap = GameObject.FindObjectOfType<KnifeTrap>();
        yield return new WaitForSeconds(2);
        Assert.AreEqual(0, trap.GetEnemiesInRangeCount());
    }

    [UnityTest]
    public IEnumerator HeavyTimeoutEnemyCount()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAbilities.HeavyAttack();
        yield return new WaitForSeconds(5);
        KnifeTrap trap = GameObject.FindObjectOfType<KnifeTrap>();
        yield return new WaitForSeconds(2);
        Assert.AreEqual(0, trap.GetEnemiesInRangeCount());
    }

    [UnityTest]
    public IEnumerator SpecialAttack()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(4f, 0, -1.5f);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAbilities.SpecialAttack();
        yield return new WaitForSeconds(4);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
        Assert.Greater(testDummy1.electricCharge, 0);
    }

    [UnityTest]
    public IEnumerator Combo1()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(3.5f, 0, -3.5f);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo1");
        yield return new WaitForSeconds(2);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
        Assert.Greater(testDummy1.electricCharge, 0);
    }

    [UnityTest]
    public IEnumerator Combo2With4Enemies()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(4, 0, 4);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-2, 0, 4);
        EnemyScript testDummy3 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy3.transform.position = new Vector3(-3, 0, -2);
        EnemyScript testDummy4 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy4.transform.position = new Vector3(2, 0, -4);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(2);
        Assert.AreEqual(testDummy1.health, testDummy1.maxHealth);
        Assert.Less(testDummy4.health, testDummy4.maxHealth);
        Assert.AreEqual(testDummy4.health, testDummy2.health);
        Assert.AreEqual(testDummy2.health, testDummy3.health);
    }

    [UnityTest]
    public IEnumerator Combo2With3Enemies()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(4, 0, 4);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-2, 0, 4);
        EnemyScript testDummy3 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy3.transform.position = new Vector3(-3, 0, -2);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(2);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
        Assert.AreEqual(testDummy1.health, testDummy2.health);
        Assert.AreEqual(testDummy2.health, testDummy3.health);
    }

    [UnityTest]
    public IEnumerator Combo2With2Enemies()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(-3, 0, -2);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(-2, 0, 4);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(2);
        Assert.Less(testDummy2.health, testDummy2.maxHealth);
        Assert.Less(testDummy1.health, testDummy2.health);
    }

    [UnityTest]
    public IEnumerator Combo2With1Enemy()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(-3, 0, -2);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(2);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
    }

    [UnityTest]
    public IEnumerator Combo2With0Enemies()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo2");
        yield return new WaitForSeconds(2);
    }
}
