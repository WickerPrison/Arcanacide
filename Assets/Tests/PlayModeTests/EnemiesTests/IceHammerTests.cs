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
    GameObject iciclePrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.vitality = 30;
        playerData.health = playerData.MaxHealth();
        mapData = Resources.Load<MapData>("Data/MapData");
        iceHammerPrefab = Resources.Load<GameObject>("Prefabs/Enemies/IceHammer");
        iciclePrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttacks/StalagmiteAttack");
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

    [UnityTest]
    public IEnumerator Icicle()
    {
        StalagmiteAttack stalagmiteAttack = GameObject.Instantiate(iciclePrefab).GetComponent<StalagmiteAttack>();
        stalagmiteAttack.transform.position = new Vector3(3f, 0, 3f);

        yield return new WaitForSeconds(3f);
    }

    [UnityTest]
    public IEnumerator Stomp()
    {
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(-3f, 0, 3f);
        yield return null;
        iceHammer.StartStomp();

        yield return new WaitForSeconds(7f);

        iceHammer.transform.position = new Vector3(-3f, 0, -3f);
        yield return null;
        iceHammer.StartStomp();

        yield return new WaitForSeconds(7f);
    }
}
