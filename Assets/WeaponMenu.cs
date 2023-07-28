using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public enum MenuWeaponSelected
{
    NONE, SWORD, LANTERN, KNIFE, CLAWS
}

public class WeaponMenu : MonoBehaviour
{
    // input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] List<Image> weaponImages;
    [SerializeField] RectTransform weaponsDescriptions;
    [SerializeField] RectTransform weaponsIcon;
    [SerializeField] List<Vector3> positions;

    PlayerControls controls;
    [System.NonSerialized] public PauseMenuButtons pauseMenu;
    [System.NonSerialized] public MenuWeaponSelected weaponSelected = MenuWeaponSelected.NONE;
    Dictionary<MenuWeaponSelected, Vector3> iconDict;
    Vector3 initialScale;
    float timer;
    [System.NonSerialized] public float transitionTime = 0.5f;
    float ratio;

    public event EventHandler onSelectWeapon;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Sword.performed += ctx => SelectWeapon(MenuWeaponSelected.SWORD);
        controls.Menu.Lantern.performed += ctx => SelectWeapon(MenuWeaponSelected.LANTERN);
        controls.Menu.Knife.performed += ctx => SelectWeapon(MenuWeaponSelected.KNIFE);
        controls.Menu.Claws.performed += ctx => SelectWeapon(MenuWeaponSelected.CLAWS);
    }

    private void Start()
    {
        iconDict = new Dictionary<MenuWeaponSelected, Vector3>
        {
            { MenuWeaponSelected.NONE, positions[0] },
            { MenuWeaponSelected.SWORD, positions[1] },
            { MenuWeaponSelected.LANTERN, positions[2] },
            { MenuWeaponSelected.KNIFE, positions[3] },
            { MenuWeaponSelected.CLAWS, positions[4] }
        };
        initialScale = weaponsIcon.localScale;
        SelectWeapon(MenuWeaponSelected.NONE);
    }

    void SelectWeapon(MenuWeaponSelected weapon)
    {
        if(weaponSelected == weapon)
        {
            weaponSelected = MenuWeaponSelected.NONE;
        }
        else
        {
            weaponSelected = weapon;
        }

        StopAllCoroutines();
        StartCoroutine(MoveIcon(weaponSelected));
        onSelectWeapon?.Invoke(this, EventArgs.Empty);
    }

    IEnumerator MoveIcon(MenuWeaponSelected weapon)
    {
        Vector3 currentPosition = weaponsDescriptions.localPosition;
        Vector3 currentScale = weaponsIcon.localScale;
        Vector3 targetScale;
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
