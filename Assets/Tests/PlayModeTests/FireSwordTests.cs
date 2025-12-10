using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FireSwordTests
{
    PlayerData playerData;
    GameObject testDummyPrefab;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    WeaponManager weaponManager;
    OrbitFlames orbitFlames;

    [SetUp]
    public void Setup()
    {
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.unlockedWeapons.Add(0);
        playerData.unlockedSwords.Add(WeaponElement.FIRE);
        playerData.equippedElements[0] = WeaponElement.FIRE;
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);
        SceneManager.LoadScene("Testing");

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator OrbitFlames()
    {
        playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        orbitFlames = playerAbilities.GetComponent<OrbitFlames>();
        weaponManager = playerAbilities.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(15f, 0, -15f);
        yield return null;
        weaponManager.SwitchWeapon(0);
        yield return new WaitForSeconds(1.5f);
        playerAbilities.SpecialAttack();
        yield return new WaitForSeconds(2);
        Assert.AreEqual(3, orbitFlames.CountNonNull());
        testDummy1.transform.position = new Vector3(1.5f, 0, -1.5f);
        yield return new WaitForSeconds(6);
        Assert.AreEqual(0, orbitFlames.CountNonNull());
        testDummy1.transform.position = new Vector3(15f, 0, -15f);
        yield return null;
        GlobalEvents.instance.PlayerDealDamage(10000);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(1, orbitFlames.CountNonNull());
        GlobalEvents.instance.PlayerDealDamage(10000);
        yield return null;
        Assert.AreEqual(2, orbitFlames.CountNonNull());
        GlobalEvents.instance.PlayerDealDamage(10000);
        yield return null;
        Assert.AreEqual(3, orbitFlames.CountNonNull());
        GlobalEvents.instance.PlayerDealDamage(10000);
        yield return null;
        Assert.AreEqual(3, orbitFlames.CountNonNull());
    }
}
