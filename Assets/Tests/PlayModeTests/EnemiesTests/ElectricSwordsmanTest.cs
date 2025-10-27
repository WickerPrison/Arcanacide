using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ElectricSwordsmanTest
{
    PlayerData playerData;
    MapData mapData;
    GameObject swordsmanPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        mapData = Resources.Load<MapData>("Data/MapData");
        swordsmanPrefab = Resources.Load<GameObject>("Prefabs/Enemies/ElectricSwordsman");
        Time.timeScale = 4;
    }

    [UnityTest]
    public IEnumerator RangedSlashHitsPlayer()
    {
        ElectricSwordsmanController swordsman = GameObject.Instantiate(swordsmanPrefab).GetComponent<ElectricSwordsmanController>();
        swordsman.transform.position = new Vector3(5f, 0, 5f);
        yield return null;

        swordsman.Attack();
        yield return new WaitForSeconds(1.5f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator RingHitsPlayer()
    {
        ElectricSwordsmanController swordsman = GameObject.Instantiate(swordsmanPrefab).GetComponent<ElectricSwordsmanController>();
        swordsman.transform.position = new Vector3(5f, 0, 5f);
        yield return null;

        swordsman.StartRings();
        yield return new WaitForSeconds(6);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator MulitpleRings()
    {
        ElectricSwordsmanController swordsman = GameObject.Instantiate(swordsmanPrefab).GetComponent<ElectricSwordsmanController>();
        swordsman.transform.position = new Vector3(5f, 0, 5f);
        yield return null;

        swordsman.StartRings();
        yield return new WaitForSeconds(6);
        Assert.Less(playerData.health, playerData.MaxHealth());
        float health = playerData.health;
        swordsman.StartRings();
        yield return new WaitForSeconds(6);
        Assert.Less(playerData.health, health);
    }

    [UnityTest]
    public IEnumerator RingInterruptedByStagger()
    {
        ElectricSwordsmanController swordsman = GameObject.Instantiate(swordsmanPrefab).GetComponent<ElectricSwordsmanController>();
        swordsman.transform.position = new Vector3(5f, 0, 5f);
        yield return null;

        swordsman.StartRings();
        yield return new WaitForSeconds(3);
        EnemyScript enemyScript = swordsman.GetComponent<EnemyScript>();
        enemyScript.StartStagger(5f);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(LightningRingsState.DISABLED, swordsman.rings.state);    
    }

    [UnityTest]
    public IEnumerator RingInterruptedByDeath()
    {
        ElectricSwordsmanController swordsman = GameObject.Instantiate(swordsmanPrefab).GetComponent<ElectricSwordsmanController>();
        swordsman.transform.position = new Vector3(5f, 0, 5f);
        yield return null;

        swordsman.StartRings();
        yield return new WaitForSeconds(3);
        EnemyScript enemyScript = swordsman.GetComponent<EnemyScript>();
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 0);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(LightningRingsState.DISABLED, swordsman.rings.state);
    }
}
