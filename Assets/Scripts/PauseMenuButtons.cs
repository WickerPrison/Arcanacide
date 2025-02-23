using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject resumeButton;
    PlayerMovement playerMovement;
    SoundManager sm;
    MusicManager musicManager;
    GameObject textMenu;
    GameObject weaponMenu;
    GameObject settingsMenu;
    GameObject bugReporter;
    [SerializeField] GameObject textMenuPrefab;
    [SerializeField] GameObject weaponMenuPrefab;
    [SerializeField] GameObject settingsMenuPrefab;
    [SerializeField] GameObject bugReporterPrefab;
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
        musicManager = sm.GetComponent<MusicManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
        playerMovement.PauseMenu();
    }

    public void OpenTextMenu()
    {
        sm.ButtonSound();
        textMenu = Instantiate(textMenuPrefab);
        textMenu.GetComponent<TextingMenu>().pauseMenuScript = this;
        controls.Disable();
    }

    public void OpenWeaponMenu()
    {
        sm.ButtonSound();
        weaponMenu = Instantiate(weaponMenuPrefab);
        weaponMenu.GetComponent<WeaponMenu>().pauseMenu = this;
        controls.Disable();
    }

    public void OpenSettingsMenu()
    {
        sm.ButtonSound();
        settingsMenu = Instantiate(settingsMenuPrefab);
        settingsMenu.GetComponent<SettingsMenu>().pauseMenu = this;
        controls.Disable();
    }

    public void OpenBugReport()
    {
        sm.ButtonSound();
        bugReporter = Instantiate(bugReporterPrefab);
        bugReporter.GetComponent<BugReporter>().pauseMenu = this;
        controls.Disable();
    }

    public void MainMenu()
    {
        musicManager.StopImmediate();
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
