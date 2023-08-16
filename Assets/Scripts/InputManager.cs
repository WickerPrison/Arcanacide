using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerControls controls;
    [SerializeField] SettingsData settingsData;
    [SerializeField] List<InputActionReference> actions;
    GameObject player;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        foreach(InputActionReference inputAction in actions)
        {
            LoadBinding(inputAction.action.name);
        }
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

    public void SaveBinding(InputAction inputAction)
    {
        for (int i = 0; i < inputAction.bindings.Count; i++)
        {
            string key = inputAction.actionMap + inputAction.name + i;

            if (inputAction.bindings[i].overridePath != null)
            {
                if (settingsData.bindings.ContainsKey(key))
                {
                    settingsData.bindings[key] = inputAction.bindings[i].overridePath;
                }
                else
                {
                    settingsData.bindings.Add(key, inputAction.bindings[i].overridePath);
                }
            }
        }
    }

    public void LoadBinding(string actionName)
    {
        InputAction inputAction = controls.asset.FindAction(actionName);
        if (inputAction == null) return;

        for (int i = 0; i < inputAction.bindings.Count; i++)
        {
            string key = inputAction.actionMap + inputAction.name + i;
            if (settingsData.bindings.ContainsKey(key))
            {
                inputAction.ApplyBindingOverride(i, settingsData.bindings[key]);
            }
        }
    }

    public string GetBindingName(string actionName, int bindingIndex)
    {
        if (actionName == null || bindingIndex < 0) return "null";
        InputAction action = controls.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
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
