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
    string finalText;
    ButtonPromptData buttonPromptData;


    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        buttonPromptData.settingsData = settingsData;
        buttonPromptData.actions = actions;
        buttonPromptData.im = im;
    }

    private void Update()
    {
        finalText = InputUtils.InsertButtonPrompt(tutorialText, buttonPromptData);
        finalText = InputUtils.InsertControlSpecificString(finalText, keyboardText, gamepadText);
        tutorialMessage.text = finalText.Replace("\\n", "\n"); 
    }
}
