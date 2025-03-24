using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class FirstFloorPatches
{
    GameObject testDummyPrefab;
    PlayerData playerData;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
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

        Assert.AreEqual(playerScript.stamina, correctStaminaVal);

        yield return null;
    }
}
