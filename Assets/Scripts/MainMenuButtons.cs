using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject buttonAudioPrefab;
    [SerializeField] GameObject optionsMenuPrefab;
    [SerializeField] GameObject firstButton;
    SoundManager sm;

    private void Start()
    {
        Time.timeScale = 1;
        sm = gm.gameObject.GetComponent<SoundManager>();
    }

    public void Play()
    {
        ButtonSound();
        gm.LoadGame();
        string sceneName = gm.GetSceneName(playerData.lastSwordSite);
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        ButtonSound();
        gm.NewGame();
        SceneManager.LoadScene("IntroCutscene");
    }

    public void Options()
    {
        SettingsMenu settingsMenu = Instantiate(optionsMenuPrefab).GetComponent<SettingsMenu>();
        settingsMenu.ActivateBackground();
        settingsMenu.firstMainMenuButton = firstButton;
    }

    public void TestFloor2()
    {
        ButtonSound();
        gm.StartAtFloor2();
        SceneManager.LoadScene("ElectricHub");
    }

    public void TestFloor3()
    {
        ButtonSound();
        gm.StartAtFloor3();
        SceneManager.LoadScene("IceHub1");
    }

    public void QuitGame()
    {
        ButtonSound();
        SceneManager.LoadScene("SurveyScreen");
    }

    void ButtonSound()
    {
        sm.ButtonSound();
    }
}
