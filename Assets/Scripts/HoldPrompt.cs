using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class HoldPrompt : MonoBehaviour
{
    [SerializeField] string promptString;
    [SerializeField] List<InputActionReference> inputActions;
    [SerializeField] SettingsData settingsData;
    string finalString;
    TextMeshProUGUI promptText;
    InputManager im;
    ButtonPromptData buttonPromptData;

    Vector3 initialScale;
    float scaleMultiplier = 1.07f;
    float pulseSpeed = 3;
    float pulseValue;

    private void Start()
    {
        im = GlobalEvents.instance.gameObject.GetComponent<InputManager>();
        initialScale = transform.localScale;
        promptText = GetComponent<TextMeshProUGUI>();
        buttonPromptData.im = im;
        buttonPromptData.settingsData = settingsData;
        buttonPromptData.actions = inputActions;
    }

    private void Update()
    {
        finalString = promptString;
        finalString = InputUtils.InsertButtonPrompt(finalString, buttonPromptData);
        promptText.text = finalString;

        pulseValue = Mathf.Cos(Time.unscaledTime * pulseSpeed) * 0.5f + 0.5f;
        transform.localScale = Vector3.Lerp(initialScale * scaleMultiplier, initialScale, pulseValue);
    }
}
