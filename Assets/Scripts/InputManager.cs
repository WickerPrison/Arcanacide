using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerControls controls;
    GameObject player;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Gameplay();
        }
    }

    private void Update()
    {
        if(Gamepad.current == null)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    public void Gameplay()
    {
        player.layer = 3;
        controls.Menu.Disable();
        controls.Tutorial.Disable();
        controls.Dialogue.Disable();
        controls.Gameplay.Enable();
    }

    public void Tutorial()
    {
        player.layer = 8;
        controls.Menu.Disable();
        controls.Gameplay.Disable();
        controls.Dialogue.Disable();
        controls.Tutorial.Enable();
    }

    public void Menu()
    {
        player.layer = 3;
        controls.Tutorial.Disable();
        controls.Gameplay.Disable();
        controls.Dialogue.Disable();
        controls.Menu.Enable();
    }

    public void Dialogue()
    {
        player.layer = 8;
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
