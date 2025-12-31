using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class IceKnifeTests
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
        playerData.equippedElements[2] = WeaponElement.ICE;
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator SpecialAttack()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(7f, 0, -1.5f);
        yield return null;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAbilities.SpecialAttack();
        yield return new WaitForSeconds(4);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
        Assert.Less(playerData.mana, playerData.maxMana);
    }
}
