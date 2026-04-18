using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class DasherTests
{
    PlayerData playerData;
    MapData mapData;
    GameObject dasherPrefab;
    GameObject filingCabinetPrefab;


    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        mapData = Resources.Load<MapData>("Data/MapData");
        dasherPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Dasher");
        filingCabinetPrefab = Resources.Load<GameObject>("Prefabs/Layout/FilingCabinet");
        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator TeleportZap()
    {
        OldManController swordsman = GameObject.Instantiate(dasherPrefab).GetComponent<OldManController>();
        swordsman.transform.position = new Vector3(5f, 0, 5f);
        yield return null;

        swordsman.StartTeleport();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator ChargeBounce()
    {
        OldManController swordsman = GameObject.Instantiate(dasherPrefab).GetComponent<OldManController>();
        swordsman.transform.position = new Vector3(5f, 0, 5f);
        yield return null;
        GameObject filingCabinet = GameObject.Instantiate(filingCabinetPrefab);
        filingCabinet.transform.position = new Vector3(-4f, 1.2f, -4f);
        yield return null;

        swordsman.attackTime = 100f;
        swordsman.Charge();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }
}
