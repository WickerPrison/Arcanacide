using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CalculateSpecials
{
    PlayerData playerData;
    BalanceData balanceData;
    BalanceAttackData lightData;
    GameObject testDummyPrefab;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    WeaponManager weaponManager;
    TestingEvents testingEvents;
    PlayerScript playerScript;
    float staminaCounter;
    int healthCounter;
    float manaCounter;
    int hitCounter;
    bool doneAttacking = false;
    EnemyScript testDummy;
    EnemyScript testDummy2;
    PlayerAttackHitEvents playerAttackHitEvents;
    BalanceWeaponType currentWeaponType;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Testing");
        yield return null;
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        balanceData = Resources.Load<BalanceData>("Data/BalanceData/BalanceData");
        lightData = Resources.Load<BalanceAttackData>("Data/BalanceData/LightData");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        playerData.ClearData();
        playerData.maxMana = 500;
        playerData.mana = playerData.maxMana;
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
        testingEvents.onAttackFalse += TestingEvents_onAttackFalse;
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
    }

    [UnityTest]
    public IEnumerator CalculateSwordSpecialCurve()
    {
        currentWeaponType = BalanceWeaponType.SWORD;
        balanceData.ClearDps(BalanceAttackType.SPECIAL, BalanceWeaponType.SWORD);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            playerData.swordSpecialTimer = 0; 
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            doneAttacking = false;
            yield return DoLightCombo(BalanceWeaponType.SWORD, stats[i], health[i]);
        }
    }

    [UnityTest]
    public IEnumerator CalculateFireSwordSpecialCurve()
    {
        currentWeaponType = BalanceWeaponType.FIRESWORD;
        balanceData.ClearDps(BalanceAttackType.SPECIAL, BalanceWeaponType.FIRESWORD);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            playerData.swordSpecialTimer = 0;
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
        playerAbilities.SpecialAttack();
        yield return new WaitForSeconds(1.5f);
        playerAbilities.Attack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        switch(type)
        {
            case BalanceWeaponType.SWORD:
                dps -= lightData.swordDps.Evaluate(stat);
                break;
            case BalanceWeaponType.FIRESWORD:
                dps -= lightData.fireSwordDps.Evaluate(stat);
                break;
        }
        float stamPerSec = staminaCounter / seconds;
        balanceData.SetDps(stat, dps, BalanceAttackType.SPECIAL, type);
        balanceData.SetMaxDps(dps, reportIndex, BalanceAttackType.SPECIAL);
        Debug.Log($"{type} Special DPS with {stat} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        doneAttacking = true;
        yield return new WaitForSeconds(5);
        testDummy.Death();
        testDummy2.Death();
    }

    [UnityTest]
    public IEnumerator CalculateLanternSpecialCurve()
    {
        playerData.equippedElements[1] = WeaponElement.FIRE;
        currentWeaponType = BalanceWeaponType.LANTERN;
        balanceData.ClearDps(BalanceAttackType.SPECIAL, BalanceWeaponType.LANTERN);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            playerData.mana = playerData.maxMana;
            doneAttacking = false;
            yield return DoLanternSpecial(stats[i], health[i], 1, BalanceWeaponType.LANTERN);
        }
    }

    [UnityTest]
    public IEnumerator CalculateElectricLanternSpecialCurve()
    {
        playerData.equippedElements[1] = WeaponElement.ELECTRICITY;
        currentWeaponType = BalanceWeaponType.ELECTRICLANTERN;
        balanceData.ClearDps(BalanceAttackType.SPECIAL, BalanceWeaponType.ELECTRICLANTERN);
        int[] stats = { 1, 10, 20 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.arcane = stats[i];
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            playerData.mana = playerData.maxMana;
            doneAttacking = false;
            yield return DoLanternSpecial(stats[i], health[i], 5, BalanceWeaponType.ELECTRICLANTERN);
        }
    }

    IEnumerator DoLanternSpecial(int stat, int health, int reportIndex, BalanceWeaponType type)
    {
        testingEvents.onFaerieReturn += TestingEvents_onFaerieReturn;
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(0, 0, 0);
        testDummy.maxHealth = health;
        testDummy.health = testDummy.maxHealth;

        testDummy2 = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy2.transform.position = new Vector3(4f, 0, -2f);
        testDummy2.maxHealth = health;
        testDummy2.health = testDummy2.maxHealth;
        yield return null;
        playerData.currentWeapon = 0;
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        playerAbilities.SpecialAttack();
        float seconds = 60;
        yield return new WaitForSeconds(seconds);
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        float manaPerSec = manaCounter / seconds;
        balanceData.SetDps(stat, dps, BalanceAttackType.SPECIAL, type);
        balanceData.SetStamPerSecond(stamPerSec, reportIndex, BalanceAttackType.SPECIAL);
        balanceData.SetMaxDps(dps, reportIndex, BalanceAttackType.SPECIAL);
        balanceData.SetHitRate(hitCounter / seconds, reportIndex, BalanceAttackType.SPECIAL);
        balanceData.SetSpecialManaPerSec(manaPerSec, reportIndex);
        Debug.Log($"{type} Special DPS with {stat} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        doneAttacking = true;
        yield return new WaitForSeconds(5);
        testDummy.Death();
    }

    private void TestingEvents_onFaerieReturn(object sender, System.EventArgs e)
    {
        staminaCounter += playerData.MaxStamina() - playerScript.stamina;
        playerScript.GainStamina(1000);
        healthCounter += testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(1000);
        healthCounter += testDummy2.maxHealth - testDummy2.health;
        testDummy2.GainHealth(1000);
        hitCounter++;
        manaCounter += playerData.maxMana - playerData.mana;
        playerData.mana = playerData.maxMana;
        playerAbilities.SpecialAttack();
    }

    [UnityTest]
    public IEnumerator CalculateKnifeSpecialCurve()
    {
        currentWeaponType = BalanceWeaponType.KNIFE;
        balanceData.ClearDps(BalanceAttackType.SPECIAL, BalanceWeaponType.KNIFE);
        int[] stats = { 1, 15, 30 };
        int[] health = { 120, 250, 400 };
        for (int i = 0; i < stats.Length; i++)
        {
            playerData.strength = stats[i];
            staminaCounter = 0;
            healthCounter = 0;
            hitCounter = 0;
            playerData.mana = playerData.maxMana;
            doneAttacking = false;
            yield return DoKnifeSpecial(health[i]);
        }
    }

    IEnumerator DoKnifeSpecial(int health)
    {
        testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2f, 0, -2f);
        testDummy.maxHealth = health;
        testDummy.health = testDummy.maxHealth;
        yield return null;
        playerData.currentWeapon = 1;
        weaponManager.SwitchWeapon(2);
        yield return new WaitForSeconds(2);
        playerAbilities.SpecialAttack();
        float seconds = 60;
        for (int i = 0; i < 12; i++)
        {
            yield return new WaitForSeconds(seconds / 12);
            staminaCounter += playerData.MaxStamina() - playerScript.stamina;
            playerScript.GainStamina(1000);
            healthCounter += testDummy.maxHealth - testDummy.health;
            testDummy.GainHealth(1000);
            manaCounter += playerData.maxMana - playerData.mana;
            playerData.mana = playerData.maxMana;
        }
        float dps = healthCounter / seconds;
        float stamPerSec = staminaCounter / seconds;
        float manaPerSec = manaCounter / seconds;
        balanceData.SetDps(playerData.strength, dps, BalanceAttackType.SPECIAL, BalanceWeaponType.KNIFE);
        balanceData.SetStamPerSecond(stamPerSec, 2, BalanceAttackType.SPECIAL);
        balanceData.SetMaxDps(dps, 2, BalanceAttackType.SPECIAL);
        balanceData.SetHitRate(hitCounter / seconds, 2, BalanceAttackType.SPECIAL);
        balanceData.SetSpecialManaPerSec(manaPerSec, 2);
        Debug.Log($"Knife Special DPS with {playerData.strength} Stat: {dps}");
        Debug.Log($"Stamina Per Second: {stamPerSec}");
        playerAbilities.EndSpecialAttack();
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
            switch (currentWeaponType)
            {
                case BalanceWeaponType.KNIFE:
                    playerAbilities.SpecialAttack();
                    break;
                case BalanceWeaponType.SWORD:
                case BalanceWeaponType.FIRESWORD:
                    playerAbilities.Attack();
                    playerData.swordSpecialTimer = 100;
                    break;

            }
        }
    }
}
