using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI moneyChange;
    float updateVal;
    [System.NonSerialized] public int displayVal;
    float speed = 25;
    WaitForSeconds delay = new WaitForSeconds(1f);
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    Color visible = new Color(1, 1, 1, 1);
    Color invisible = new Color(1, 1, 1, 0);

    private void Start()
    {
        displayVal = playerData.money;
        text.text = displayVal.ToString();
    }

    private void OnPlayerMoneyChange(GlobalEvents sender, int amount)
    {
        if (amount == 0) return;
        int finalVal = playerData.money + amount;
        int difference = Mathf.Abs(displayVal - finalVal);
        float time = difference / speed;
        StopAllCoroutines();
        StartCoroutine(UpdateDisplay(time, finalVal));
        StartCoroutine(MoneyChange(amount));
    }

    IEnumerator UpdateDisplay(float time, int finalVal)
    {
        if (time > 2) time = 2;
        float timer = time;
        int initialVal = displayVal;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            displayVal = (int)Mathf.Lerp(finalVal, initialVal, timer / time);
            text.text = displayVal.ToString();
            yield return endOfFrame;
        }
        displayVal = finalVal;
        text.text = displayVal.ToString();
    }

    IEnumerator MoneyChange(int amount)
    {
        if (amount > 0)
        {
            moneyChange.text = "+" + amount.ToString();
        }
        else moneyChange.text = amount.ToString();
        moneyChange.color = visible;
        yield return delay;

        float maxTime = 0.5f;
        float timer = maxTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            moneyChange.color = Color.Lerp(invisible, visible, timer / maxTime);
            yield return endOfFrame;
        }
        moneyChange.color = invisible;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onPlayerMoneyChange += OnPlayerMoneyChange;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onPlayerMoneyChange -= OnPlayerMoneyChange;
    }
}
