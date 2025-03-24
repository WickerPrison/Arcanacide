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
        emblemLibrary = Resources.Load<EmblemLibrary>("Data/EmblemLibrary");
        
        testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
    }

    [TearDown]
    public void Teardown()
    {
        playerData.equippedPatches.Clear();
    }

    [UnityTest]
    public IEnumerator ArcaneStep()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Clear();
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
    public IEnumerator Quickstep()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Clear();
        playerData.equippedPatches.Add(Patches.QUICKSTEP);

        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        float correctStaminaVal = playerData.MaxStamina() - playerMovement.dashStaminaCost / 2f;

        Assert.AreEqual(correctStaminaVal, playerScript.stamina);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PayRaise()
    {
        playerData.equippedPatches.Clear();
        playerData.equippedPatches.Add(Patches.PAY_RAISE);

        int[] costs = { 5, 17, 35, 0 };

        foreach(int cost in costs)
        {
            EnemyScript enemyScript = GameObject.Instantiate(testDummyPrefab).GetComponent<EnemyScript>();
            yield return null;
            enemyScript.reward = cost;
            enemyScript.LoseHealth(enemyScript.maxHealth, 0);

            playerData.money = 0;

            yield return new WaitForSeconds(0.2f);
            int expected = Mathf.RoundToInt(enemyScript.reward * emblemLibrary.patchDictionary[Patches.PAY_RAISE].value);

            Assert.AreEqual(expected, playerData.money);
        }
    }

    [UnityTest]
    public IEnumerator ShellCompanyStamina()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Clear();
        playerData.equippedPatches.Add(Patches.SHELL_COMPANY);

        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        float correctStaminaVal = playerData.MaxStamina() - playerMovement.dashStaminaCost * 2f;

        Assert.AreEqual(correctStaminaVal, playerScript.stamina);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShellCompanyMana()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData.equippedPatches.Clear();
        playerData.equippedPatches.Add(Patches.SHELL_COMPANY);

        playerData.mana = playerData.maxMana;

        PlayerAbilities playerAbilities = playerScript.GetComponent<PlayerAbilities>();
        playerAbilities.Shield();

        yield return new WaitForSeconds(1);

        float expected = playerData.maxMana - playerAbilities.blockManaCost / 2;
        float difference = Mathf.Abs(expected - playerData.mana);

        Assert.Less(difference, 2);
    }
}
