using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CalculateCombo2
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
    int attacksCounter = 1;
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
        playerScript.testingEvents = testingEvents;
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        testingEvents.onFaerieReturn += TestingEvents_onFaerieReturn;
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
    }

    [UnityTest]
    public IEnumerator CalculateSwordCombo2Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO2, BalanceWeaponType.SWORD);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            attacksCounter = 1;
            yield return DoCombo2(BalanceWeaponType.SWORD, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateLanternCombo2Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO2, BalanceWeaponType.LANTERN);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            attacksCounter = 1;
            yield return DoCombo2(BalanceWeaponType.LANTERN, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateKnifeCombo2Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO2, BalanceWeaponType.KNIFE);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            attacksCounter = 1;
            yield return DoCombo2(BalanceWeaponType.KNIFE, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateClawsCombo2Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO2, BalanceWeaponType.CLAWS);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            attacksCounter = 1;
            yield return DoCombo2(BalanceWeaponType.CLAWS, stats[i], health[i]);
        }
    }

    IEnumerator DoCombo2(BalanceWeaponType type, int stat, int health)
    {
        int weaponIndex = weaponIndexDict[type];
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(1.5f, 0, -1.5f);
        testDummy.maxHealth = health;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        playerData.currentWeapon = weaponIndex + 1 < 4 ? weaponIndex + 1 : 0;
        weaponManager.SwitchWeapon(weaponIndex);
        yield return new WaitForSeconds(2);
        playerAbilities.Attack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.SetDps(stat, dps, BalanceAttackType.COMBO2, type);
        balanceData.combo2StamPerSecond[weaponIndex] = stamPerSec;
        balanceData.combo2MaxDps[weaponIndex] = dps;
        balanceData.combo2HitRate[weaponIndex] = hitCounter / seconds;
        Debug.Log($"{type} Combo2 DPS with {stat} Stat: {dps}");
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
        attacksCounter++;
        if (!doneAttacking)
        {
            if(attacksCounter == 3)
            {
                playerAbilities.HeavyAttack();
                attacksCounter = 0;
            }
            else
            {
                playerAbilities.Attack();
            }

        }
    }

    private void TestingEvents_onFaerieReturn(object sender, System.EventArgs e)
    {
        staminaCounter += playerData.MaxStamina() - playerScript.stamina;
        playerScript.GainStamina(1000);
        healthCounter += testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(1000);
        attacksCounter = 1;
        playerAbilities.Attack();
    }
}
