using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipEmblem : MonoBehaviour
{
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject check;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    public string emblemName;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = emblemName;
        descriptionText.text = emblemLibrary.GetDescription(emblemName);
        if (!playerData.equippedEmblems.Contains(emblemName))
        {
            check.SetActive(false);
        }
    }

    public void ButtonPressed()
    {
        if (playerData.equippedEmblems.Contains(emblemName))
        {
            EmblemUnequip();
        }
        else if(playerData.equippedEmblems.Count < 2)
        { 
            EmblemEquip();
        }
    }

    void EmblemEquip()
    {
        playerData.equippedEmblems.Add(emblemName);
        check.SetActive(true);
    }

    void EmblemUnequip()
    {
        playerData.equippedEmblems.Remove(emblemName);
        check.SetActive(false);
    }
}
