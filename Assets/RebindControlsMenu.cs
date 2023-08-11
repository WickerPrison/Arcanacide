using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RebindControlsMenu : MonoBehaviour
{
    [System.NonSerialized] public PauseMenuButtons pauseMenu;
    [SerializeField] GameObject firstButton;
    InputManager im;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void RebindMapButton()
    {
        var rebindOperation = im.controls.Gameplay.Map.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .Start();

        rebindOperation.OnComplete(operation => operation.Dispose());
    }

    public void RebindButton(InputAction action)
    {
        var rebind = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .Start();

        rebind.OnComplete(operation => operation.Dispose());
    }

    public void LeaveMenu()
    {
        //sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton);
        pauseMenu.controls.Enable();
        
        Destroy(gameObject);
    }
}
