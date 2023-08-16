using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindControlsMenu : MonoBehaviour
{
    [System.NonSerialized] public SettingsMenu settingsMenu;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject firstButton;
    [SerializeField] SettingsData settingsData;
    public List<GameObject> buttons;
    [System.NonSerialized] public InputManager im;
    PlayerControls menuControls;

    float scrollDir;
    float scrollSpeed = .1f;

    public event Action rebindComplete;
    public event Action rebindCanceled;
    public event Action<InputAction, int> rebindStarted;

    private void Awake()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        menuControls = new PlayerControls();
        menuControls.Menu.Back.performed += ctx => LeaveMenu();
        menuControls.Menu.Scroll.performed += ctx => scrollDir = ctx.ReadValue<Vector2>().y;
        menuControls.Menu.Scroll.canceled += ctx => scrollDir = 0;
    }

    private void Start()
    {
        //im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    private void Update()
    {
        if (Gamepad.current == null)
        {
            MouseScrollPosition();
        }
        else
        {
            ControllerScrollPosition();
        } 
    }

    public void DoRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText)
    {
        InputAction actionToRebind = im.controls.asset.FindAction(actionName);
        if (actionToRebind == null || actionToRebind.bindings.Count <= bindingIndex) return;

        statusText.text = "Press a Button";

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
            im.Menu();
            im.SaveBinding(actionToRebind);
            rebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
            im.Menu();

            rebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");
        rebind.WithControlsExcluding("<Gamepad>/rightStick");
        rebind.WithControlsExcluding("<Gamepad>/leftStick");
        rebind.WithControlsExcluding("<Gamepad>/Start");
        rebind.WithControlsExcluding("<Gamepad>/Select");

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
    }

    public void LeaveMenu()
    {
        //sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsMenu.firstButton);
        settingsMenu.controls.Enable();
        
        Destroy(gameObject);
    }

    void ControllerScrollPosition()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;
        if (!buttons.Contains(currentButton))
        {
            return;
        }

        int index = buttons.IndexOf(currentButton);
        if (index % 2 != 0) index--; 
        float position = index / ((float)buttons.Count - 1);
        position = 1 - position;
        position = Mathf.Round(position * 100f) / 100f;

        float scrollDiff = position - scrollRect.verticalNormalizedPosition;
        scrollDir = scrollDiff / Mathf.Abs(scrollDiff);
        float scrollDistance = scrollDiff * 2f * Time.unscaledDeltaTime + scrollDir * .4f * Time.unscaledDeltaTime;
        if (Mathf.Abs(scrollDiff) <= Mathf.Abs(scrollDistance))
        {
            scrollRect.verticalNormalizedPosition = position;
        }
        else
        {
            scrollRect.verticalNormalizedPosition += scrollDistance;
        }
    }

    void MouseScrollPosition()
    {
        scrollDir /= 120;
        scrollRect.verticalNormalizedPosition += scrollDir * scrollSpeed;
        if (scrollRect.verticalNormalizedPosition > 1)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
        else if (scrollRect.verticalNormalizedPosition < 0)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
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
