using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindButton : MonoBehaviour
{
    [SerializeField] InputActionReference inputActionReference;
    string actionName;
    [Range(0,10)] [SerializeField] int selectedBinding;
    [SerializeField] InputBinding.DisplayStringOptions displayStringOptions;
    [Header("Binding Infor - DO NOT EDIT")]
    [SerializeField] InputBinding inputBinding;
    int bindingIndex;
    [SerializeField] TextMeshProUGUI rebindText;
    RebindControlsMenu menu;
    InputManager im;

    private void Awake()
    {
        menu = GetComponentInParent<RebindControlsMenu>();
    }

    private void Start()
    {
        im = menu.im;
        menu.buttons.Add(gameObject);
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
                rebindText.text = im.GetBindingName(actionName, bindingIndex);
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
