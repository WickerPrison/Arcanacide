using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class HalfGolemTests
{
    PlayerData playerData;
    GameObject halfGolemPrefab;
    PlayerScript playerScript;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.vitality = 30;
        playerData.health = playerData.MaxHealth();
        halfGolemPrefab = Resources.Load<GameObject>("Prefabs/Enemies/HalfGolem");
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator DoubleAttack()
    {
        HalfGolemController halfGolem = GameObject.Instantiate(halfGolemPrefab).GetComponent<HalfGolemController>();
        halfGolem.transform.position = new Vector3(2.5f, 0, 2.5f);
        EnemyScript enemyScript = halfGolem.GetComponent<EnemyScript>();
        halfGolem.state = EnemyState.IDLE;
        halfGolem.attackTime = 1000;
        yield return null;
        for(int i = 0; i < 3; i++)
        {
            enemyScript.LoseHealthUnblockable(1, 1);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        halfGolem.DoubleAttack();

        yield return new WaitForSeconds(4f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator Jump()
    {
        HalfGolemController halfGolem = GameObject.Instantiate(halfGolemPrefab).GetComponent<HalfGolemController>();
        halfGolem.transform.position = new Vector3(-6f, 0,-6f);
        EnemyScript enemyScript = halfGolem.GetComponent<EnemyScript>();
        halfGolem.state = EnemyState.IDLE;
        halfGolem.attackTime = 1000;
        yield return null;
        for (int i = 0; i < 3; i++)
        {
            enemyScript.LoseHealthUnblockable(1, 1);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        halfGolem.JumpAttack();

        yield return new WaitForSeconds(4f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator DeathDuringStalagmites()
    {
        int initialCount = GameObject.FindObjectsOfType<StalagmiteAttack>().Length;
        HalfGolemController halfGolem = GameObject.Instantiate(halfGolemPrefab).GetComponent<HalfGolemController>();
        halfGolem.transform.position = new Vector3(7f, 0, 7f);
        EnemyScript enemyScript = halfGolem.GetComponent<EnemyScript>();
        halfGolem.state = EnemyState.IDLE;
        halfGolem.attackTime = 1000;
        yield return null;
        for (int i = 0; i < 3; i++)
        {
            enemyScript.LoseHealthUnblockable(1, 1);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        halfGolem.DoubleAttack();

        yield return new WaitForSeconds(2f);
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(4);
        int count = GameObject.FindObjectsOfType<StalagmiteAttack>().Length;
        Assert.AreEqual(initialCount, count);
    }
}
