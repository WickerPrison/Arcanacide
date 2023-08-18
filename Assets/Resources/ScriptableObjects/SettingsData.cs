using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    public Dictionary<string, string> bindings = new Dictionary<string, string>();

    private Dictionary<string, Sprite> spriteDict;
    private Dictionary<string, string> displayStringDict;
    [SerializeField] Sprite northButton;
    [SerializeField] Sprite westButton;
    [SerializeField] Sprite eastButton;
    [SerializeField] Sprite southButton;

    private enum ControllerType
    {
        KBM, XBOX, PLAYSTATION, OTHER, NONE
    };
    ControllerType controllerType = ControllerType.NONE;

    public void CreateBindingDictionary(string[] keys, string[] values)
    {
        if(keys == null || values == null) return;
        if (keys.Length != values.Length) 
            Debug.Log("Error: Different number of keys and values");

        bindings.Clear();
        if (keys.Length <= 0) return;

        for(int i = 0; i < keys.Length; i++)
        {
            bindings.Add(keys[i], values[i]);
        }
    }

    public Dictionary<string, string> GetStringDictionary()
    {
        if (NeedNewDictionary()) SetupDictionaries();
        return displayStringDict;
    }

    public Dictionary<string, Sprite> GetSpriteDictionary()
    {
        if(NeedNewDictionary()) SetupDictionaries();
        return spriteDict;
    }

    bool NeedNewDictionary()
    {
        if (displayStringDict == null && spriteDict == null) return true;

        if(Gamepad.current == null)
        {
            if (controllerType != ControllerType.KBM) return true;
            else return false;
        }
        else
        {
            if (Gamepad.current is XInputController && controllerType != ControllerType.XBOX) return true;

            if (Gamepad.current is DualShockGamepad && controllerType != ControllerType.PLAYSTATION) return true;
        }

        return false;
    }

    void SetupDictionaries()
    {
        if(Gamepad.current == null)
        {
            SetupKBMDicitonaries();
        }
        else if (Gamepad.current is XInputController)
        {
            SetupXboxDictionaries();
        }
        else if (Gamepad.current is DualShockGamepad)
        {
            SetupPlaystationDictionaries();
        }
    }

    void SetupKBMDicitonaries()
    {
        controllerType = ControllerType.KBM;
        spriteDict = new Dictionary<string, Sprite>();
        displayStringDict = new Dictionary<string, string>();
    }

    void SetupXboxDictionaries()
    {
        controllerType = ControllerType.XBOX;
        spriteDict = new Dictionary<string, Sprite>
        {
            {"Y", northButton },
            {"B", eastButton },
            {"X", westButton },
            {"A", southButton },
            {"LS", null },
            {"RS", null },
            {"Left Stick Press", null },
            {"Right Stick Press", null }
        };

        displayStringDict = new Dictionary<string, string>
        {
            {"Y", "" },
            {"B", "" },
            {"X", "" },
            {"A", "" },
            {"LS", "Left Stick" },
            {"RS", "Right Stick" },
            {"Left Stick Press", "L3" },
            {"Right Stick Press", "R3" }
        };
    }

    void SetupPlaystationDictionaries()
    {
        controllerType = ControllerType.PLAYSTATION;
        spriteDict = new Dictionary<string, Sprite>
        {
            {"<Gamepad>/buttonNorth", northButton },
            {"<Gamepad>/buttonEast", eastButton },
            {"<Gamepad>/buttonWest", westButton },
            {"<Gamepad>/buttonSouth", southButton }
        };

        displayStringDict = new Dictionary<string, string>
        {
            {"<Gamepad>/buttonNorth", "" },
            {"<Gamepad>/buttonEast", "" },
            {"<Gamepad>/buttonWest", "" },
            {"<Gamepad>/buttonSouth", "" }

        };
    }
}
