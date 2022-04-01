using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject resumeButton;
    PlayerController playerController;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void ResumeGame()
    {
        playerController.preventInput = false;
        Destroy(gameObject);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
