using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SettingsSaveData
{

    public string[] bindingDictionaryKeys;
    public string[] bindingDictionaryValues;
    public bool showArrow = true;
    public float masterVol;
    public float sfxVol;
    public float musicVol;
    public bool fullscreenMode = true;

    public SettingsSaveData(SettingsData settingsData)
    {
        bindingDictionaryKeys = settingsData.bindings.Keys.ToArray();
        bindingDictionaryValues = settingsData.bindings.Values.ToArray();
        showArrow = settingsData.showArrow;
        masterVol = settingsData.masterVol;
        sfxVol = settingsData.sfxVol;
        musicVol = settingsData.musicVol;
        fullscreenMode = settingsData.fullscreenMode;
    }
}
