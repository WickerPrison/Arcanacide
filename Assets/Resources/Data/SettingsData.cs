using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Audio;
using FMODUnity;

public enum VolumeChannel
{
    MASTER, MUSIC, SFX
}

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    [SerializeField] AudioMixer audioMixer;

    public Dictionary<string, string> bindings = new Dictionary<string, string>();

    private Dictionary<string, Sprite> spriteDict;
    private Dictionary<string, string> displayStringDict;
    public List<Sprite> buttonIconSprites = new List<Sprite>();
    public List<TMP_SpriteAsset> buttonIconTMProSprites = new List<TMP_SpriteAsset>();

    public bool showArrow;

    public float masterVol;
    public float sfxVol;
    public float musicVol;

    public bool fullscreenMode;

    bool vsync;

    private enum ControllerType
    {
        KBM, XBOX, PLAYSTATION, OTHER, NONE
    };
    ControllerType controllerType = ControllerType.NONE;

    private Dictionary<string, Sprite> default_gamepad_spriteDict;
    public Dictionary<string, Sprite> defaultGamepadSpriteDict
    {
        get
        {
            if(default_gamepad_spriteDict == null)
            {
                default_gamepad_spriteDict = new Dictionary<string, Sprite>
                {
                    { "Y", buttonIconSprites[0] },
                    { "B", buttonIconSprites[1] },
                    { "A", buttonIconSprites[2] },
                    { "X", buttonIconSprites[3] },
                    { "LS", null },
                    { "RS", null },
                    { "Left Stick Press", null },
                    { "Right Stick Press", null }
                };
            }
            return default_gamepad_spriteDict;
        }
    }
    private Dictionary<string, string> default_gamepad_display_stringDict;
    public Dictionary<string, string> defaultGamepadDisplayStringDict
    {
        get
        {
            if(default_gamepad_display_stringDict == null)
            {
                default_gamepad_display_stringDict = new Dictionary<string, string>
                {
                    {"Y", "" },
                    {"B", "" },
                    {"A", "" },
                    {"X", "" },
                    {"LS", "Left Stick" },
                    {"RS", "Right Stick" },
                    {"Left Stick Press", "L3" },
                    {"Right Stick Press", "R3" }
                };
            }
            return default_gamepad_display_stringDict;
        }
    }


    public void SetVolume(VolumeChannel channel, float normalizedVolume)
    {
        //float volume = Mathf.Lerp(-80, 0, normalizedVolume);
        switch (channel)
        {
            case VolumeChannel.MASTER:
                RuntimeManager.GetBus("bus:/").setVolume(normalizedVolume);
                masterVol = normalizedVolume;
                break;
            case VolumeChannel.SFX:
                RuntimeManager.GetBus("bus:/SFX").setVolume(normalizedVolume);
                sfxVol = normalizedVolume;
                break;
            case VolumeChannel.MUSIC:
                RuntimeManager.GetBus("bus:/Music").setVolume(normalizedVolume);
                musicVol = normalizedVolume;
                break;
        }
    }

    public void SetVsync(bool vsyncValue)
    {
        vsync = vsyncValue;
        QualitySettings.vSyncCount = vsyncValue ? 1 : 0;
    }

    public bool GetVsync()
    {
        return vsync;
    }

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
            {"Y", buttonIconSprites[0] },
            {"B", buttonIconSprites[1] },
            {"A", buttonIconSprites[2] },
            {"X", buttonIconSprites[3] },
            {"LS", null },
            {"RS", null },
            {"Left Stick Press", null },
            {"Right Stick Press", null }
        };

        displayStringDict = new Dictionary<string, string>
        {
            {"Y", "" },
            {"B", "" },
            {"A", "" },
            {"X", "" },
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
            {"<Gamepad>/buttonNorth", buttonIconSprites[0] },
            {"<Gamepad>/buttonEast", buttonIconSprites[1] },
            {"<Gamepad>/buttonSouth", buttonIconSprites[2] },
            {"<Gamepad>/buttonWest", buttonIconSprites[3] }
        };

        displayStringDict = new Dictionary<string, string>
        {
            {"<Gamepad>/buttonNorth", "" },
            {"<Gamepad>/buttonEast", "" },
            {"<Gamepad>/buttonSouth", "" },
            {"<Gamepad>/buttonWest", "" }

        };
    }
}
