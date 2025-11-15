using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerTrailTests
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
        playerData.unlockedWeapons.Add(1);
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
    public IEnumerator Overlap()
    {
        Time.timeScale = 1;
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerData.equippedPatches.Add(Patches.ARCANE_STEP);
        WeaponManager weaponManager = playerScript.GetComponent<WeaponManager>();
        playerData.currentWeapon = 0;
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        PatchEffects patchEffects = playerScript.gameObject.GetComponent<PatchEffects>();
        PlayerTrailManager trailManager = playerScript.gameObject.GetComponent<PlayerTrailManager>();

        playerScript.transform.position = new Vector3(2f, 0, 5f);
        PlayerAnimation playerAnimation = playerScript.GetComponent<PlayerAnimation>();
        playerMovement.rightStickValue = new Vector2(0, -1);
        playerAnimation.PlayAnimation("Combo");

        yield return new WaitForSeconds(3f);

        int pathCount = trailManager.pathTrails.Count;

        yield return new WaitForSeconds(7);

        Assert.AreEqual(0, trailManager.pathTrails.Count);

        playerScript.transform.position = Vector3.zero;

        patchEffects.ArcaneStepDodgeThrough();

        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        yield return new WaitForSeconds(0.5f);

        int dashCount = trailManager.pathTrails.Count;

        playerScript.transform.position = new Vector3(2f, 0, 5f);
        playerMovement.rightStickValue = new Vector2(0, -1);
        playerAnimation.PlayAnimation("Combo");

        yield return new WaitForSeconds(3f);

        Assert.Less(trailManager.pathTrails.Count - dashCount, pathCount);
    }

    [UnityTest]
    public IEnumerator Removed()
    {
        Time.timeScale = 1;
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        PlayerMovement playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerData.equippedPatches.Add(Patches.ARCANE_STEP);
        WeaponManager weaponManager = playerScript.GetComponent<WeaponManager>();
        playerData.currentWeapon = 0;
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2);
        PatchEffects patchEffects = playerScript.gameObject.GetComponent<PatchEffects>();
        PlayerTrailManager trailManager = playerScript.gameObject.GetComponent<PlayerTrailManager>();

        playerScript.transform.position = new Vector3(2f, 0, 5f);
        PlayerAnimation playerAnimation = playerScript.GetComponent<PlayerAnimation>();
        playerMovement.rightStickValue = new Vector2(0, -1);
        playerAnimation.PlayAnimation("Combo");

        yield return new WaitForSeconds(3f);

        int pathCount = trailManager.pathTrails.Count;

        yield return new WaitForSeconds(7);

        Assert.AreEqual(0, trailManager.pathTrails.Count);

        playerScript.transform.position = Vector3.zero;

        patchEffects.ArcaneStepDodgeThrough();

        playerScript.GainStamina(playerData.MaxStamina());

        playerMovement.moveDirection = Vector3.right;
        playerMovement.Dodge();

        yield return new WaitForSeconds(5f);

        playerScript.transform.position = new Vector3(2f, 0, 5f);
        playerMovement.rightStickValue = new Vector2(0, -1);
        playerAnimation.PlayAnimation("Combo");

        yield return new WaitForSeconds(3f);

        Assert.AreEqual(trailManager.pathTrails.Count, pathCount);
    }
}
