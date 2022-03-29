using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] GameObject restMenuPrefab;
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI currentMoney;
    [SerializeField] TextMeshProUGUI requiredMoneyText;
    [SerializeField] TextMeshProUGUI strengthAmount;
    [SerializeField] TextMeshProUGUI dexterityAmount;
    [SerializeField] TextMeshProUGUI vitalityAmount;
    [SerializeField] TextMeshProUGUI dedicationAmount;
    GameObject restMenu;
    int requiredMoney;
    public int altarNumber;
    public Transform spawnPoint;

    private void Start()
    {
        UpdateText();   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenRestMenu();
        }
    }

    public void IncreaseStrength()
    {
        RequiredMoney();
        if(playerData.money >= requiredMoney)
        {
            playerData.money -= requiredMoney;
            playerData.strength += 1;
            UpdateText();
        }
    }

    public void IncreaseDexterity()
    {
        RequiredMoney();
        if (playerData.money >= requiredMoney)
        {
            playerData.money -= requiredMoney;
            playerData.dexterity += 1;
            UpdateText();
        }
    }

    public void IncreaseVitality()
    {
        RequiredMoney();
        if (playerData.money >= requiredMoney)
        {
            playerData.money -= requiredMoney;
            playerData.vitality += 1;
            UpdateText();
        }
    }

    public void IncreaseDedication()
    {
        RequiredMoney();
        if (playerData.money >= requiredMoney)
        {
            playerData.money -= requiredMoney;
            playerData.dedication += 1;
            UpdateText();
        }
    }

    public void OpenRestMenu()
    {
        restMenu = Instantiate(restMenuPrefab);
        RestMenuButtons restMenuScript = restMenu.GetComponent<RestMenuButtons>();
        restMenuScript.altarNumber = altarNumber;
        restMenuScript.spawnPoint = spawnPoint;
        Destroy(gameObject);
    }

    void UpdateText()
    {
        currentMoney.text = "Current Money: " + playerData.money.ToString();
        RequiredMoney();
        requiredMoneyText.text = "Required Money: " + requiredMoney.ToString();
        strengthAmount.text = playerData.strength.ToString();
        dexterityAmount.text = playerData.dexterity.ToString();
        vitalityAmount.text = playerData.vitality.ToString();
        dedicationAmount.text = playerData.dedication.ToString();
    }

    void RequiredMoney()
    {
        requiredMoney = Mathf.RoundToInt(4 + Mathf.Pow(playerData.GetLevel(), 2));
    }
}
