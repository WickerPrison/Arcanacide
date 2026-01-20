using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CalculateCombo1
{
    PlayerData playerData;
    BalanceData balanceData;
    GameObject testDummyPrefab;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    WeaponManager weaponManager;
    TestingEvents testingEvents;
    PlayerScript playerScript;
    LockOn lockOn;
    PlayerMovement playerMovement;
    float staminaCounter;
    int healthCounter;
    int hitCounter;
    bool doneAttacking = false;
    int attacksCounter = 1;
    EnemyScript testDummy;
    Vector3 defaultDist = new Vector3(1.5f, 0, -1.5f);

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
        lockOn = playerAnimation.GetComponent<LockOn>();
        playerMovement = playerAnimation.GetComponent<PlayerMovement>();
        PlayerAnimationEvents playerAnimationEvents = playerAnimation.GetComponentInChildren<PlayerAnimationEvents>();
        playerScript.testingEvents = testingEvents;
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
    }

    [UnityTest]
    public IEnumerator CalculateSwordCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.SWORD);
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
            yield return DoCombo1(BalanceWeaponType.SWORD, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateFireSwordCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.FIRESWORD);
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
            yield return DoCombo1(BalanceWeaponType.FIRESWORD, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateLanternCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.LANTERN);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            playerData.equippedElements[1] = WeaponElement.FIRE;
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            attacksCounter = 1;
            yield return DoCombo1(BalanceWeaponType.LANTERN, stats[i], health[i], 4f);
        }
    }

    [UnityTest]
    public IEnumerator CalculateElectricLanternCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.ELECTRICLANTERN);
        int[] stats = { 1, 10, 20 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            playerData.strength = stats[i];
            playerData.equippedElements[1] = WeaponElement.ELECTRICITY;
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            attacksCounter = 1;
            yield return DoCombo1(BalanceWeaponType.ELECTRICLANTERN, stats[i], health[i], 4f);
        }
    }

    [UnityTest]
    public IEnumerator CalculateKnifeCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.KNIFE);
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
            yield return DoCombo1(BalanceWeaponType.KNIFE, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateIceKnifeCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.ICEKNIFE);
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
            yield return DoCombo1(BalanceWeaponType.ICEKNIFE, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateClawsCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.CLAWS);
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
            yield return DoCombo1(BalanceWeaponType.CLAWS, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateChaosClawsCombo1Curve()
    {
        balanceData.ClearDps(BalanceAttackType.COMBO1, BalanceWeaponType.CHAOSCLAWS);
        int[] stats = { 1, 10, 20 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            attacksCounter = 1;
            yield return DoCombo1(BalanceWeaponType.CHAOSCLAWS, stats[i], health[i]);
        }
    }

    IEnumerator DoCombo1(BalanceWeaponType type, int stat, int health, float targetDist = 1.5f)
    {
        int reportIndex = BalanceTestUtils.weaponIndexDict[type];
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(1, 0, -1) * targetDist;
        testDummy.maxHealth = health;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        lockOn.ToggleLockOn();
        int weaponIndex = reportIndex > 3 ? reportIndex - 4 : reportIndex;
        playerData.equippedElements[weaponIndex] = BalanceTestUtils.weaponElementDict[type];
        playerData.currentWeapon = weaponIndex == 0 ? 1 : 0;
        weaponManager.SwitchWeapon(weaponIndex);
        yield return new WaitForSeconds(2);
        playerAbilities.Attack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.SetDps(stat, dps, BalanceAttackType.COMBO1, type);
        balanceData.SetStamPerSecond(stamPerSec, reportIndex, BalanceAttackType.COMBO1);
        balanceData.SetMaxDps(dps, reportIndex, BalanceAttackType.COMBO1);
        balanceData.SetHitRate(hitCounter / seconds, reportIndex, BalanceAttackType.COMBO1);
        Debug.Log($"{type} Combo1 DPS with {stat} Stat: {dps}");
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
            if(attacksCounter == 2)
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
}
