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
    RebindControlsMenu menu;
    InputManager im;
    Color transparent = new Color(1, 1, 1, 0);

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
                string initialString = im.GetBindingName(actionName, bindingIndex);
                Debug.Log(initialString);
                if (isGamepad && menu.spriteDict.ContainsKey(initialString))
                {
                    rebindText.text = menu.displayStringDict[initialString];
                    rebindSprite.sprite = menu.spriteDict[initialString];
                    if (menu.spriteDict[initialString] == null)
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
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
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
