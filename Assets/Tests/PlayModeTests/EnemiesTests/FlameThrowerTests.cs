using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class FlameThrowerTests
{
    PlayerData playerData;
    MapData mapData;
    GameObject flamethrowerPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        mapData = Resources.Load<MapData>("Data/MapData");
        flamethrowerPrefab = Resources.Load<GameObject>("Prefabs/Enemies/FlameThrower");
        Time.timeScale = 4;
    }


    [UnityTest]
    public IEnumerator DieWhileFlamethrowing()
    {
        Flamethrower flamethrower = GameObject.Instantiate(flamethrowerPrefab).GetComponent<Flamethrower>();
        flamethrower.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        flamethrower.Attack();

        yield return new WaitForSeconds(1.5f);
        Assert.IsTrue(flamethrower.isShooting);

        EnemyScript enemyScript = flamethrower.GetComponent<EnemyScript>();
        enemyScript.LoseHealth(enemyScript.maxHealth, 0);
        yield return new WaitForSeconds(0.2f);
        Assert.IsFalse(flamethrower.isShooting);
    }

    [UnityTest]
    public IEnumerator StaggerWhileFlamethrowing()
    {
        Flamethrower flamethrower = GameObject.Instantiate(flamethrowerPrefab).GetComponent<Flamethrower>();
        flamethrower.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        flamethrower.Attack();

        yield return new WaitForSeconds(1.5f);
        Assert.IsTrue(flamethrower.isShooting);

        EnemyScript enemyScript = flamethrower.GetComponent<EnemyScript>();
        enemyScript.LoseHealth(0, 1000);
        yield return new WaitForSeconds(0.2f);
        Assert.IsFalse(flamethrower.isShooting);
    }
}
