using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music
{
    NONE, MAINTITLES, MAINMENU, PEACEFUL, LEVEL1, ELECTRICBOSS, ICEBOSS, LEVEL2, LEVEL3
}

public enum MusicState
{
    MAINLOOP, BOSSDIALOGUE, BOSSMUSIC, BOSSVICTORY, DEATH, OUTRO, MAINMENU
}

public class MusicPlayer : MonoBehaviour
{
    public Music currentTrack = Music.NONE;
    public MusicState state = MusicState.MAINLOOP;
    [SerializeField] EventReference[] fmodEvents;
    [SerializeField] SettingsData settingsData;
    Dictionary<Music, EventReference> playlistDict;
    Dictionary<MusicState, string> stateLabelDict;
    Dictionary<Music, string> parameterNameDict;
    EventInstance musicInstance;

    private void Awake()
    {
        playlistDict = new Dictionary<Music, EventReference>()
        {
            {Music.MAINTITLES, fmodEvents[0]},
            {Music.PEACEFUL, fmodEvents[1]},
            {Music.LEVEL1, fmodEvents[2]},
            {Music.ELECTRICBOSS, fmodEvents[3]},
            {Music.ICEBOSS, fmodEvents[4]},
            {Music.LEVEL2, fmodEvents[5]},
            {Music.LEVEL3, fmodEvents[6] },
            {Music.MAINMENU, fmodEvents[7] }
        };

        parameterNameDict = new Dictionary<Music, string>()
        {
            { Music.LEVEL1, "LEVEL 1 MUSICSTATE" },
            {Music.LEVEL2, "LEVEL 2 MUSICSTATE" },
            {Music.LEVEL3, "LEVEL 3 MUSICSTATE" },
            { Music.MAINTITLES, "MAIN TITLES MUSICSTATE" },
            {Music.PEACEFUL, "peacful" },
            {Music.MAINMENU, "MAIN MENU MUSICSTATE" }
        };

        stateLabelDict = new Dictionary<MusicState, string>()
        {
            {MusicState.MAINLOOP, "MAIN LOOP" },
            {MusicState.BOSSDIALOGUE, "BOSS DIALOGUE" },
            {MusicState.BOSSMUSIC, "BOSS MUSIC" },
            {MusicState.BOSSVICTORY, "BOSS VICTORY" },
            {MusicState.DEATH, "DEATH" },
            {MusicState.OUTRO, "OUTRO" },
            {MusicState.MAINMENU, "MAIN MENU" }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeState(MusicState newState)
    {
        musicInstance.setParameterByNameWithLabel(parameterNameDict[currentTrack], stateLabelDict[newState]);
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

    public void UpdateBossHealth(int healthPercent)
    {
        musicInstance.setParameterByName("BOSS HEALTH", healthPercent);
    }
}
