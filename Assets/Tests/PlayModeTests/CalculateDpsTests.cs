using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CalculateDpsTests
{
    PlayerData playerData;
    GameObject testDummyPrefab;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    WeaponManager weaponManager;
    TestingEvents testingEvents;
    PlayerScript playerScript;

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
    public IEnumerator SwordLightCombo()
    {
        testingEvents = GlobalEvents.instance.gameObject.GetComponent<TestingEvents>();
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        playerAbilities = playerAnimation.GetComponent<PlayerAbilities>();
        playerScript = playerAnimation.GetComponent<PlayerScript>();
        PlayerAnimationEvents playerAnimationEvents = playerAnimation.GetComponentInChildren<PlayerAnimationEvents>();
        playerAnimationEvents.testingEvents = testingEvents;
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
        EnemyScript testDummy1 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy1.transform.position = new Vector3(2f, 0, -2f);
        testDummy1.maxHealth *= 100;
        testDummy1.health = testDummy1.maxHealth;
        yield return null;
        weaponManager.SwitchWeapon(0);
        yield return new WaitForSeconds(2);
        playerAbilities.Attack();
        yield return new WaitForSeconds(60);
        Assert.Less(testDummy1.health, testDummy1.maxHealth);
    }

    private void TestingEvents_onAttackFalse(object sender, System.EventArgs e)
    {
        playerScript.GainStamina(1000);
        playerAbilities.Attack();
    }
}
