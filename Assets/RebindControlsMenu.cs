using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RebindControlsMenu : MonoBehaviour
{
    [System.NonSerialized] public SettingsMenu settingsMenu;
    [SerializeField] GameObject firstButton;
    InputManager im;
    PlayerControls menuControls;

    public event Action rebindComplete;
    public event Action rebindCanceled;
    public event Action<InputAction, int> rebindStarted;

    private void Awake()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        menuControls = new PlayerControls();
        menuControls.Menu.Back.performed += ctx => LeaveMenu();
    }

    private void Start()
    {
        //im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void DoRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText)
    {
        InputAction actionToRebind = im.controls.asset.FindAction(actionName);
        if (actionToRebind == null || actionToRebind.bindings.Count <= bindingIndex) return;

        statusText.text = $"Press a {actionToRebind.expectedControlType}";

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
            im.Menu();

            rebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
            im.Menu();

            rebindCanceled?.Invoke();
        });

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
    }

    public string GetBindingName(string actionName, int bindingIndex)
    {
        InputAction action = im.controls.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }

    public void LeaveMenu()
    {
        //sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsMenu.firstButton);
        settingsMenu.controls.Enable();
        
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        menuControls.Enable();
    }

    private void OnDisable()
    {
        menuControls.Disable();
    }
}
