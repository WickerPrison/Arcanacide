using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class LightningThrowerTests
{
    PlayerData playerData;
    GameObject lightningthrowerPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        lightningthrowerPrefab = Resources.Load<GameObject>("Prefabs/Enemies/LightningThrower");
        Time.timeScale = Resources.Load<BuildMode>("Data/BuildMode").testTimeScale;
    }

    [UnityTest]
    public IEnumerator Death()
    {
        LightningThrower lightningthrower = GameObject.Instantiate(lightningthrowerPrefab).GetComponent<LightningThrower>();
        lightningthrower.transform.position = new Vector3(-3f, 0, -3f);
        yield return null;

        EnemyScript enemyScript = lightningthrower.GetComponent<EnemyScript>();
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 0);
        yield return new WaitForSeconds(2);
    }
}
