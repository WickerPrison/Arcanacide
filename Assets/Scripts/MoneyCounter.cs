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
    int displayVal;
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

    private void Update()
    {
        if (displayVal == playerData.money) return;

        if(displayVal < playerData.money)
        {
            updateVal += Time.deltaTime * speed;
            if(updateVal >= 1)
            {
                displayVal += Mathf.FloorToInt(updateVal);
                updateVal = 0;
                if(displayVal > playerData.money)
                {
                    displayVal = playerData.money;
                }
            }
        }
        else if (displayVal > playerData.money)
        {
            updateVal += Time.deltaTime * speed;
            if (updateVal >= 1)
            {
                displayVal -= Mathf.FloorToInt(updateVal);
                updateVal = 0;
                if (displayVal < playerData.money)
                {
                    displayVal = playerData.money;
                }
            }
        }

        text.text = displayVal.ToString();
    }

    private void OnPlayerMoneyChange(GlobalEvents sender, int amount)
    {
        int difference = Mathf.Abs(displayVal - (playerData.money + amount));
        if (difference > 50)
        {
            speed = difference / 2;
        }
        else speed = 25;
        StopAllCoroutines();
        StartCoroutine(MoneyChange(amount));
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
