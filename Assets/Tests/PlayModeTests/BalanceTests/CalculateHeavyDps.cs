using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CalculateHeavyDps
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
    PlayerAttackHitEvents playerAttackHitEvents;

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
        playerData.UnlockAllWeapons();
        playerData.unlockedAbilities.Add(UnlockableAbilities.SPECIAL_ATTACK);
        playerData.dexterity = 30;

        Time.timeScale = 10;
        staminaCounter = 0;
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        testingEvents = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TestingEvents>();
        playerAbilities = playerAnimation.GetComponent<PlayerAbilities>();
        playerScript = playerAnimation.GetComponent<PlayerScript>();
        playerAttackHitEvents = playerScript.GetComponentInChildren<PlayerAttackHitEvents>();
        playerScript.testingEvents = testingEvents;
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
    }

    [UnityTest]
    public IEnumerator CalculateSwordHeavyCurve()
    {
        balanceData.swordHeavyDps.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            playerData.equippedElements[0] = WeaponElement.DEFAULT;
            doneAttacking = false;
            yield return DoSwordHeavy(0, stats[i], BalanceWeaponType.SWORD, BalanceAttackType.HEAVY);
        }
    }

    [UnityTest]
    public IEnumerator CalculateSwordHeavyCurveNoCharge()
    {
        balanceData.swordHeavyDpsNoCharge.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            playerData.equippedElements[0] = WeaponElement.DEFAULT;
            doneAttacking = false;
            yield return DoSwordHeavy(0, stats[i], BalanceWeaponType.SWORD, BalanceAttackType.HEAVY_NO_CHARGE);
        }
    }

    [UnityTest]
    public IEnumerator CalculateFireSwordHeavyCurve()
    {
        balanceData.fireSwordHeavyDps.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            playerData.equippedElements[0] = WeaponElement.FIRE;
            doneAttacking = false;
            yield return DoSwordHeavy(4, stats[i], BalanceWeaponType.FIRESWORD, BalanceAttackType.HEAVY);
        }
    }

    [UnityTest]
    public IEnumerator CalculateFireSwordHeavyCurveNoCharge()
    {
        balanceData.fireSwordHeavyDpsNoCharge.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            playerData.equippedElements[0] = WeaponElement.FIRE;
            doneAttacking = false;
            yield return DoSwordHeavy(4, stats[i], BalanceWeaponType.FIRESWORD, BalanceAttackType.HEAVY_NO_CHARGE);
        }
    }

    IEnumerator DoSwordHeavy(int reportIndex, int stat, BalanceWeaponType type, BalanceAttackType attackType)
    {
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        if(attackType == BalanceAttackType.HEAVY)
        {
            testingEvents.onFullyCharged += (sender, e) =>
            {
                playerAttackHitEvents.chargeTimer = 1;
                playerAbilities.EndHeavyAttack();
            };
        }
        else
        {
            testingEvents.onStartCharging += TestingEvents_onStartCharging;
        }
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2f, 0, -2f);
        testDummy.maxHealth *= 100;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        playerData.currentWeapon = 3;
        weaponManager.SwitchWeapon(0);
        yield return new WaitForSeconds(2);
        playerAbilities.HeavyAttack();
        if(attackType == BalanceAttackType.HEAVY_NO_CHARGE)
        {
            yield return null;
            playerAbilities.EndHeavyAttack();
        }
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.SetDps(stat, dps, attackType, type);
        if(attackType == BalanceAttackType.HEAVY)
        {
            balanceData.heavyStamPerSecond[reportIndex] = stamPerSec;
            balanceData.heavyMaxDps[reportIndex] = dps;
            balanceData.heavyHitRate[reportIndex] = hitCounter / seconds;
        }
        else
        {
            balanceData.heavyNoChargeStamPerSecond[reportIndex] = stamPerSec;
            balanceData.heavyNoChargeMaxDps[reportIndex] = dps;
            balanceData.heavyNoChargeHitRate[reportIndex] = hitCounter / seconds;
        }
        Debug.Log($"Sword Heavy DPS with {playerData.strength} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        doneAttacking = true;
        yield return new WaitForSeconds(5);
        testDummy.Death();
    }

    [UnityTest]
    public IEnumerator CalculateLanternHeavyCurve()
    {
        balanceData.lanternHeavyDps.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoLanternHeavy();
        }
    }

    IEnumerator DoLanternHeavy()
    {
        testingEvents.onFaerieReturn += TestingEvents_onAttackFalse;
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2f, 0, -2f);
        testDummy.maxHealth *= 100;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        playerData.currentWeapon = 3;
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        playerAbilities.HeavyAttack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.lanternHeavyDps.AddKey(playerData.arcane, dps);
        balanceData.heavyStamPerSecond[1] = stamPerSec;
        balanceData.heavyMaxDps[1] = dps;
        balanceData.heavyHitRate[1] = hitCounter / seconds;
        Debug.Log($"Lantern Heavy DPS with {playerData.arcane} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        doneAttacking = true;
        yield return new WaitForSeconds(5);
        testDummy.Death();
    }

    [UnityTest]
    public IEnumerator CalculateKnifeHeavyCurve()
    {
        balanceData.knifeHeavyDps.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoKnifeHeavy(health[i]); ;
        }
    }

    IEnumerator DoKnifeHeavy(int health)
    {
        testingEvents.onElectricTrapDone += TestingEvents_onAttackFalse;
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2f, 0, -2f);
        testDummy.maxHealth = health;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        playerData.currentWeapon = 3;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAbilities.HeavyAttack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        ResetForNextAttack();
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.knifeHeavyDps.AddKey(playerData.strength, dps);
        balanceData.heavyStamPerSecond[2] = stamPerSec;
        balanceData.heavyMaxDps[2] = dps;
        balanceData.heavyHitRate[2] = hitCounter / seconds;
        Debug.Log($"Knife Heavy DPS with {playerData.strength} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        doneAttacking = true;
        yield return new WaitForSeconds(5);
        testDummy.Death();
    }

    [UnityTest]
    public IEnumerator CalculateClawsHeavyCurve()
    {
        balanceData.clawsHeavyDps.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoClawsHeavy(3, stats[i], BalanceWeaponType.CLAWS, BalanceAttackType.HEAVY);
        }
    }

    [UnityTest]
    public IEnumerator CalculateClawsHeavyNoChargeCurve()
    {
        balanceData.clawsHeavyDpsNoCharge.keys = new Keyframe[0];
        int[] stats = { 1, 15, 30 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoClawsHeavy(3, stats[i], BalanceWeaponType.CLAWS, BalanceAttackType.HEAVY_NO_CHARGE);
        }
    }

    IEnumerator DoClawsHeavy(int reportIndex, int stat, BalanceWeaponType type, BalanceAttackType attackType)
    {
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        if(attackType == BalanceAttackType.HEAVY)
        {
            testingEvents.onFullyCharged += (sender, e) =>
            {
                playerAttackHitEvents.chargeTimer = 1.5f;
                playerAbilities.EndHeavyAttack();
            };
        }
        else
        {
            testingEvents.onStartCharging += TestingEvents_onStartCharging;
        }
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2f, 0, -2f);
        testDummy.maxHealth *= 100;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        playerData.currentWeapon = 1;
        weaponManager.SwitchWeapon(3);
        yield return new WaitForSeconds(2);
        playerAbilities.HeavyAttack();
        if (attackType == BalanceAttackType.HEAVY_NO_CHARGE)
        {
            yield return null;
            playerAbilities.EndHeavyAttack();
        }
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        balanceData.SetDps(stat, dps, attackType, type);
        if(attackType == BalanceAttackType.HEAVY)
        {
            balanceData.heavyStamPerSecond[reportIndex] = stamPerSec;
            balanceData.heavyMaxDps[reportIndex] = dps;
            balanceData.heavyHitRate[reportIndex] = hitCounter / seconds;
        }
        else
        {
            balanceData.heavyNoChargeStamPerSecond[reportIndex] = stamPerSec;
            balanceData.heavyNoChargeMaxDps[reportIndex] = dps;
            balanceData.heavyNoChargeHitRate[reportIndex] = hitCounter / seconds;
        }
        Debug.Log($"Sword Heavy DPS with {playerData.strength} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        doneAttacking = true;
        yield return new WaitForSeconds(5);
        testDummy.Death();
    }

    private void TestingEvents_onAttackFalse(object sender, System.EventArgs e)
    {
        ResetForNextAttack();
    }

    void ResetForNextAttack()
    {
        staminaCounter += playerData.MaxStamina() - playerScript.stamina;
        playerScript.GainStamina(1000);
        healthCounter += testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(1000);
        hitCounter++;
        if (!doneAttacking)
        {
            playerAbilities.HeavyAttack();
        }
    }

    private void TestingEvents_onStartCharging(object sender, System.EventArgs e)
    {
        playerAbilities.EndHeavyAttack();
    }
}
