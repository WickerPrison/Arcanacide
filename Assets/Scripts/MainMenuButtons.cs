using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] SwordSiteDirectory altarDirectory;
    [SerializeField] GameManager gm;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject buttonAudioPrefab;
    SoundManager sm;

    private void Start()
    {
        sm = gm.gameObject.GetComponent<SoundManager>();
    }

    public void Play()
    {
        ButtonSound();
        gm.LoadGame();
        string sceneName = altarDirectory.GetSceneName(playerData.lastSwordSite);
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        ButtonSound();
        gm.NewGame();
        SceneManager.LoadScene("IntroCutscene");
    }

    public void QuitGame()
    {
        ButtonSound();
        Application.Quit();
    }

    void ButtonSound()
    {
        sm.ButtonSound();
    }
}
