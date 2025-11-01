using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ChaosKnightTests
{
    PlayerData playerData;
    MapData mapData;
    GameObject knightPrefab;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        mapData = Resources.Load<MapData>("Data/MapData");
        knightPrefab = Resources.Load<GameObject>("Prefabs/Enemies/ChaosKnight");
        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator MeleeBlock()
    {
        PlayerAbilities playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        ChaosKnightController knight = GameObject.Instantiate(knightPrefab).GetComponent<ChaosKnightController>();
        knight.attackTime = 100;
        EnemyScript enemyScript = knight.GetComponent<EnemyScript>();
        knight.transform.position = new Vector3(2f, 0, 2f);
        yield return new WaitForSeconds(0.75f);

        playerAbilities.Attack();
        yield return new WaitForSeconds(2f);
        Assert.AreEqual(enemyScript.health, enemyScript.maxHealth);
    }

    [UnityTest]
    public IEnumerator LanternBlock()
    {
        PlayerAbilities playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        WeaponManager weaponManager = playerAbilities.GetComponent<WeaponManager>();
        ChaosKnightController knight = GameObject.Instantiate(knightPrefab).GetComponent<ChaosKnightController>();
        knight.attackTime = 100;
        EnemyScript enemyScript = knight.GetComponent<EnemyScript>();
        knight.transform.position = new Vector3(3f, 0, 3f);
        playerData.unlockedWeapons.Add(1);
        weaponManager.SwitchWeapon(1);
        yield return new WaitForSeconds(2f);

        playerAbilities.Attack();
        yield return new WaitForSeconds(2f);
        Assert.AreEqual(enemyScript.health, enemyScript.maxHealth);
    }

    [UnityTest]
    public IEnumerator GetShocked()
    {
        ChaosKnightController knight = GameObject.Instantiate(knightPrefab).GetComponent<ChaosKnightController>();
        knight.attackTime = 100;
        EnemyScript enemyScript = knight.GetComponent<EnemyScript>();
        knight.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        enemyScript.GainElectricCharge(10 + enemyScript.chargeResistance);
        yield return new WaitForSeconds(2f);
        Assert.Less(enemyScript.health, enemyScript.maxHealth);
    }
}
