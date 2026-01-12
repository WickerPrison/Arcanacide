using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MoneyGainTest
{
    PlayerData playerData;
    MoneyCounter moneyCounter;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Testing");
        playerData = Resources.Load<PlayerData>("Data/PlayerData");
        playerData.ClearData();
        playerData.hasHealthGem = true;
        playerData.money = 0;

        Time.timeScale = 1;

        yield return null;
        moneyCounter = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<MoneyCounter>();
    }

    [UnityTest]
    public IEnumerator GainZero()
    {
        int amount = 0;
        GlobalEvents.instance.MoneyChange(amount);
        yield return new WaitForSeconds(2.1f);
        Assert.AreEqual(moneyCounter.displayVal, amount);
    }

    [UnityTest]
    public IEnumerator SmallValue()
    {
        int amount = 27;
        GlobalEvents.instance.MoneyChange(amount);
        yield return new WaitForSeconds(2.1f);
        Assert.AreEqual(moneyCounter.displayVal, amount);
    }

    [UnityTest]
    public IEnumerator MediumValue()
    {
        int amount = 500;
        GlobalEvents.instance.MoneyChange(amount);
        yield return new WaitForSeconds(2.1f);
        Assert.AreEqual(moneyCounter.displayVal, amount);
    }

    [UnityTest]
    public IEnumerator LargeValue()
    {
        int amount = 3000;
        GlobalEvents.instance.MoneyChange(amount);
        yield return new WaitForSeconds(2.1f);
        Assert.AreEqual(moneyCounter.displayVal, amount);
    }

    [UnityTest]
    public IEnumerator HugeValue()
    {
        int amount = 50000;
        GlobalEvents.instance.MoneyChange(amount);
        yield return new WaitForSeconds(2.1f);
        Assert.AreEqual(moneyCounter.displayVal, amount);
    }

    [UnityTest]
    public IEnumerator RapidChanges()
    {
        int amount0 = 15;
        int amount1 = 50000;
        int amount2 = -507;
        int amount3 = 234;
        GlobalEvents.instance.MoneyChange(amount0);
        yield return new WaitForSeconds(1f);
        GlobalEvents.instance.MoneyChange(amount1);
        yield return new WaitForSeconds(0.5f);
        GlobalEvents.instance.MoneyChange(amount2);
        yield return new WaitForSeconds(1.1f);
        GlobalEvents.instance.MoneyChange(amount3);
        yield return new WaitForSeconds(2.1f);
        Assert.AreEqual(moneyCounter.displayVal, amount0 + amount1 + amount2 + amount3);
    }

    [UnityTest]
    public IEnumerator RapidSpending()
    {
        GlobalEvents.instance.MoneyChange(1000);
        yield return new WaitForSeconds(2.1f);
        int amount0 = -15;
        int amount1 = -60;
        int amount2 = -507;
        int amount3 = -234;
        GlobalEvents.instance.MoneyChange(amount0);
        yield return null;
        GlobalEvents.instance.MoneyChange(amount1);
        yield return null;
        GlobalEvents.instance.MoneyChange(amount2);
        yield return null;
        GlobalEvents.instance.MoneyChange(amount3);
        yield return new WaitForSeconds(2.1f);
        Assert.AreEqual(moneyCounter.displayVal, 1000 + amount0 + amount1 + amount2 + amount3);
    }
}
