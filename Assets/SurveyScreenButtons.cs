using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyScreenButtons : MonoBehaviour
{
    [SerializeField] GameObject buttonAudioPrefab;
    [SerializeField] SoundManager sm;

    public void NoSurvey()
    {
        sm.ButtonSound();
        Application.Quit();
    }

    public void GoToSurvey()
    {
        sm.ButtonSound();
        Application.OpenURL("https://forms.gle/DH89JpViSvfW55Yn8");
        Application.Quit();
    }
}
