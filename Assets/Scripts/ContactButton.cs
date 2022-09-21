using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContactButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject textingScreenPrefab;
    [SerializeField] GameObject newIcon;
    public TextingMenu textingMenu;
    public string contactName;
    public bool newMessage;
    public int listIndex;
    Button button;
    SoundManager sm;

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        text.text = contactName;
        newIcon.SetActive(newMessage);
        SetUpMenuControls();
    }

    public void OpenTextingScreen()
    {
        sm.ButtonSound();
        TextingScreen textingScreen = Instantiate(textingScreenPrefab).GetComponent<TextingScreen>();
        textingScreen.contactName = contactName;
        textingScreen.textingMenu = textingMenu;
        textingMenu.controls.Disable();
    }

    void SetUpMenuControls()
    {
        button = GetComponent<Button>();
        Navigation nav = button.navigation;
        if (listIndex != 0)
        {
            nav.selectOnUp = textingMenu.contactButtons[listIndex - 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnUp = textingMenu.leaveButton.GetComponent<Button>();
        }

        if (listIndex < textingMenu.contactButtons.Count - 1)
        {
            nav.selectOnDown = textingMenu.contactButtons[listIndex + 1].GetComponent<Button>();
        }
        else
        {
            nav.selectOnDown = textingMenu.leaveButton.GetComponent<Button>();
        }

        button.navigation = nav;
    }
}
