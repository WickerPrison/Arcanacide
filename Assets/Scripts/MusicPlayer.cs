using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music
{
    NONE, MAINMENU
}

public class MusicPlayer : MonoBehaviour
{
    public Music currentTrack = Music.NONE;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] EventReference[] fmodEvents;
    [SerializeField] SettingsData settingsData;
    Dictionary<Music, EventReference> playlistDict;
    EventInstance musicInstance;
    Coroutine musicFade;

    private void Awake()
    {
        playlistDict = new Dictionary<Music, EventReference>()
        {
            {Music.MAINMENU, fmodEvents[0]}
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
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }

    public void StopImmediate()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }
}
