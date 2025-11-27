using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class FirstFloorPatches
{
    GameObject testDummyPrefab;
    PlayerData playerData;
    EmblemLibrary emblemLibrary;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        emblemLibrary = Resources.Load<EmblemLibrary>("Data/EmblemLibrary");
        
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        Time.timeScale = 1;
    }

    [TearDown]
    public void Teardown()
    {
        playerData.equippedPatches.Clear();
    }

    [UnityTest]
    public IEnumerator ArcaneStepDamage()
    {
        Time.timeScale = 1;
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Add(Patches.ARCANE_STEP);
        PatchEffects patchEffects = playerScript.gameObject.GetComponent<PatchEffects>();
        patchEffects.ArcaneStepDodgeThrough();

        GameObject testDummy = GameObject.Instantiate(testDummyPrefab);
        testDummy.transform.position = Vector3.right * 2f;
        EnemyScript enemyScript = testDummy.GetComponent<EnemyScript>();

        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        yield return new WaitForSeconds(1);
        
        float dashLength = Time.fixedDeltaTime * playerMovement.dashSpeed * 0.2f;
        Vector3 targetDestination = Vector3.zero + Vector3.right * dashLength;
        float distance = Vector3.Distance(targetDestination, playerMovement.transform.position);
        Assert.LessOrEqual(distance, 0.1f);
        Assert.Less(enemyScript.health, enemyScript.maxHealth);
    }

    [UnityTest]
    public IEnumerator ArcaneStepCount()
    {
        Time.timeScale = 1;
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Add(Patches.ARCANE_STEP);
        PatchEffects patchEffects = playerScript.gameObject.GetComponent<PatchEffects>();
        PlayerTrailManager trailManager = playerScript.gameObject.GetComponent<PlayerTrailManager>();
        patchEffects.ArcaneStepDodgeThrough();

        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(5, trailManager.pathTrails.Count);

        playerMovement.moveDirection = -Vector3.right;
        playerMovement.Dodge();

        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(6, trailManager.pathTrails.Count);
    }

    [UnityTest]
    public IEnumerator Quickstep()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Add(Patches.QUICKSTEP);

        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        float correctStaminaVal = playerData.MaxStamina() - playerMovement.dashStaminaCost * (float)emblemLibrary.quickstep.value;

        Assert.AreEqual(correctStaminaVal, playerScript.stamina);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PayRaise()
    {
        playerData.equippedPatches.Add(Patches.PAY_RAISE);

        int[] costs = { 5, 17, 35, 0 };

        foreach(int cost in costs)
        {
            EnemyScript enemyScript = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
            yield return null;
            enemyScript.reward = cost;
            enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 0);

            playerData.money = 0;

            yield return new WaitForSeconds(0.2f);
            int expected = Mathf.RoundToInt(enemyScript.reward * (float)emblemLibrary.patchDictionary[Patches.PAY_RAISE].value);

            Assert.AreEqual(expected, playerData.money);
        }
    }

    [UnityTest]
    public IEnumerator ShellCompanyStamina()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Add(Patches.SHELL_COMPANY);

        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        float correctStaminaVal = playerData.MaxStamina() - playerMovement.dashStaminaCost * (((float, float))emblemLibrary.shellCompany.value).Item1;

        Assert.AreEqual(correctStaminaVal, playerScript.stamina);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShellCompanyMana()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Add(Patches.SHELL_COMPANY);
        playerData.unlockedAbilities.Add(UnlockableAbilities.BLOCK);

        playerData.mana = playerData.maxMana;

        PlayerAbilities playerAbilities = playerScript.GetComponent<PlayerAbilities>();
        playerAbilities.Shield();

        yield return new WaitForSeconds(1);

        float expected = playerData.maxMana - playerAbilities.blockManaCost * (((float, float))emblemLibrary.shellCompany.value).Item2;
        float difference = Mathf.Abs(expected - playerData.mana);

        Assert.Less(difference, 2);
    }

    [UnityTest]
    public IEnumerator CloseCall()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        WeaponManager weaponManager = playerScript.gameObject.GetComponent<WeaponManager>();
        playerData.equippedPatches.Add(Patches.CLOSE_CALL);

        playerScript.PerfectDodge(EnemyAttackType.MELEE);
        Assert.AreEqual(1, weaponManager.weaponMagicSources);
        yield return new WaitForSeconds(2f);
        playerScript.PerfectDodge(EnemyAttackType.MELEE);
        Assert.AreEqual(1, weaponManager.weaponMagicSources);
    }

    [UnityTest]
    public IEnumerator VampiricStrikes()
    {
        playerData.equippedPatches.Add(Patches.VAMPIRIC_STRIKES);

        EnemyScript enemyScript = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        yield return null;

        int startingHealth = Mathf.FloorToInt(playerData.MaxHealth() * 0.5f);
        playerData.health = startingHealth;
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 0);

        yield return new WaitForSeconds(0.2f);
        int healAmount = Mathf.FloorToInt(playerData.MaxHealth() * (float)emblemLibrary.patchDictionary[Patches.VAMPIRIC_STRIKES].value);
        int expected = startingHealth + healAmount;

        Assert.AreEqual(expected, playerData.health);

    }

    [UnityTest]
    public IEnumerator MagicalAccelerationDelay()
    {
        playerData.equippedPatches.Add(Patches.MAGICAL_ACCELERATION);

        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        playerScript.LoseMana(playerData.maxMana);
        yield return new WaitForSeconds(playerScript.maxManaDelay * 0.75f);
        Assert.Greater(playerData.mana, 0);
    }

    [UnityTest]
    public IEnumerator MagicalAccelerationRate()
    {
        playerData.equippedPatches.Add(Patches.MAGICAL_ACCELERATION);

        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        playerScript.LoseMana(playerData.maxMana);

        float magicallAccelerationValue = (float)emblemLibrary.patchDictionary[Patches.MAGICAL_ACCELERATION].value;

        float delayTime = playerScript.maxManaDelay / magicallAccelerationValue;

        float rechargeTime = playerData.maxMana / (playerScript.manaRechargeRate * magicallAccelerationValue);

        yield return new WaitForSeconds(delayTime + rechargeTime + 0.1f);
        Assert.GreaterOrEqual(playerData.mana, playerData.maxMana);
    }
}
