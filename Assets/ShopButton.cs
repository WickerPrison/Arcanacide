using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string itemName;
    int cost = 100;

    public void ShopButtonPressed()
    {
        if (playerData.money >= cost)
        {
            switch (itemName)
            {
                case "Damage":
                    if (!mapData.boughtDamage)
                    {
                        playerData.money -= cost;
                        playerData.attackPower += 5;
                        mapData.boughtDamage = true;
                        UpdateText();
                    }
                    break;
                case "Health":
                    if (!mapData.boughtHealth)
                    {
                        playerData.money -= cost;
                        playerData.maxHealth += 20;
                        playerData.health += 20;
                        mapData.boughtHealth = true;
                        UpdateText();
                    }
                    break;
                case "Stamina":
                    if (!mapData.boughtStamina)
                    {
                        playerData.money -= cost;
                        playerData.maxStamina += 20;
                        mapData.boughtStamina = true;
                        UpdateText();
                    }
                    break;
            }
        }
    }

    private void Start()
    {
        UpdateText();
    }


    void UpdateText()
    {
        switch (itemName)
        {
            case "Damage":
                if (mapData.boughtDamage)
                {
                    text.text = "Bought";
                }
                else
                {
                    text.text = "Buy";
                }
                break;
            case "Health":
                if (mapData.boughtHealth)
                {
                    text.text = "Bought";
                }
                else
                {
                    text.text = "Buy";
                }
                break;
            case "Stamina":
                if (mapData.boughtStamina)
                {
                    text.text = "Bought";
                }
                else
                {
                    text.text = "Buy";
                }
                break;
        }
    }
}
