using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipEmblem : MonoBehaviour
{
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject check;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    public EmblemMenu emblemMenu;
    Button button;
    public string emblemName;
    SoundManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        SetUpMenuControls();
        nameText.text = emblemName;
        descriptionText.text = emblemLibrary.GetDescription(emblemName);
        if (!playerData.equippedEmblems.Contains(emblemName))
        {
            check.SetActive(false);
        }
    }

    public void ButtonPressed()
    {
        sm.ButtonSound();
        if (playerData.equippedEmblems.Contains(emblemName))
        {
            EmblemUnequip();
        }
        else if(playerData.equippedEmblems.Count < playerData.maxPatches)
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

    void SetUpMenuControls()
    {
        button = GetComponent<Button>();
        int listIndex = playerData.emblems.IndexOf(emblemName);
        Navigation nav = button.navigation;
        if (listIndex != 0)
        {
            nav.selectOnUp = emblemMenu.buttons[listIndex - 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnUp = emblemMenu.leaveButton.GetComponent<Button>();
        }

        if(listIndex < emblemMenu.buttons.Count - 1)
        {
            nav.selectOnDown = emblemMenu.buttons[listIndex + 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnDown = emblemMenu.leaveButton.GetComponent<Button>();
        }

        button.navigation = nav;
    }
}
