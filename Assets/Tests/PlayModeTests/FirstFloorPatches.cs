using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class FirstFloorPatches
{

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");

    }

    [UnityTest]
    public IEnumerator ArcaneStep()
    {
        //EditorSceneManager.OpenScene("Assets/Scenes/Testing.unity");
        EmblemLibrary emblemLibrary = Resources.Load<EmblemLibrary>("Data/EmblemLibrary");
        PlayerData playerData = Resources.Load<PlayerData>("Data/PlayerData");
        GameObject testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");

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

        yield return new WaitForSeconds(2);
        
        float dashLength = Time.fixedDeltaTime * playerMovement.dashSpeed * 0.2f;
        Vector3 targetDestination = Vector3.zero + Vector3.right * dashLength;
        float distance = Vector3.Distance(targetDestination, playerMovement.transform.position);
        Assert.LessOrEqual(distance, 0.1f);
        Assert.Less(enemyScript.health, enemyScript.maxHealth);
    }
}
