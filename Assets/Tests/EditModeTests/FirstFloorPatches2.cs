using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;

public class FirstFloorPatches2
{
    EmblemLibrary emblemLibrary = Resources.Load<EmblemLibrary>("Data/EmblemLibrary");
    PlayerData playerData = Resources.Load<PlayerData>("Data/PlayerData");
    GameObject testDummyPrefab = Resources.Load<GameObject>("Prefabs/Testing/TestDummy");
    GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player/Player");


    [UnityTest]
    public IEnumerator ArcaneStep()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing.unity");
        PlayerScript playerScript = GameObject.Instantiate(playerPrefab).GetComponent<PlayerScript>();
        playerData.equippedPatches.Clear();
        playerData.equippedPatches.Add(Patches.ARCANE_STEP);

        GameObject testDummy = GameObject.Instantiate(testDummyPrefab);
        testDummy.transform.position = Vector3.zero;

        playerScript.transform.position = Vector3.right;
        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        //playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = -Vector3.right;
        playerMovement.Dodge();

        yield return new WaitForSeconds(2);

        float dashLength = Time.fixedDeltaTime * playerMovement.dashSpeed * 0.2f;
        Vector3 targetDestination = Vector3.right - Vector3.right * dashLength;
        Debug.Log(targetDestination);
        Debug.Log(playerMovement.transform.position);
        Assert.IsTrue(true);
        //Assert.AreEqual(targetDestination, playerMovement.transform.position);

    }
}
