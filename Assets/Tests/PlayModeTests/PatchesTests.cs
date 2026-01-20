using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PatchesTests
{
    GameObject testDummyPrefab;
    GameObject elementalistPrfab;
    GameObject remnantPrefab;
    PlayerData playerData;
    MapData mapData;
    EmblemLibrary emblemLibrary;
    PlayerScript playerScript;
    PlayerMovement playerMovement;
    PatchEffects patchEffects;
    PlayerAbilities playerAbilities;
    PlayerAnimation playerAnimation;
    PlayerHealth playerHealth;
    WeaponManager weaponManager;
    LockOn lockOn;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Testing");
        mapData = Resources.Load<MapData>("Data/MapData");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.tutorials.Clear();
        playerData.UnlockAllWeapons();
        playerData.hasHealthGem = true;
        emblemLibrary = Resources.Load<EmblemLibrary>("Data/EmblemLibrary");
        elementalistPrfab = Resources.Load<GameObject>("Prefabs/Enemies/Elementalist");
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
        remnantPrefab = Resources.Load<GameObject>("Prefabs/Player/Remnant");

        yield return null;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerMovement = playerScript.GetComponent<PlayerMovement>();
        patchEffects = playerScript.gameObject.GetComponent<PatchEffects>();
        playerAbilities = playerScript.gameObject.GetComponent<PlayerAbilities>();
        playerAnimation = playerScript.GetComponent<PlayerAnimation>();
        playerHealth = playerScript.GetComponent<PlayerHealth>();
        weaponManager = playerScript.GetComponent<WeaponManager>();
        lockOn = playerScript.GetComponent<LockOn>();
        Time.timeScale = 1;
    }

    [TearDown]
    public void Teardown()
    {
        playerData.equippedPatches.Clear();
    }


    [UnityTest]
    public IEnumerator AdrenalineRush()
    {
        playerData.equippedPatches.Add(Patches.ADRENALINE_RUSH);
        playerScript.LoseStamina(50);
        yield return new WaitForSeconds(0.2f);

        Assert.Less(playerScript.stamina, playerData.MaxStamina());
        playerScript.PerfectDodge(EnemyAttackType.MELEE);
        Assert.AreEqual(playerScript.stamina, playerData.MaxStamina());
    }

    [UnityTest]
    public IEnumerator ArcanePreservation()
    {
        playerData.equippedPatches.Add(Patches.ARCANE_PRESERVATION);

        playerScript.LoseHealth(playerData.MaxHealth() + 5, EnemyAttackType.NONPARRIABLE, null);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(1, playerData.health);
        Assert.AreEqual(playerData.maxMana - 5, playerData.mana);
    }

    [UnityTest]
    public IEnumerator ArcaneRemainsDamage()
    {
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(-1.5f, 0, -1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return null;

        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int initialDiff = testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(initialDiff);

        playerData.equippedPatches.Add(Patches.ARCANE_REMAINS);
        Remnant remnant = GameObject.Instantiate(remnantPrefab).GetComponent<Remnant>();
        remnant.transform.position = new Vector3(1, 0, 1);
        yield return null;

        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int finalDiff = testDummy.maxHealth - testDummy.health;

        Assert.Greater(finalDiff, initialDiff);
        int expectedDamage = initialDiff + Mathf.RoundToInt(initialDiff * (float)emblemLibrary.arcaneRemains.value);
        bool correctDamageDiff = TestUtils.RoughEquals(expectedDamage, finalDiff);
        Assert.True(correctDamageDiff);
    }

    [UnityTest]
    public IEnumerator ArcaneRemainsHeal()
    {
        playerData.equippedPatches.Add(Patches.ARCANE_REMAINS);
        Remnant remnant = GameObject.Instantiate(remnantPrefab).GetComponent<Remnant>();
        remnant.transform.position = new Vector3(1, 0, 1);
        yield return null;

        playerScript.LoseHealth(50, EnemyAttackType.NONPARRIABLE, null);
        yield return new WaitForSeconds(2);
        remnant.PickUpRemnant();

        yield return new WaitForSeconds(1);
        Assert.AreEqual(playerData.MaxHealth(), playerData.health);
    }

    [UnityTest]
    public IEnumerator ArcaneStepCount()
    {
        Time.timeScale = 1;
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
    public IEnumerator ArcaneStepDamage()
    {
        Time.timeScale = 1;
        playerData.equippedPatches.Add(Patches.ARCANE_STEP);
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
    public IEnumerator BurningCloak()
    {
        playerData.equippedPatches.Add(Patches.BURNING_CLOAK);
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2.5f, 0, 2.5f);
        yield return null;
        playerScript.LoseHealth(5, EnemyAttackType.NONPARRIABLE, testDummy);

        yield return new WaitForSeconds(2);
        Assert.Greater(testDummy.DOT, 0);
    }

    [UnityTest]
    public IEnumerator BurningReflection()
    {
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(2.5f, 0, 2.5f);
        yield return null;
        playerData.equippedPatches.Add(Patches.BURNING_REFLECTION);
        playerAbilities.Parry(EnemyAttackType.PROJECTILE, testDummy);

        yield return new WaitForSeconds(2);
        Assert.Greater(testDummy.DOT, 0);
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
    public IEnumerator ConfidentKiller()
    {
        playerData.equippedPatches.Add(Patches.CONFIDENT_KILLER);
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(-1.5f, 0, -1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        playerScript.LoseHealth(50, EnemyAttackType.NONPARRIABLE, null);
        yield return new WaitForSeconds(0.5f);

        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int initialDiff = testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(initialDiff);

        playerHealth.MaxHeal();
        yield return new WaitForSeconds(1f);

        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int finalDiff = testDummy.maxHealth - testDummy.health;

        Assert.Greater(finalDiff, initialDiff);
        int expectedDamage = initialDiff + Mathf.RoundToInt(initialDiff * (float)emblemLibrary.confidentKiller.value);
        bool correctDamageDiff = TestUtils.RoughEquals(expectedDamage, finalDiff);
        Assert.True(correctDamageDiff);
    }

    [UnityTest]
    public IEnumerator DeathAuraCharge()
    {
        playerData.equippedPatches.Add(Patches.DEATH_AURA);
        Remnant remnant = GameObject.Instantiate(remnantPrefab).GetComponent<Remnant>();
        remnant.transform.position = new Vector3(1, 0, 1);
        yield return new WaitForSeconds(0.5f);

        playerScript.LoseMana(playerData.maxMana);
        (float delayMod, float chargeMod) = ((float, float))emblemLibrary.deathAura.value;

        float delayTime = playerScript.maxManaDelay - playerScript.maxManaDelay * delayMod;

        float rechargeTime = playerData.maxMana / (playerScript.manaRechargeRate + playerScript.manaRechargeRate * chargeMod);

        yield return new WaitForSeconds(delayTime + rechargeTime + 0.1f);
        Assert.GreaterOrEqual(playerData.mana, playerData.maxMana);
    }

    [UnityTest]
    public IEnumerator DeathAuraDelay()
    {
        playerData.equippedPatches.Add(Patches.DEATH_AURA);
        Remnant remnant = GameObject.Instantiate(remnantPrefab).GetComponent<Remnant>();
        remnant.transform.position = new Vector3(1, 0, 1);
        yield return new WaitForSeconds(0.5f);

        playerScript.LoseMana(playerData.maxMana);
        yield return new WaitForSeconds(playerScript.maxManaDelay * 0.75f);

        Assert.Greater(playerData.mana, 0);
    }

    [UnityTest]
    public IEnumerator DeathAuraPickup()
    {
        playerData.equippedPatches.Add(Patches.DEATH_AURA);
        Remnant remnant = GameObject.Instantiate(remnantPrefab).GetComponent<Remnant>();
        remnant.transform.position = new Vector3(1, 0, 1);
        yield return new WaitForSeconds(0.5f);

        playerScript.LoseMana(playerData.maxMana);
        yield return new WaitForSeconds(0.5f);
        remnant.PickUpRemnant();
        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(playerData.maxMana, playerData.mana);
    }

    [UnityTest] 
    public IEnumerator ExplosiveHealing()
    {
        playerData.equippedPatches.Add(Patches.EXPLOSIVE_HEALING);

        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(1.5f, 0, 1.5f);
        yield return null;

        playerAnimation.HealAnimation();
        yield return new WaitForSeconds(2);
        Assert.Less(testDummy.health, testDummy.maxHealth);
    }

    [UnityTest]
    public IEnumerator HeavyBlows()
    {
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(1.5f, 0, -1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return null;

        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.poise, testDummy.maxPoise);
        float initialDiff = testDummy.maxPoise - testDummy.poise;
        testDummy.poise = testDummy.maxPoise;
       
        playerData.equippedPatches.Add(Patches.HEAVY_BLOWS);
        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.poise, testDummy.maxPoise);
        float finalDiff = testDummy.maxPoise - testDummy.poise;
        Assert.Greater(finalDiff, initialDiff);
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
        (float delayMod, float chargeMod) = ((float, float))emblemLibrary.magicalAcceleration.value;

        float delayTime = playerScript.maxManaDelay - playerScript.maxManaDelay * delayMod;

        float rechargeTime = playerData.maxMana / (playerScript.manaRechargeRate + playerScript.manaRechargeRate * chargeMod);

        yield return new WaitForSeconds(delayTime + rechargeTime + 0.1f);
        Assert.GreaterOrEqual(playerData.mana, playerData.maxMana);
    }

    [UnityTest]
    public IEnumerator MaximumRefund()
    {
        playerData.equippedPatches.Add(Patches.MAXIMUM_REFUND);
        playerScript.Rest();
        yield return null;
        Assert.AreEqual(playerData.maxHealCharges + 1, playerData.healCharges);

        PlayerAnimation playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        playerAnimation.HealAnimation();
        yield return new WaitForSeconds(3);
        Assert.AreEqual(playerData.maxHealCharges, playerData.healCharges);
    }

    [UnityTest]
    public IEnumerator MirrorCloak()
    {
        playerData.equippedPatches.Add(Patches.MIRROR_CLOAK);
        playerScript.GainStamina(playerData.MaxStamina());

        ElementalistController controller = GameObject.Instantiate(elementalistPrfab).GetComponent<ElementalistController>();
        controller.transform.position = new Vector3(7f, 0, 0);
        controller.attackTime = 1000;
        yield return null;

        controller.PlayAnimation("CastSpell");

        yield return new WaitForSeconds(0.7f);

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();
        yield return new WaitForSeconds(2);

        EnemyScript enemyScript = controller.GetComponent<EnemyScript>();
        Assert.Less(enemyScript.health, enemyScript.maxHealth);
        enemyScript.GainHealth(enemyScript.maxHealth);

        playerScript.transform.position = Vector3.zero;
        yield return new WaitForSeconds((float)emblemLibrary.mirrorCloak.value);

        controller.PlayAnimation("CastSpell");

        yield return new WaitForSeconds(0.7f);

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();
        yield return new WaitForSeconds(2);

        Assert.Less(enemyScript.health, enemyScript.maxHealth);
    }

    [UnityTest]
    public IEnumerator MirrorCloakBrokenGem()
    {
        playerData.healCharges = 0;
        playerData.equippedPatches.Add(Patches.MIRROR_CLOAK);
        playerScript.GainStamina(playerData.MaxStamina());

        ElementalistController controller = GameObject.Instantiate(elementalistPrfab).GetComponent<ElementalistController>();
        controller.transform.position = new Vector3(7f, 0, 0);
        controller.attackTime = 1000;
        yield return null;

        playerAnimation.HealAnimation();

        yield return new WaitForSeconds(3);

        controller.PlayAnimation("CastSpell");

        yield return new WaitForSeconds(0.7f);

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();
        yield return new WaitForSeconds(2);

        EnemyScript enemyScript = controller.GetComponent<EnemyScript>();
        Assert.IsFalse(playerMovement.isDashing);
    }

    [UnityTest]
    public IEnumerator OpportuneStrike()
    {
        playerData.equippedPatches.Add(Patches.OPPORTUNE_STRIKE);
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(-1.5f, 0, -1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return null;
        testDummy.dotDps = 0;

        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int initialDiff = testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(initialDiff);

        testDummy.GainDOT(9f);
        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int finalDiff = testDummy.maxHealth - testDummy.health;
        Assert.Greater(finalDiff, initialDiff);
        int expectedDamage = initialDiff + Mathf.RoundToInt(initialDiff * (float)emblemLibrary.opportuneStrike.value);
        bool correctDamageDiff = TestUtils.RoughEquals(expectedDamage, finalDiff);
        Assert.True(correctDamageDiff);
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
    public IEnumerator ProtectiveBarrier()
    {
        playerData.equippedPatches.Add(Patches.PROTECTIVE_BARRIER);
        yield return new WaitForSeconds(1f);
        playerScript.LoseHealth(50, EnemyAttackType.NONPARRIABLE, null);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(playerData.MaxHealth(), playerData.health);
        playerScript.LoseHealth(50, EnemyAttackType.NONPARRIABLE, null);
        Assert.Less(playerData.health, playerData.MaxHealth());

        yield return new WaitForSeconds((float)emblemLibrary.protectiveBarrier.value + 0.1f);
        playerScript.LoseHealth(50, EnemyAttackType.NONPARRIABLE, null);
        Assert.AreEqual(playerData.MaxHealth() - 50, playerData.health);
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
    public IEnumerator RecklessAttack()
    {
        playerData.equippedPatches.Add(Patches.RECKLESS_ATTACK);
        (float threshold, float damage) = ((float, float))emblemLibrary.recklessAttack.value;
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(-1.5f, 0, -1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return null;

        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int initialDiff = testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(initialDiff);

        playerScript.LoseHealth(Mathf.RoundToInt(playerData.MaxHealth() * (1- threshold)) + 5, EnemyAttackType.NONPARRIABLE, null);
        playerAbilities.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        int finalDiff = testDummy.maxHealth - testDummy.health;
        Assert.Greater(finalDiff, initialDiff);
        int expectedDamage = initialDiff + Mathf.RoundToInt(initialDiff * damage);
        bool correctDamageDiff = TestUtils.RoughEquals(expectedDamage, finalDiff);
        Assert.True(correctDamageDiff);
    }

    [UnityTest]
    public IEnumerator RendingBlows()
    {
        playerData.equippedPatches.Add(Patches.RENDING_BLOWS);
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(-1.5f, 0, -1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        playerData.equippedElements[1] = WeaponElement.FIRE;
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);

        playerAbilities.HeavyAttack();
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(testDummy.DOT, 0);
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

        float expected = playerData.maxMana - playerAbilities.blockManaCost * (((float dodge, float block))emblemLibrary.shellCompany.value).block;
        float difference = Mathf.Abs(expected - playerData.mana);

        Assert.Less(difference, 2);
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

        float correctStaminaVal = playerData.MaxStamina() - playerMovement.dashStaminaCost * (((float dodge, float block))emblemLibrary.shellCompany.value).dodge;

        Assert.AreEqual(correctStaminaVal, playerScript.stamina);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Spellsword()
    {
        EnemyScript testDummy = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
        testDummy.transform.position = new Vector3(1.5f, 0, 1.5f);
        yield return null;
        lockOn.ToggleLockOn();
        yield return null;

        playerAbilities.Attack();

        yield return new WaitForSeconds(3);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        Assert.AreEqual(playerData.maxMana, playerData.mana);
        int initialDiff = testDummy.maxHealth - testDummy.health;
        testDummy.GainHealth(initialDiff);

        playerData.equippedPatches.Add(Patches.SPELLSWORD);

        playerAbilities.Attack();
        yield return new WaitForSeconds(3);
        Assert.Less(testDummy.health, testDummy.maxHealth);
        Assert.Less(playerData.mana, playerData.maxMana);
        int finalDiff = testDummy.maxHealth - testDummy.health;

        Assert.Less(initialDiff, finalDiff);
    }

    [UnityTest]
    public IEnumerator StandardDeduction()
    {
        playerData.equippedPatches.Add(Patches.STANDARD_DEDUCTION);
        playerData.money = 100;
        playerScript.LoseHealth(playerData.MaxHealth() + 1, EnemyAttackType.NONPARRIABLE, null);
        yield return new WaitForSeconds(3);
        Assert.AreEqual(50, playerData.money);
        Assert.AreEqual(0, playerData.lostMoney);
        Assert.AreEqual("none", mapData.deathRoom);
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
}
