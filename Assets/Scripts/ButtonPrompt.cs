using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ButtonPrompt : MonoBehaviour
{
    public string prompt;
    [SerializeField] List<InputActionReference> actions;
    [SerializeField] SettingsData settingsData;
    Dictionary<Sprite, TMP_SpriteAsset> TMPSpriteDict;
    InputManager im;
    TextMeshProUGUI text;
    Dictionary<string, string> displayStringDict;
    Dictionary<string, Sprite> displaySpriteDict;
    string[] textSegments;
    string finalText;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        TMPSpriteDict = new Dictionary<Sprite, TMP_SpriteAsset>();
        for (int i = 0; i < settingsData.buttonIconSprites.Count; i++)
        {
            TMPSpriteDict.Add(settingsData.buttonIconSprites[i], settingsData.buttonIconTMProSprites[i]);
        }
    }

    void Update()
    {
        InsertButtonPrompt();
        text.text = finalText.Replace("\\n", "\n");
    }


    void InsertButtonPrompt()
    {
        textSegments = prompt.Split("|");
        finalText = "";
        for (int i = 0; i < textSegments.Length; i++)
        {
            finalText += textSegments[i];
            if (i < textSegments.Length - 1)
            {
                finalText += GetButtonPrompt(i);
            }
        }
    }

    string GetButtonPrompt(int index)
    {
        displayStringDict = settingsData.GetStringDictionary();
        displaySpriteDict = settingsData.GetSpriteDictionary();

        int bindingIndex;
        string bindingName = "";
        for (int i = 0; i < actions[index].action.bindings.Count; i++)
        {
            if (Gamepad.current != null && actions[index].action.bindings[i].groups.Contains("Gamepad"))
            {
                bindingIndex = i;
                bindingName = im.GetBindingName(actions[index].action.name, bindingIndex);
            }
            else if (Gamepad.current == null && actions[index].action.bindings[i].groups.Contains("Keyboard"))
            {
                bindingIndex = i;
                bindingName = im.GetBindingName(actions[index].action.name, bindingIndex);
            }
            else if (Gamepad.current == null && actions[index].action.bindings[i].isComposite)
            {
                bindingName = "W, A, S, D";
            }
        }

        if (displayStringDict.ContainsKey(bindingName))
        {
            if (displayStringDict[bindingName] == "")
            {
                text.spriteAsset = TMPSpriteDict[displaySpriteDict[bindingName]];
                return "<sprite index=0>";
            }
            else return displayStringDict[bindingName];
        }
        else return bindingName;
    }
}
