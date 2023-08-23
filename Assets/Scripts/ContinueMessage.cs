using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ContinueMessage : MonoBehaviour
{
    [SerializeField] InputActionReference inputAction;
    [SerializeField] SettingsData settingsData;
    [SerializeField] Sprite[] buttonIconSprites;
    [SerializeField] TMP_SpriteAsset[] buttonIconTMProSprites;
    Dictionary<Sprite, TMP_SpriteAsset> TMPSpriteDict;
    Dictionary<string, string> displayStringDict;
    string bindingName;
    int bindingIndex = 0;
    string buttonPromptString;
    TextMeshProUGUI continueMessage;
    InputManager im;

    Vector3 initialScale;
    float scaleMultiplier = 1.07f;
    float pulseSpeed = 3;
    float pulseValue;

    private void Start()
    {
        im = GlobalEvents.instance.gameObject.GetComponent<InputManager>();
        continueMessage = gameObject.GetComponent<TextMeshProUGUI>();
        initialScale = transform.localScale;
        TMPSpriteDict = new Dictionary<Sprite, TMP_SpriteAsset>();
        for (int i = 0; i < buttonIconSprites.Length; i++)
        {
            TMPSpriteDict.Add(buttonIconSprites[i], buttonIconTMProSprites[i]);
        }
    }

    private void Update()
    {
        UpdateText();

        pulseValue = Mathf.Cos(Time.unscaledTime * pulseSpeed) * 0.5f + 0.5f;
        transform.localScale = Vector3.Lerp(initialScale * scaleMultiplier, initialScale, pulseValue);
    }

    void UpdateText()
    {
        if (Gamepad.current == null) bindingIndex = 0;
        else bindingIndex = 1;

        bindingName = im.GetBindingName(inputAction.action.name, bindingIndex);
        displayStringDict = settingsData.GetStringDictionary();
        if (displayStringDict.ContainsKey(bindingName))
        {
            if (displayStringDict[bindingName] == "")
            {
                continueMessage.spriteAsset = TMPSpriteDict[settingsData.GetSpriteDictionary()[bindingName]];
                buttonPromptString = "<sprite index=0>";
            }
            else buttonPromptString = displayStringDict[bindingName];
        }
        else buttonPromptString = bindingName;

        continueMessage.text = "Continue: " + buttonPromptString;
    }
}
