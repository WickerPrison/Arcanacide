using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        Gameplay();
    }

    public void Gameplay()
    {
        controls.Menu.Disable();
        controls.Tutorial.Disable();
        controls.Dialogue.Disable();
        controls.Gameplay.Enable();
    }

    public void Tutorial()
    {
        controls.Menu.Disable();
        controls.Gameplay.Disable();
        controls.Dialogue.Disable();
        controls.Tutorial.Enable();
    }

    public void Menu()
    {
        controls.Tutorial.Disable();
        controls.Gameplay.Disable();
        controls.Dialogue.Disable();
        controls.Menu.Enable();
    }

    public void Dialogue()
    {
        controls.Tutorial.Disable();
        controls.Gameplay.Disable();
        controls.Menu.Disable();
        controls.Dialogue.Enable();
    }

    public void DisableAll()
    {
        controls.Tutorial.Disable();
        controls.Gameplay.Disable();
        controls.Menu.Disable();
        controls.Dialogue.Disable();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
