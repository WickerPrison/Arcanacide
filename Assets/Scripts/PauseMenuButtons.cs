using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public void ResumeGame()
    {
        Destroy(gameObject);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
