using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CalculateLightDpsTests
{
    PlayerData playerData;
    BalanceData balanceData;
    GameObject testDummyPrefab;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    WeaponManager weaponManager;
    TestingEvents testingEvents;
    PlayerScript playerScript;
    float staminaCounter;
    int healthCounter;
    int hitCounter;
    bool doneAttacking = false;
    EnemyScript testDummy;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Testing");
        yield return null;
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        balanceData = Resources.Load<BalanceData>("Data/BalanceData/BalanceData");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.UnlockAllWeapons();
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);

        Time.timeScale = 10;
        staminaCounter = 0;
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        testingEvents = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TestingEvents>();
        playerAbilities = playerAnimation.GetComponent<PlayerAbilities>();
        playerScript = playerAnimation.GetComponent<PlayerScript>();
        PlayerAnimationEvents playerAnimationEvents = playerAnimation.GetComponentInChildren<PlayerAnimationEvents>();
        playerScript.testingEvents = testingEvents;
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
    }

    [UnityTest]
    public IEnumerator CalculateSwordLightCurve()
    {
        balanceData.ClearDps(BalanceAttackType.LIGHT, BalanceWeaponType.SWORD);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoLightCombo(BalanceWeaponType.SWORD, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateLanternLightCurve()
    {
        balanceData.ClearDps(BalanceAttackType.LIGHT, BalanceWeaponType.LANTERN);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoLightCombo(BalanceWeaponType.LANTERN, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateKnifeLightCurve()
    {
        balanceData.ClearDps(BalanceAttackType.LIGHT, BalanceWeaponType.KNIFE);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoLightCombo(BalanceWeaponType.KNIFE, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateClawsLightCurve()
    {
        balanceData.ClearDps(BalanceAttackType.LIGHT, BalanceWeaponType.CLAWS);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoLightCombo(BalanceWeaponType.CLAWS, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateFireSwordLightCurve()
    {
        balanceData.ClearDps(BalanceAttackType.LIGHT, BalanceWeaponType.FIRESWORD);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoLightCombo(BalanceWeaponType.FIRESWORD, stats[i], health[i]);
        }
    }


    IEnumerator DoLightCombo(BalanceWeaponType type, int stat, int health)
    {
        int reportIndex = BalanceTestUtils.weaponIndexDict[type];
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(1.5f, 0, -1.5f);
        testDummy.maxHealth = health;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        int weaponIndex = reportIndex > 3 ? reportIndex - 4 : reportIndex;
        playerData.currentWeapon = weaponIndex == 0 ? 1 : 0;
        playerData.equippedElements[weaponIndex] = BalanceTestUtils.weaponElementDict[type];
        weaponManager.SwitchWeapon(weaponIndex);
        yield return new WaitForSeconds(2);
        playerAbilities.Attack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.SetDps(stat, dps, BalanceAttackType.LIGHT, type);
        balanceData.SetStamPerSecond(stamPerSec, reportIndex, BalanceAttackType.LIGHT);
        balanceData.SetMaxDps(dps, reportIndex, BalanceAttackType.LIGHT);
        balanceData.SetHitRate(hitCounter / seconds, reportIndex, BalanceAttackType.LIGHT);
        Debug.Log($"{type} Light DPS with {stat} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        doneAttacking = true;
        yield return new WaitForSeconds(5);
        testDummy.Death();
    }

    private void TestingEvents_onAttackFalse(object sender, System.EventArgs e)
    {
        staminaCounter += playerData.MaxStamina() - playerScript.stamina;
        playerScript.GainStamina(1000);
        healthCounter += testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(1000);
        hitCounter++;
        if (!doneAttacking)
        {
            playerAbilities.Attack();
        }
    }
}
