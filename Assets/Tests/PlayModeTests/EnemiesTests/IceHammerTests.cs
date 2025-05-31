using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class IceHammerTests
{
    PlayerData playerData;
    MapData mapData;
    GameObject iceHammerPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        mapData = Resources.Load<MapData>("Data/MapData");
        iceHammerPrefab = Resources.Load<GameObject>("Prefabs/Enemies/IceHammer");
        Time.timeScale = 1;
    }


    [UnityTest]
    public IEnumerator JumpSmash()
    {
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        iceHammer.JumpSmash();

        yield return new WaitForSeconds(5f);
    }
}
