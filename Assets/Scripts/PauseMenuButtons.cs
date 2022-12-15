using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject resumeButton;
    PlayerController playerController;
    SoundManager sm;
    GameObject textMenu;
    [SerializeField] GameObject textMenuPrefab;
    [SerializeField] GameObject newMessage;
    [SerializeField] DialogueData dialogueData;
    public PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.started += ctx => ResumeGame();
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (dialogueData.GetNewMessages().Count == 0)
        {
            newMessage.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        playerController.PauseMenu();
    }

    public void OpenTextMenu()
    {
        sm.ButtonSound();
        textMenu = Instantiate(textMenuPrefab);
        textMenu.GetComponent<TextingMenu>().pauseMenuScript = this;
        controls.Disable();
    }

    public void MainMenu()
    {
        sm.ButtonSound();
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
