using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct ButtonPromptData
{
    public SettingsData settingsData;
    public List<InputActionReference> actions;
    public InputManager im;
}

public static class InputUtils
{
    public static string InsertButtonPrompt(string inputString, ButtonPromptData data)
    {
        string[] textSegments = inputString.Split("|");
        string finalText = "";
        for (int i = 0; i < textSegments.Length; i++)
        {
            finalText += textSegments[i];
            if (i < textSegments.Length - 1)
            {
                finalText += GetButtonPrompt(i, data);
            }
        }

        return finalText;
    }

    static string GetButtonPrompt(int index, ButtonPromptData data)
    {
        Dictionary<string, string> displayStringDict = data.settingsData.GetStringDictionary();
        Dictionary<string, Sprite> displaySpriteDict = data.settingsData.GetSpriteDictionary();

        int bindingIndex;
        string bindingName = "";
        for (int i = 0; i < data.actions[index].action.bindings.Count; i++)
        {
            if (Gamepad.current != null && data.actions[index].action.bindings[i].groups.Contains("Gamepad"))
            {
                bindingIndex = i;
                bindingName = data.im.GetBindingName(data.actions[index].action.name, bindingIndex);
            }
            else if (Gamepad.current == null && data.actions[index].action.bindings[i].groups.Contains("Keyboard"))
            {
                bindingIndex = i;
                bindingName = data.im.GetBindingName(data.actions[index].action.name, bindingIndex);
            }
            else if (Gamepad.current == null && data.actions[index].action.bindings[i].isComposite)
            {
                bindingName = "W, A, S, D";
            }
        }

        bindingName = bindingName.Replace("Hold or Tap ", "");

        if (displayStringDict.ContainsKey(bindingName))
        {
            if (displayStringDict[bindingName] == "")
            {
                int test = data.settingsData.buttonIconSprites.IndexOf(displaySpriteDict[bindingName]);
                return "<sprite=" + test.ToString() + ">";
            }
            else return displayStringDict[bindingName];
        }
        else return bindingName;
    }

    public static string InsertControlSpecificString(string inputString, string[] keyboardText, string[] gamepadText)
    {
        string[] textSegments = inputString.Split("*");
        string finalText = "";
        for (int i = 0; i < textSegments.Length; i++)
        {
            finalText += textSegments[i];
            if (i < textSegments.Length - 1)
            {
                if (Gamepad.current == null)
                {
                    finalText += keyboardText[i];
                }
                else
                {
                    finalText += gamepadText[i];
                }
            }
        }
        return finalText;
    }
}

