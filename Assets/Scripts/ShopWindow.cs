using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] GameObject firstButton;
    [SerializeField] List<ShopButton> shopButtons;
    [SerializeField] TextMeshProUGUI yourMoney;
    [SerializeField] PlayerData playerData;
    public List<string> emblemNames = new List<string>();
    public List<int> emblemCosts = new List<int>();
    [SerializeField] Color selectedColor;
    [SerializeField] TextMeshProUGUI backButtonText;
    
    Shop shop;
    PlayerControls controls;
    SoundManager sm;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => CloseShopWindow();
    }

    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>();

        for (int i = 0; i < 3; i++)
        {
            shopButtons[i].emblemName = emblemNames[i];
            shopButtons[i].cost = emblemCosts[i];
        }
    }

    private void Update()
    {
        yourMoney.text = "$" + playerData.money.ToString();
        if(EventSystem.current.currentSelectedGameObject == backButtonText.transform.parent.gameObject)
        {
            backButtonText.color = selectedColor;
        }
        else
        {
            backButtonText.color = Color.white;
        }
    }

    public void CloseShopWindow()
    {
        sm.ButtonSound();
        shop.CloseShop();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
