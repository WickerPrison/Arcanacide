using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject resumeButton;
    PlayerController playerController;
    SoundManager sm;

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void ResumeGame()
    {
        playerController.PauseMenu();
    }

    public void MainMenu()
    {
        sm.ButtonSound();
        SceneManager.LoadScene("MainMenu");
    }
}
