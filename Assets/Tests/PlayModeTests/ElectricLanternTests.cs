using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ElectricLanternTests
{
    PlayerData playerData;
    GameObject testDummyPrefab;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    PlayerMovement playerMovement;
    WeaponManager weaponManager;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.unlockedWeapons.Add(1);
        playerData.equippedElements[1] = WeaponElement.ELECTRICITY;
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator Combo1With1Target()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        playerAnimation = playerAbilities.GetComponent<PlayerAnimation>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(7f, 0, -4f);
        yield return null;
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        playerAnimation.PlayAnimation("Combo");
        yield return new WaitForSeconds(4);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
        Assert.Greater(testDummy1.electricCharge, 0);
    }

    [UnityTest]
    public IEnumerator Combo1With2Targets()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        playerAnimation = playerAbilities.GetComponent<PlayerAnimation>();
        playerMovement = playerAnimation.GetComponent<PlayerMovement>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(7f, 0, -4f);
        EnemyScript testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(7f, 0, 4f);
        yield return null;
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        playerMovement.SetLookDirection(Vector3.right);
        playerMovement.LockAttackPoint();
        yield return null;
        playerAnimation.PlayAnimation("Combo");
        yield return new WaitForSeconds(4);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
        Assert.Greater(testDummy1.electricCharge, 0);
        Assert.Less(testDummy2.health, testDummy1.maxHealth);
        Assert.Greater(testDummy2.electricCharge, 0);
    }
}
