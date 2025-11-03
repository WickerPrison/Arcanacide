using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum MenuWeaponSelected
{
    NONE, SWORD, LANTERN, KNIFE, CLAWS
}

public class WeaponMenu : MonoBehaviour
{
    // input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] List<GameObject> weaponImages;
    [SerializeField] List<GameObject> questionMarks;
    [SerializeField] RectTransform weaponsDescriptions;
    [SerializeField] RectTransform weaponsIcon;
    [SerializeField] List<Vector3> positions;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject numbers;

    SoundManager sm;
    PlayerControls controls;
    [System.NonSerialized] public PauseMenuButtons pauseMenu;
    [System.NonSerialized] public RestMenuButtons restMenu;
    [System.NonSerialized] public MenuWeaponSelected weaponSelected = MenuWeaponSelected.NONE;
    Dictionary<MenuWeaponSelected, Vector3> iconDict;
    Dictionary<MenuWeaponSelected, int> intDict;
    Vector3 initialScale;
    Vector3 centerPosition;
    Vector3 leftPosition;
    Vector3 rightPosition;
    float vibrateDuration = 0.3f;
    float vibrateAmp = 5;
    float vibrateFreq = 40;
    float timer;
    [System.NonSerialized] public float transitionTime = 0.5f;
    float ratio;
    bool movingIcon = false;

    public event EventHandler onSelectWeapon;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.MenuSword.performed += ctx => SelectWeapon(MenuWeaponSelected.SWORD);
        controls.Menu.MenuLantern.performed += ctx => SelectWeapon(MenuWeaponSelected.LANTERN);
        controls.Menu.MenuKnife.performed += ctx => SelectWeapon(MenuWeaponSelected.KNIFE);
        controls.Menu.MenuClaws.performed += ctx => SelectWeapon(MenuWeaponSelected.CLAWS);
        controls.Menu.Back.performed += ctx => Back();
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        iconDict = new Dictionary<MenuWeaponSelected, Vector3>
        {
            { MenuWeaponSelected.NONE, positions[0] },
            { MenuWeaponSelected.SWORD, positions[1] },
            { MenuWeaponSelected.LANTERN, positions[2] },
            { MenuWeaponSelected.KNIFE, positions[3] },
            { MenuWeaponSelected.CLAWS, positions[4] }
        };

        intDict = new Dictionary<MenuWeaponSelected, int>
        {
            { MenuWeaponSelected.NONE, 0 },
            { MenuWeaponSelected.SWORD, 0 },
            { MenuWeaponSelected.LANTERN, 1 },
            { MenuWeaponSelected.KNIFE, 2 },
            { MenuWeaponSelected.CLAWS, 3 }
        };

        centerPosition = weaponsIcon.localPosition;
        leftPosition = centerPosition - Vector3.right * vibrateAmp;
        rightPosition = centerPosition + Vector3.right * vibrateAmp;

        for (int i = 0; i < 4; i++)
        {
            if (!playerData.unlockedWeapons.Contains(i))
            {
                weaponImages[i].SetActive(false);
                questionMarks[i].SetActive(true);
            }
        }

        initialScale = weaponsIcon.localScale;
        SelectWeapon(MenuWeaponSelected.NONE);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    private void Update()
    {
        if(Gamepad.current == null)
        {
            numbers.SetActive(true);
        }
        else
        {
            numbers.SetActive(false);
        }
    }

    public void SelectWeaponButton(string weapon)
    {
        switch (weapon)
        {
            case "Sword":
                SelectWeapon(MenuWeaponSelected.SWORD);
                break;
            case "Lantern":
                SelectWeapon(MenuWeaponSelected.LANTERN);
                break;
            case "Knife":
                SelectWeapon(MenuWeaponSelected.KNIFE);
                break;
            case "Claws":
                SelectWeapon(MenuWeaponSelected.CLAWS);
                break;
        }
    }

    void SelectWeapon(MenuWeaponSelected weapon)
    {
        if (!playerData.unlockedWeapons.Contains(intDict[weapon]))
        {
            if (!movingIcon)
            {
                StartCoroutine(FailToPress());
            }
            return;
        }

        if(weaponSelected == weapon)
        {
            weaponSelected = MenuWeaponSelected.NONE;
        }
        else
        {
            weaponSelected = weapon;
        }

        StopAllCoroutines();
        weaponsIcon.localPosition = centerPosition;
        StartCoroutine(MoveIcon(weaponSelected));
        onSelectWeapon?.Invoke(this, EventArgs.Empty);
    }

    IEnumerator MoveIcon(MenuWeaponSelected weapon)
    {
        Vector3 currentPosition = weaponsDescriptions.localPosition;
        Vector3 currentScale = weaponsIcon.localScale;
        Vector3 targetScale;
        movingIcon = true;
        if(weapon == MenuWeaponSelected.NONE)
        {
            targetScale = initialScale;
        }
        else
        {
            targetScale = initialScale / 2;
        }

        timer = transitionTime;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            ratio = timer / transitionTime;
            weaponsDescriptions.localPosition = Vector3.Lerp(iconDict[weapon], currentPosition, ratio);
            weaponsIcon.localScale = Vector3.Lerp(targetScale, currentScale, ratio);
            yield return new WaitForEndOfFrame();
        }
        movingIcon = false;
    }

    IEnumerator FailToPress()
    {
        timer = vibrateDuration;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float input = Mathf.Sin(timer * vibrateFreq) * 0.5f + 0.5f;
            weaponsIcon.localPosition = Vector3.Lerp(leftPosition, rightPosition, input);
            yield return new WaitForEndOfFrame();
        }
        weaponsIcon.localPosition = centerPosition;
    }

    void Back()
    {
        if(weaponSelected != MenuWeaponSelected.NONE)
        {
            SelectWeapon(MenuWeaponSelected.NONE);
        }
        else
        {
            LeaveMenu();
        }
    }

    public void LeaveMenu()
    {
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        if(pauseMenu != null)
        {
            EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton);
            pauseMenu.controls.Enable();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(restMenu.firstButton);
            restMenu.controls.Enable();
        }
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
