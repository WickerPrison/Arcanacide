using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] List<InputActionReference> action;
    [SerializeField] string tutorialText;
    [SerializeField] SettingsData settingsData;
    [SerializeField] Sprite[] buttonIconSprites;
    [SerializeField] TMP_SpriteAsset[] buttonIconTMProSprites;
    Dictionary<Sprite, TMP_SpriteAsset> TMPSpriteDict;
    InputManager im;
    Dictionary<string, string> displayStringDict;
    Dictionary<string, Sprite> displaySpriteDict;
    string[] textSegments;
    string finalText;


    [SerializeField] TextMeshProUGUI continueMessage;
    string KBMcontinue = "Continue: E";
    string GPcontinue = "Continue: <sprite index= 0>";
    [SerializeField] TextMeshProUGUI tutorialMessage;
    [SerializeField] string KBMtutorial;
    [SerializeField] string GPtutorial;


    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        TMPSpriteDict = new Dictionary<Sprite, TMP_SpriteAsset>();
        for(int i = 0; i < buttonIconSprites.Length; i++)
        {
            TMPSpriteDict.Add(buttonIconSprites[i], buttonIconTMProSprites[i]);
        }
    }

    private void Update()
    {
        InsertButtonPrompt();
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
        for(int i = 0; i < action[index].action.bindings.Count; i++)
        {
            if(Gamepad.current != null && action[index].action.bindings[i].groups.Contains("Gamepad"))
            {
                bindingIndex = i;
                bindingName = im.GetBindingName(action[index].action.name, bindingIndex);
            }
            else if(Gamepad.current == null && action[index].action.bindings[i].groups.Contains("Keyboard"))
            {
                bindingIndex = i;
                bindingName = im.GetBindingName(action[index].action.name, bindingIndex);
            }
            else if(Gamepad.current == null && action[index].action.bindings[i].isComposite)
            {
                bindingName = "W, A, S, D";
            }
        }

        if (displayStringDict.ContainsKey(bindingName))
        {
            if (displayStringDict[bindingName] == "")
            {
                tutorialMessage.spriteAsset = TMPSpriteDict[displaySpriteDict[bindingName]];
                return "<sprite index=0>";
            }
            else return displayStringDict[bindingName];
        }
        else return bindingName;
    }
}
