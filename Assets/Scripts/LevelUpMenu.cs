using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI currentMoney;
    [SerializeField] TextMeshProUGUI requiredMoneyText;
    [SerializeField] TextMeshProUGUI strengthAmount;
    [SerializeField] TextMeshProUGUI dexterityAmount;
    [SerializeField] TextMeshProUGUI vitalityAmount;
    [SerializeField] TextMeshProUGUI dedicationAmount;
    [SerializeField] GameObject firstButton;
    public RestMenuButtons restMenuScript;
    int requiredMoney;
    public int altarNumber;
    public Transform spawnPoint;
    PlayerControls controls;
    SoundManager sm;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => OpenRestMenu();
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        UpdateText();   
    }

    public void IncreaseStrength()
    {
        sm.ButtonSound();
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
        sm.ButtonSound();
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
        sm.ButtonSound();
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
        sm.ButtonSound();
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
        /*
        restMenu = Instantiate(restMenuPrefab);
        RestMenuButtons restMenuScript = restMenu.GetComponent<RestMenuButtons>();
        restMenuScript.altarNumber = altarNumber;
        restMenuScript.spawnPoint = spawnPoint;
        */
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restMenuScript.firstButton);
        restMenuScript.controls.Enable();
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

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void Test()
    {
        Debug.Log("TEst");
    }
}
