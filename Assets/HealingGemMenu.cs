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
    [SerializeField] MenuRefundStone[] refundStones;
    [System.NonSerialized] public RestMenuButtons restMenuScript;
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
        EventSystem.current.SetSelectedGameObject(empowerButton);
        maxMana.text = $"Maximum Mana: {playerData.maxMana}";
        refundShards.text = $"Refund Shards: {playerData.currentGemShards}/3";
        for(int i = 0; i < refundStones.Length; i++)
        {
            refundStones[i].SetGem(i);
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
}
