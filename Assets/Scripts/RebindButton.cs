using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class RebindButton : MonoBehaviour
{
    [SerializeField] bool isGamepad;
    [SerializeField] InputActionReference inputActionReference;
    string actionName;
    [Range(0,10)] [SerializeField] int selectedBinding;
    [SerializeField] InputBinding.DisplayStringOptions displayStringOptions;
    [Header("Binding Infor - DO NOT EDIT")]
    [SerializeField] InputBinding inputBinding;
    int bindingIndex;
    [SerializeField] TextMeshProUGUI rebindText;
    [SerializeField] Image rebindSprite;
    [SerializeField] SettingsData settingsData;
    RebindControlsMenu menu;
    InputManager im;
    Color transparent = new Color(1, 1, 1, 0);

    Dictionary<string, string> displayStringDict;
    Dictionary<string, Sprite> spriteDict;

    private void Awake()
    {
        menu = GetComponentInParent<RebindControlsMenu>();
    }

    private void Start()
    {
        im = menu.im;
        if(inputActionReference != null)
        {
            /*
            if (inputActionReference.action != null)
            {
                actionName = inputActionReference.action.name;
                im.LoadBinding(actionName);
            }
            */
            GetBindingInfo();
            UpdateUI();
        }
    }

    private void OnValidate()
    {
        if (inputActionReference == null || Application.isPlaying) return;
        GetBindingInfo();
        UpdateUI();
    }

    public void DoRebind()
    {
        rebindSprite.color = transparent;
        menu.DoRebind(actionName, bindingIndex, rebindText);
    }

    void GetBindingInfo()
    {
        if(inputActionReference.action != null)
        {
            actionName = inputActionReference.action.name;
        }

        if(inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    void UpdateUI()
    {
        if(rebindText != null)
        {
            if (Application.isPlaying)
            {
                displayStringDict = GetStringDictionary();
                spriteDict = GetSpriteDictionary();

                string displayString = im.GetBindingName(actionName, bindingIndex);
                displayString = RemoveInteractions(displayString);
                if (isGamepad && spriteDict.ContainsKey(displayString))
                {
                    rebindText.text = displayStringDict[displayString];
                    rebindSprite.sprite = spriteDict[displayString];
                    if (spriteDict[displayString] == null)
                    {
                        rebindSprite.color = transparent;
                    }
                    else
                    {
                        rebindSprite.color = Color.white;
                    }
                }
                else
                {
                    rebindText.text = displayString;
                    rebindSprite.sprite = null;
                    rebindSprite.color = transparent;
                }
            }
            else
            {
                string initialString = inputActionReference.action.GetBindingDisplayString(bindingIndex);
                rebindText.text = RemoveInteractions(initialString);
            }
        }
    }

    Dictionary<string, string> GetStringDictionary()
    {
        if(isGamepad && Gamepad.current == null)
        {
            return settingsData.defaultGamepadDisplayStringDict;
        }
        else
        {
            return settingsData.GetStringDictionary();
        }
    }

    Dictionary<string, Sprite> GetSpriteDictionary()
    {
        if(isGamepad && Gamepad.current == null)
        {
            return settingsData.defaultGamepadSpriteDict;
        }
        else
        {
            return settingsData.GetSpriteDictionary();
        }
    }

    string RemoveInteractions(string initialString)
    {
        initialString = initialString.Replace("Hold or Tap ", "");
        return initialString;
    }

    private void OnEnable()
    {
        menu.rebindComplete += UpdateUI;
        menu.rebindCanceled += UpdateUI;
    }

    private void OnDisable()
    {
        menu.rebindComplete -= UpdateUI;
        menu.rebindCanceled -= UpdateUI;
    }
}
