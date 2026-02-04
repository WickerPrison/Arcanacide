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
    [SerializeField] ToggleUI arrowToggle;
    [SerializeField] ToggleUI fullscreenToggle;
    [SerializeField] ToggleUI vsyncToggle;
    [SerializeField] OptionSetUi frameRateUI;
    [SerializeField] Image background;
    [System.NonSerialized] public GameObject firstMainMenuButton;
    bool canScrollSideways = true;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.started += ctx => LeaveMenu();
        controls.Menu.ControllerDirection.performed += ctx => OptionSelectInput(ctx.ReadValue<Vector2>().x);
        controls.Menu.ControllerDirection.canceled += ctx => canScrollSideways = true;
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        gm = sm.GetComponent<GameManager>();
        gm.LoadSettings();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);     
        arrowToggle.SetToggleInstant(settingsData.showArrow);
        fullscreenToggle.SetToggleInstant(settingsData.fullscreenMode);
        vsyncToggle.SetToggleInstant(settingsData.GetVsync());
        frameRateUI.ImmediateSelectOption(settingsData.frameRateLimit);
        UpdateMenu();
    }

    void UpdateMenu()
    {
        arrowToggle.ToggleSwitch(settingsData.showArrow);
        fullscreenToggle.ToggleSwitch(settingsData.fullscreenMode);
        vsyncToggle.ToggleSwitch(settingsData.GetVsync());
        frameRateUI.SelectOption(settingsData.frameRateLimit);

        SaveSystem.SaveSettings(settingsData);
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

    public void ToggleVsync()
    {
        settingsData.SetVsync(settingsData.GetVsync());
        UpdateMenu();
    }

    void OptionSelectInput(float input)
    {
        if (!frameRateUI.selected) return;
        if (Mathf.Abs(input) < 0.5f) canScrollSideways = true;

        if (!canScrollSideways) return;

        if (input > 0.9f)
        {
            FrameRateRight();
            canScrollSideways = false;
        }
        else if(input < -0.9f)
        {
            FrameRateLeft();
            canScrollSideways = false;
        }
    }

    public void FrameRateLeft()
    {
        settingsData.frameRateLimit = settingsData.frameRateLimit - 1 >= 0 ? settingsData.frameRateLimit - 1 : 3;
        frameRateUI.SelectOption(settingsData.frameRateLimit);
    }

    public void FrameRateRight()
    {
        settingsData.frameRateLimit = settingsData.frameRateLimit + 1 <= 3 ? settingsData.frameRateLimit + 1 : 0;
        frameRateUI.SelectOption(settingsData.frameRateLimit);
    }

    public void LeaveMenu()
    {
        SaveSystem.SaveSettings(settingsData);
        if (background.enabled == false)
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
