using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject firstButton;
    GameManager gm;
    SoundManager sm;
    GameObject rebindControlsMenu;
    [SerializeField] GameObject rebindControlsMenuPrefab;
    public PlayerControls controls;
    [System.NonSerialized] public PauseMenuButtons pauseMenu;
    [SerializeField] SettingsData settingsData;
    TextMeshProUGUI direcitonalArrowText;
    [SerializeField] ToggleUI arrowToggle;
    TextMeshProUGUI fullscreenText;
    [SerializeField] ToggleUI fullscreenToggle;
    [SerializeField] Image background;
    [System.NonSerialized] public GameObject firstMainMenuButton;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.started += ctx => LeaveMenu();
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        gm = sm.GetComponent<GameManager>();
        gm.LoadGame();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);     
        direcitonalArrowText = arrowToggle.GetComponentInChildren<TextMeshProUGUI>();
        arrowToggle.SetToggleInstant(settingsData.showArrow);
        fullscreenText = fullscreenToggle.GetComponentInChildren<TextMeshProUGUI>();
        fullscreenToggle.SetToggleInstant(settingsData.fullscreenMode);
        UpdateMenu();
    }

    void UpdateMenu()
    {
        if (settingsData.showArrow)
        {
            arrowToggle.ToggleSwitch(true);
            direcitonalArrowText.text = "On";
        }
        else
        {
            arrowToggle.ToggleSwitch(false);
            direcitonalArrowText.text = "Off";
        }

        if (settingsData.fullscreenMode)
        {
            fullscreenToggle.ToggleSwitch(true);
            fullscreenText.text = "On";
        }
        else
        {
            fullscreenToggle.ToggleSwitch(false);
            fullscreenText.text = "Off";
        }
        gm.SaveGame();
        GlobalEvents.instance.OnChangedSetting();
    }

    public void OpenRebindControlsMenu()
    {
        sm.ButtonSound();
        rebindControlsMenu = Instantiate(rebindControlsMenuPrefab);
        rebindControlsMenu.GetComponent<RebindControlsMenu>().settingsMenu = this;
        controls.Disable();
    }

    public void ToggleDirectionalArrow()
    {
        settingsData.showArrow = !settingsData.showArrow;
        UpdateMenu();
    }

    public void ToggleFullScreenMode()
    {
        settingsData.fullscreenMode = !settingsData.fullscreenMode;
        Screen.fullScreen = settingsData.fullscreenMode;
        if (settingsData.fullscreenMode)
        {
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        UpdateMenu();
    }

    public void LeaveMenu()
    {
        gm.SaveGame();
        if(background.enabled == false)
        {
            sm.ButtonSound();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton);
            pauseMenu.controls.Enable();
        }
        else
        {
            sm.ButtonSound();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
        }

        Destroy(gameObject);
    }

    public void ActivateBackground()
    {
        background.enabled = true;
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
