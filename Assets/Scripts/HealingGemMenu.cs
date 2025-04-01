using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HealingGemMenu : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI maxMana;
    [SerializeField] TextMeshProUGUI refundShards;
    [SerializeField] GameObject empowerButton;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] MenuRefundStone[] refundStones;
    [System.NonSerialized] public RestMenuButtons restMenuScript;
    PlayerControls controls;
    SoundManager sm;
    ButtonVibrate buttonVibrate;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => OpenRestMenu();
    }

    private void Start()
    {
        buttonVibrate = empowerButton.GetComponent<ButtonVibrate>();
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(empowerButton);
        maxMana.text = $"Maximum Mana: {playerData.maxMana}";
        refundShards.text = $"Refund Shards: {playerData.currentGemShards}/3";
        if(playerData.maxHealCharges >= 4)
        {
            buttonText.text = "Refund Maximized";
        }
        for(int i = 0; i < refundStones.Length; i++)
        {
            refundStones[i].SetGem(i);
        }
    }

    public void EmpowerGem()
    {
        if (playerData.currentGemShards < 3 || playerData.maxHealCharges >= 4)
        {
            buttonVibrate.StartVibrate();
            return;
        }
        playerData.currentGemShards -= 3;
        refundShards.text = $"Refund Shards: {playerData.currentGemShards}/3";
        playerData.maxHealCharges += 1;
        playerData.healCharges += 1;
        if(playerData.maxHealCharges >= 4)
        {
            buttonText.text = "Refund Maximized";
        }
        for (int i = 0; i < refundStones.Length; i++)
        {
            refundStones[i].EmpowerGem(i);
        }
    }

    public void OpenRestMenu()
    {
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restMenuScript.firstButton);
        restMenuScript.controls.Enable();
        Destroy(gameObject);
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
