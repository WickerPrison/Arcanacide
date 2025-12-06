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
    PlayerAttackHitEvents playerAttackHitEvents;
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
        weaponManager = playerAnimation.GetComponent<WeaponManager>();
    }



    [UnityTest]
    public IEnumerator CalculateKnifeSpecialCurve()
    {
        balanceData.knifeSpecialDps.keys = new Keyframe[0];
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
        balanceData.knifeSpecialDps.AddKey(playerData.strength, dps);
        balanceData.knifeSpecialStamPerSecond = stamPerSec;
        balanceData.knifeSpecialMaxDps = dps;
        balanceData.knifeSpecialManaPerSecond = manaPerSec;
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
            playerAbilities.SpecialAttack();
        }
    }
}
