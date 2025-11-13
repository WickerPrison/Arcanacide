using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CalculateDpsTests
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
    EnemyScript testDummy;
    Dictionary<BalanceWeaponType, int> weaponIndexDict = new Dictionary<BalanceWeaponType, int>
    {
        { BalanceWeaponType.SWORD, 0 },
        { BalanceWeaponType.LANTERN, 1 },
        { BalanceWeaponType.KNIFE, 2 },
        { BalanceWeaponType.CLAWS, 3 },
    };

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Testing");
        yield return null;
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        balanceData = Resources.Load<BalanceData>("Data/BalanceData");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.unlockedWeapons.Add(0);
        playerData.unlockedWeapons.Add(1);
        playerData.unlockedWeapons.Add(2);
        playerData.unlockedWeapons.Add(3);
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);

        Time.timeScale = 10;
        staminaCounter = 0;
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        testingEvents = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TestingEvents>();
        playerAbilities = playerAnimation.GetComponent<PlayerAbilities>();
        playerScript = playerAnimation.GetComponent<PlayerScript>();
        PlayerAnimationEvents playerAnimationEvents = playerAnimation.GetComponentInChildren<PlayerAnimationEvents>();
        playerAnimationEvents.testingEvents = testingEvents;
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
    }

    [UnityTest]
    public IEnumerator CalculateSwordLightCurve()
    {
        balanceData.ClearDps(BalanceWeaponType.SWORD);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            yield return DoLightCombo(BalanceWeaponType.SWORD, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateLanternLightCurve()
    {
        balanceData.ClearDps(BalanceWeaponType.LANTERN);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            yield return DoLightCombo(BalanceWeaponType.LANTERN, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateKnifeLightCurve()
    {
        balanceData.ClearDps(BalanceWeaponType.KNIFE);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            yield return DoLightCombo(BalanceWeaponType.KNIFE, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateClawsLightCurve()
    {
        balanceData.ClearDps(BalanceWeaponType.CLAWS);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for(int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            yield return DoLightCombo(BalanceWeaponType.CLAWS, stats[i], health[i]);
        }
    }

    IEnumerator DoLightCombo(BalanceWeaponType type, int stat, int health)
    {
        int weaponIndex = weaponIndexDict[type];
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2f, 0, -2f);
        testDummy.maxHealth = health;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        playerData.currentWeapon = 3;
        weaponManager.SwitchWeapon(weaponIndex);
        yield return new WaitForSeconds(2);
        playerAbilities.Attack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.SetDps(stat, dps, type);
        balanceData.stamPerSecond[weaponIndex] = stamPerSec;
        balanceData.maxDps[weaponIndex] = dps;
        balanceData.hitRate[weaponIndex] = hitCounter / seconds;
        Debug.Log($"{type} Light DPS with {stat} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        testDummy.Death();
    }

    private void TestingEvents_onAttackFalse(object sender, System.EventArgs e)
    {
        staminaCounter += playerData.MaxStamina() - playerScript.stamina;
        playerScript.GainStamina(1000);
        healthCounter += testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(1000);
        hitCounter++;
        playerAbilities.Attack();
    }
}
