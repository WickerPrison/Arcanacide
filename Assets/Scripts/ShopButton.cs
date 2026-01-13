using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI emblemNameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    public Patches patchName;
    Patch patch;
    public int cost = 100;
    SoundManager sm;

    public void ShopButtonPressed()
    {
        sm.ButtonSound();
        if (!playerData.patches.Contains(patchName) && playerData.money >= cost)
        {
            playerData.patches.Add(patchName);
            GlobalEvents.instance.MoneyChange(playerData.money, -cost);
        }
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        patch = emblemLibrary.patchDictionary[patchName];
        StartCoroutine(SetEmblemName());
    }

    IEnumerator SetEmblemName()
    {
        yield return new WaitForEndOfFrame();
        emblemNameText.text = patch.name;   
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == gameObject)
        {
            emblemNameText.color = mapData.floorColor;
            descriptionText.text = patch.description;
            if (playerData.patches.Contains(patchName))
            {
                costText.text = "Sold Out";
            }
            else
            {
                costText.text = "$" + cost.ToString();
            }
        }
        else
        {
            emblemNameText.color = Color.white;
        }
    }
}
