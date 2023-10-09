using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music
{
    NONE, MAINMENU, PEACEFUL, FIREBOSS
}

public class MusicPlayer : MonoBehaviour
{
    public Music currentTrack = Music.NONE;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] EventReference[] fmodEvents;
    [SerializeField] SettingsData settingsData;
    Dictionary<Music, EventReference> playlistDict;
    EventInstance musicInstance;

    private void Awake()
    {
        playlistDict = new Dictionary<Music, EventReference>()
        {
            {Music.MAINMENU, fmodEvents[0]},
            {Music.PEACEFUL, fmodEvents[1]},
            {Music.FIREBOSS, fmodEvents[2]}
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(Music musicOption)
    {
        musicInstance.release();
        musicInstance = RuntimeManager.CreateInstance(playlistDict[musicOption]);
        musicInstance.start();
        currentTrack = musicOption;

        settingsData.SetVolume(VolumeChannel.MASTER, settingsData.masterVol);
        settingsData.SetVolume(VolumeChannel.MUSIC, settingsData.musicVol);
    }

    public void StopFadeOut()
    {
        currentTrack = Music.NONE;
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }

    public void StopImmediate()
    {
        currentTrack = Music.NONE;
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }
}
