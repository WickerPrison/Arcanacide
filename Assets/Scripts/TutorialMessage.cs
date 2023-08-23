using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialMessage;
    [SerializeField] List<InputActionReference> actions;
    [SerializeField] string tutorialText;
    [SerializeField] SettingsData settingsData;
    [SerializeField] string[] gamepadText;
    [SerializeField] string[] keyboardText;
    InputManager im;
    Dictionary<string, string> displayStringDict;
    Dictionary<string, Sprite> displaySpriteDict;
    string[] textSegments;
    string finalText;


    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
    }

    private void Update()
    {
        InsertButtonPrompt();
        InsertControlSpecificString();
        tutorialMessage.text = finalText.Replace("\\n", "\n");

        /*
        if(Gamepad.current == null)
        {
            continueMessage.text = KBMcontinue;
            tutorialMessage.text = KBMtutorial.Replace("\\n", "\n");
        }
        else
        {
            continueMessage.text = GPcontinue;
            tutorialMessage.text = GPtutorial.Replace("\\n", "\n"); ;
        }
        */
    }

    void InsertButtonPrompt()
    {
        textSegments = tutorialText.Split("|");
        finalText = "";
        for(int i = 0; i < textSegments.Length; i++)
        {
            finalText += textSegments[i];
            if(i < textSegments.Length - 1)
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
        for(int i = 0; i < actions[index].action.bindings.Count; i++)
        {
            if(Gamepad.current != null && actions[index].action.bindings[i].groups.Contains("Gamepad"))
            {
                bindingIndex = i;
                bindingName = im.GetBindingName(actions[index].action.name, bindingIndex);
            }
            else if(Gamepad.current == null && actions[index].action.bindings[i].groups.Contains("Keyboard"))
            {
                bindingIndex = i;
                bindingName = im.GetBindingName(actions[index].action.name, bindingIndex);
            }
            else if(Gamepad.current == null && actions[index].action.bindings[i].isComposite)
            {
                bindingName = "W, A, S, D";
            }
        }

        if (displayStringDict.ContainsKey(bindingName))
        {
            if (displayStringDict[bindingName] == "")
            {
                int test = settingsData.buttonIconSprites.IndexOf(displaySpriteDict[bindingName]);
                return "<sprite=" + test.ToString() + ">";
            }
            else return displayStringDict[bindingName];
        }
        else return bindingName;
    }

    void InsertControlSpecificString()
    {
        textSegments = finalText.Split("*");
        finalText = "";
        for(int i = 0; i < textSegments.Length; i++)
        {
            finalText += textSegments[i];
            if(i < textSegments.Length - 1)
            {
                if(Gamepad.current == null)
                {
                    finalText += keyboardText[i];
                }
                else
                {
                    finalText += gamepadText[i];
                }
            }
        }
    }
}
