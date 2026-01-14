using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyScreenButtons : MonoBehaviour
{
    [SerializeField] GameObject buttonAudioPrefab;
    [SerializeField] SoundManager sm;
    [SerializeField] BuildMode buildMode;

    public void NoSurvey()
    {
        sm.ButtonSound();
        Application.Quit();
    }

    public void GoToSurvey()
    {
        sm.ButtonSound();
        Application.OpenURL($"https://docs.google.com/forms/d/e/1FAIpQLSd9OFB_8rb6sr38Rgm3saaua8fbQS1s8fT2ZeOoqbA_Sv9Svw/viewform?usp=pp_url&entry.1945729572={buildMode.versionNumber}");
        Application.Quit();
    }
}
