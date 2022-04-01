using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] GameObject firstButton;
    Shop shop;
    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => CloseShopWindow();
    }

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>();   
    }

    public void CloseShopWindow()
    {
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
