using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialMessage;
    [SerializeField] List<InputActionReference> actions;
    [SerializeField] List<InputActionReference> kbmActions;
    [SerializeField] string tutorialText;
    [SerializeField] SettingsData settingsData;
    [SerializeField] string[] gamepadText;
    [SerializeField] string[] keyboardText;
    InputManager im;
    string finalText;
    ButtonPromptData buttonPromptData;
    ButtonPromptData kbmButtonPromptData;


    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        buttonPromptData.settingsData = settingsData;
        buttonPromptData.actions = actions;
        buttonPromptData.im = im;
        kbmButtonPromptData.settingsData = settingsData;
        kbmButtonPromptData.actions = kbmActions;
        kbmButtonPromptData.im = im;
    }

    private void Update()
    {
        finalText = InputUtils.InsertButtonPrompt(tutorialText, buttonPromptData);
        keyboardText = keyboardText.Select(text => InputUtils.InsertButtonPrompt(text, kbmButtonPromptData)).ToArray();
        finalText = InputUtils.InsertControlSpecificString(finalText, keyboardText, gamepadText);
        tutorialMessage.text = finalText.Replace("\\n", "\n"); 
    }
}
