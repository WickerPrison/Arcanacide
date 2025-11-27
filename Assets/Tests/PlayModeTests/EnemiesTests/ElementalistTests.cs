using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ElementalistTests
{
    PlayerData playerData;
    GameObject elementalistPrefab;
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
        elementalistPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Elementalist");
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator Bubbles()
    {
        ElementalistController elementalist = GameObject.Instantiate(elementalistPrefab).GetComponent<ElementalistController>();
        elementalist.transform.position = new Vector3(2.5f, 0, 2.5f);
        elementalist.state = EnemyState.IDLE;
        elementalist.attackTime = 1000;
        yield return null;
        elementalist.PlayAnimation("Bubbles");

        yield return new WaitForSeconds(4f);
    }

    [UnityTest]
    public IEnumerator ChaosHeadDeath()
    {
        ElementalistController elementalist = GameObject.Instantiate(elementalistPrefab).GetComponent<ElementalistController>();
        elementalist.transform.position = new Vector3(2.5f, 0, 2.5f);
        elementalist.state = EnemyState.IDLE;
        elementalist.attackTime = 1000;
        yield return null;
        elementalist.PlayAnimation("ChaosHead");

        yield return new WaitForSeconds(1f);
        
        EnemyScript enemyScript = elementalist.GetComponent<EnemyScript>();
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(2);
        Assert.AreEqual(playerData.MaxHealth(), playerData.health);
    }

    [UnityTest]
    public IEnumerator Death()
    {
        ElementalistController elementalist = GameObject.Instantiate(elementalistPrefab).GetComponent<ElementalistController>();
        elementalist.transform.position = new Vector3(2.5f, 0, 2.5f);
        elementalist.state = EnemyState.IDLE;
        elementalist.attackTime = 1000;
        yield return null;
        EnemyScript enemyScript = elementalist.GetComponent<EnemyScript>();
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 2);
        yield return new WaitForSeconds(2);
    }
}
