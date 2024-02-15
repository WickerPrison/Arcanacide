using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] PlayerData playerData;
    [SerializeField] MenuData menuData;
    [SerializeField] GameObject optionsMenuPrefab;
    [SerializeField] GameObject firstButton;
    SoundManager sm;

    private void Start()
    {
        Time.timeScale = 1;
        sm = gm.gameObject.GetComponent<SoundManager>();
        gm.LoadSettings();
        GlobalEvents.instance.OnChangedSetting();
    }

    public void Load()
    {
        ButtonSound();
        menuData.loadGame = true;
        SceneManager.LoadScene("LoadScreen");
    }

    public void NewGame()
    {
        ButtonSound();
        menuData.loadGame = false;
        SceneManager.LoadScene("LoadScreen");
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
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        SceneManager.LoadScene("ElectricHub");
    }

    public void TestFloor3()
    {
        ButtonSound();
        gm.StartAtFloor3();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        SceneManager.LoadScene("IceHub1");
    }

    public void TestFloor4()
    {
        ButtonSound();
        gm.StartAtFloor4();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        SceneManager.LoadScene("ChaosHub1");
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
