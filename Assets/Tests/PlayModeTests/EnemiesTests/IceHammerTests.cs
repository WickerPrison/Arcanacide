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
    PlayerScript playerScript;
    bool playerMoveOnJump = false;

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
        Time.timeScale = 1f;
    }

    [UnityTest]
    public IEnumerator HammerSmash()
    {
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        iceHammer.HammerSmash();

        yield return new WaitForSeconds(4f);
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator HammerSmashStagger()
    {
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        EnemyScript enemyScript = iceHammer.GetComponent<EnemyScript>();
        iceHammer.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        iceHammer.HammerSmash();

        yield return new WaitForSeconds(2.2f);
        enemyScript.StartStagger(2f);
        yield return new WaitForSeconds(2f);
        Assert.AreEqual(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator LongJumpSmash()
    {
        playerMoveOnJump = false;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(15f, 0, 15f);
        yield return null;
        iceHammer.JumpSmash();
        EnemyEvents enemyEvents = iceHammer.GetComponent<EnemyEvents>();
        enemyEvents.OnTriggerVfx += EnemyEvents_OnTriggerVfx;

        yield return new WaitForSeconds(5f);
        enemyEvents.OnTriggerVfx -= EnemyEvents_OnTriggerVfx;
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator JumpSmash()
    {
        playerMoveOnJump = true;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        iceHammer.JumpSmash();
        EnemyEvents enemyEvents = iceHammer.GetComponent<EnemyEvents>();
        enemyEvents.OnTriggerVfx += EnemyEvents_OnTriggerVfx;

        yield return new WaitForSeconds(5f);
        enemyEvents.OnTriggerVfx -= EnemyEvents_OnTriggerVfx;
        Assert.Less(playerData.health, playerData.MaxHealth());
    }

    private void EnemyEvents_OnTriggerVfx(object sender, string name)
    {
        if(playerMoveOnJump && name == "JumpSmash")
        {
            RandomPlayerPosition();
        }
    }

    void RandomPlayerPosition()
    {
        playerScript.transform.position = new Vector3(
            Random.Range(-7f, 7f),
            0,
            Random.Range(-7f, 7f));
    }

    [UnityTest]
    public IEnumerator JumpStagger()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(3f, 0, 3f);
        iceHammer.attackTime = 1000;
        yield return null;
        iceHammer.JumpSmash();
        yield return new WaitForSeconds(0.83f);
        EnemyScript enemyScript = iceHammer.GetComponent<EnemyScript>();
        enemyScript.LoseHealthUnblockable(1, 1000);
        yield return new WaitForSeconds(3);
        Assert.AreEqual(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator JumpDeath()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(3f, 0, 3f);
        yield return null;
        iceHammer.JumpSmash();
        yield return new WaitForSeconds(0.5f);
        EnemyScript enemyScript = iceHammer.GetComponent<EnemyScript>();
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 0);
        yield return new WaitForSeconds(3);
        Assert.AreEqual(playerData.health, playerData.MaxHealth());
    }

    [UnityTest]
    public IEnumerator StompDeath()
    {
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(-3f, 0, 3f);
        yield return null;
        iceHammer.StartStomp();

        yield return new WaitForSeconds(0.7f);
        EnemyScript enemyScript = iceHammer.GetComponent<EnemyScript>();
        enemyScript.LoseHealthUnblockable(enemyScript.maxHealth, 0);

        yield return new WaitForSeconds(2f);
    }

    [UnityTest]
    public IEnumerator PlayerMoveStomp()
    {
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(-8f, 0, 0);
        yield return null;
        iceHammer.StartStomp();

        yield return new WaitForSeconds(1f);
        int health = playerData.health;
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(0, 0, 3f);
        yield return new WaitForSeconds(3f);
        Assert.Less(playerData.health, health);
    }

    [UnityTest]
    public IEnumerator StompEdge()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerScript.transform.position = new Vector3(-12f, 0, -12f);
        IceHammerController iceHammer = GameObject.Instantiate(iceHammerPrefab).GetComponent<IceHammerController>();
        iceHammer.transform.position = new Vector3(-10f, 0, -10f);
        yield return null;
        iceHammer.StartStomp();

        yield return new WaitForSeconds(4f);
    }
}
