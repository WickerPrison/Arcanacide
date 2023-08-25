using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class VolumeSlider : SliderUI
{
    [SerializeField] VolumeChannel volumeChannel;
    [SerializeField] SettingsData settingsData;

    public override void Start()
    {
        base.Start();
        switch(volumeChannel)
        {
            case VolumeChannel.MASTER:
                slidePosNorm = settingsData.masterVol;
                break;
            case VolumeChannel.SFX:
                slidePosNorm = settingsData.sfxVol;
                break;
            case VolumeChannel.MUSIC:
                slidePosNorm = settingsData.musicVol;
                break;
        }
        MoveSlider();
    }

    public override void MoveSlider()
    {
        base.MoveSlider();
        settingsData.SetVolume(volumeChannel, slidePosNorm);
    }
}
