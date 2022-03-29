using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI emblemNameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] string emblemName;
    [SerializeField] int cost = 100;

    public void ShopButtonPressed()
    {
        if (!playerData.emblems.Contains(emblemName) && playerData.money >= cost)
        {
            playerData.emblems.Add(emblemName);
            playerData.money -= cost;
            UpdateText();
        }
    }

    private void Start()
    {
        UpdateText();
    }


    void UpdateText()
    {
        if (playerData.emblems.Contains(emblemName))
        {
            text.text = "Sold Out";
        }
        else
        {
            text.text = "Buy";
        }

        costText.text = cost.ToString();
        emblemNameText.text = emblemName;
        descriptionText.text = emblemLibrary.GetDescription(emblemName);
    }
}
