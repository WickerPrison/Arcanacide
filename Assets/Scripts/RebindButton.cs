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
    [SerializeField] InputActionReference[] inputActionReference;
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
        menu.DoRebind(actionName, bindingIndex, rebindText);
    }

    void GetBindingInfo()
    {
        for(int i = 0; i < inputActionReference.Length; i++)
        {
            Debug.Log(inputActionReference);
            if(inputActionReference[i].action != null)
            {
                actionName = inputActionReference[i].action.name;
            }

            if(inputActionReference[i].action.bindings.Count > selectedBinding)
            {
                inputBinding = inputActionReference[i].action.bindings[selectedBinding];
                bindingIndex = selectedBinding;
            }
        }
    }

    void UpdateUI()
    {
        if(rebindText != null)
        {
            if (Application.isPlaying)
            {
                displayStringDict = settingsData.GetStringDictionary();
                spriteDict = settingsData.GetSpriteDictionary();

                string initialString = im.GetBindingName(actionName, bindingIndex);
                if (isGamepad && spriteDict.ContainsKey(initialString))
                {
                    rebindText.text = displayStringDict[initialString];
                    rebindSprite.sprite = spriteDict[initialString];
                    if (spriteDict[initialString] == null)
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
                    rebindText.text = im.GetBindingName(actionName, bindingIndex);
                    rebindSprite.sprite = null;
                    rebindSprite.color = transparent;
                }
            }
            else
            {
                rebindText.text = inputActionReference[0].action.GetBindingDisplayString(bindingIndex);
            }
        }
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
