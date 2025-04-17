using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlMode
{
    GAMEPLAY, TUTORIAL, MENU, DIALOGUE, DISABLED
}

public class InputManager : MonoBehaviour
{
    public PlayerControls controls;
    [SerializeField] SettingsData settingsData;
    [SerializeField] List<InputActionReference> actions;
    [SerializeField] List<InputActionReference> selectActions;
    GameObject player;
    [System.NonSerialized] public ControlMode controlMode;

    private void Awake()
    {
        controls = new PlayerControls();
        foreach(InputActionReference inputAction in actions)
        {
            LoadBinding(inputAction.action.name);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Gameplay();
        }

    }

    private void Start()
    {

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

    public void ResetAllBindings()
    {
        settingsData.bindings.Clear();
        foreach(InputActionReference inputActionReference in actions)
        {
            InputAction inputAction = controls.asset.FindAction(inputActionReference.action.name);
            if (inputAction == null) continue;
            inputAction.RemoveAllBindingOverrides();
        }
        GlobalEvents.instance.OnChangedSetting();
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

        string noIndexKey = inputAction.actionMap + inputAction.name;
        if(actionName == "Interact")
        {
            CopyBindings(selectActions[0], noIndexKey);
            CopyBindings(selectActions[1], noIndexKey);
        }
    }

    void CopyBindings(InputActionReference copyTo, string noIndexKey)
    {
        bool hasBinding = false;
        foreach(KeyValuePair<string, string> pair in settingsData.bindings)
        {
            if(pair.Key.Contains(noIndexKey))
            {
                hasBinding = true;
                break;
            }
        }
        if (!hasBinding) return;

        InputAction action = controls.asset.FindAction(copyTo.name);
        if (action == null) return;

        for(int i = 0; i < action.bindings.Count; i++)
        {
            string key = noIndexKey + i;
            if(settingsData.bindings.ContainsKey(key))
            {
                action.ApplyBindingOverride(i, settingsData.bindings[key]);
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
        controlMode = ControlMode.GAMEPLAY;
        player.layer = 3;
        controls.Menu.Disable();
        controls.Tutorial.Disable();
        controls.Dialogue.Disable();
        controls.Gameplay.Enable();
    }

    public void Tutorial()
    {
        controlMode = ControlMode.TUTORIAL;
        player.layer = 8;
        controls.Menu.Disable();
        controls.Gameplay.Disable();
        controls.Dialogue.Disable();
        controls.Tutorial.Enable();
    }

    public void Menu()
    {
        controlMode = ControlMode.MENU;
        player.layer = 3;
        controls.Tutorial.Disable();
        controls.Gameplay.Disable();
        controls.Dialogue.Disable();
        controls.Menu.Enable();
    }

    public void Dialogue()
    {
        controlMode = ControlMode.DIALOGUE;
        player.layer = 8;
        controls.Tutorial.Disable();
        controls.Gameplay.Disable();
        controls.Menu.Disable();
        controls.Dialogue.Enable();
    }

    public void DisableAll()
    {
        controlMode = ControlMode.DISABLED;
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
