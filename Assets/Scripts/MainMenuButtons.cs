using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] AltarDirectory altarDirectory;
    [SerializeField] GameManager gm;
    [SerializeField] PlayerData playerData;

    public void Play()
    {
        gm.LoadGame();
        string sceneName = altarDirectory.GetSceneName(playerData.lastAltar);
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        gm.NewGame();
        SceneManager.LoadScene("IntroCutscene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
