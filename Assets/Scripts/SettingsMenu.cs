using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject firstButton;
    SoundManager sm;
    GameObject rebindControlsMenu;
    [SerializeField] GameObject rebindControlsMenuPrefab;
    public PlayerControls controls;
    [System.NonSerialized] public PauseMenuButtons pauseMenu;
    [SerializeField] SettingsData settingsData;
    [SerializeField] TextMeshProUGUI direcitonalArrowText;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.started += ctx => LeaveMenu();
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        UpdateMenu();
    }

    void UpdateMenu()
    {
        if (settingsData.showArrow) direcitonalArrowText.text = "On";
        else direcitonalArrowText.text = "Off";
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

    public void LeaveMenu()
    {
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton);
        pauseMenu.controls.Enable();

        Destroy(gameObject);
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
