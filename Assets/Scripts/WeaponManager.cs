using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] List<RuntimeAnimatorController> frontAnimatorControllers;
    [SerializeField] List<RuntimeAnimatorController> backAnimatorControllers;
    [SerializeField] List<GameObject> frontWeaponSprites;
    [SerializeField] List<GameObject> backWeaponSprites;
    [SerializeField] PlayerData playerData;
    [System.NonSerialized] public PlayerWeapon previousWeapon;
    public PlayerWeapon editorWeapon;
    PlayerMovement playerController;
    PlayerEvents playerEvents;
    InputManager im;
    [System.NonSerialized] public int weaponMagicSources;
    [SerializeField] GameObject[] knifeOffhand;
    [SerializeField] GameObject[] clawsOffhand;
    int[] specificWeaponMagicSources = new int[4];
    public event EventHandler OnStartWeaponMagic;
    public event EventHandler OnStopWeaponMagic;

    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
    }

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();

        im.controls.Gameplay.Sword.performed += ctx => SwitchWeapon(0);
        im.controls.Gameplay.Axe.performed += ctx => SwitchWeapon(1);
        im.controls.Gameplay.Knife.performed += ctx => SwitchWeapon(2);
        im.controls.Gameplay.Claws.performed += ctx => SwitchWeapon(3);

        playerController = GetComponent<PlayerMovement>();

        SetWeapon(playerData.currentWeapon);
    }

    public void AddWeaponMagicSource()
    {
        weaponMagicSources++;
        OnStartWeaponMagic?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveWeaponMagicSource()
    {
        weaponMagicSources--;
        if(weaponMagicSources + specificWeaponMagicSources[playerData.currentWeapon] <= 0)
        {
            OnStopWeaponMagic?.Invoke(this, EventArgs.Empty);
        }
    }

    public void AddSpecificWeaponSource(int weaponID)
    {
        specificWeaponMagicSources[weaponID]++;
        OnStartWeaponMagic?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveSpecificWeaponSource(int weaponID)
    {
        specificWeaponMagicSources[weaponID]--;
        if(weaponMagicSources + specificWeaponMagicSources[playerData.currentWeapon] <= 0)
        {
            OnStopWeaponMagic?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SwitchWeapon(int nextWeapon)
    {
        if (!playerController.CanInput() || !playerData.unlockedWeapons.Contains(nextWeapon) || playerData.currentWeapon == nextWeapon)
        {
            return;
        }

        playerData.currentWeapon = nextWeapon;
        frontAnimator.runtimeAnimatorController = frontAnimatorControllers[nextWeapon];
        backAnimator.runtimeAnimatorController = backAnimatorControllers[nextWeapon];
        ClearSprites();
        frontAnimator.SetLayerWeight(1, 1);
        backAnimator.SetLayerWeight(1, 1);
        frontAnimator.Play("SwitchWeapon");
        backAnimator.Play("SwitchWeapon");

        if(weaponMagicSources + specificWeaponMagicSources[nextWeapon] > 0)
        {
            OnStartWeaponMagic?.Invoke(this, EventArgs.Empty);
        }
    }

    public void CheckWeaponMagic()
    {
        if (weaponMagicSources + specificWeaponMagicSources[playerData.currentWeapon] > 0)
        {
            OnStartWeaponMagic?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnStopWeaponMagic?.Invoke(this, EventArgs.Empty);
        }
    }

    void SetWeapon(int weaponID)
    {
        playerData.currentWeapon = weaponID;
        frontAnimator.runtimeAnimatorController = frontAnimatorControllers[weaponID];
        backAnimator.runtimeAnimatorController = backAnimatorControllers[weaponID];
        ClearSprites();
        ActivateWeaponSprite(weaponID);

        if (weaponMagicSources + specificWeaponMagicSources[weaponID] > 0)
        { 
            OnStartWeaponMagic?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ActivateWeaponSprite(int weaponID)
    {
        int weaponSpriteId = WeaponSpriteId(weaponID);
        frontWeaponSprites[weaponSpriteId].SetActive(true);
        backWeaponSprites[weaponSpriteId].SetActive(true);
        for(int i = 0; i < 2; i++)
        {
            switch (weaponSpriteId)
            {
                case 2:
                    knifeOffhand[i].SetActive(true);
                    break;
                case 3:
                    clawsOffhand[i].SetActive(true);
                    break;
            }
        }
    }

    public int WeaponSpriteId(int weaponID)
    {
        WeaponElement element = playerData.equippedElements[weaponID];
        switch((weaponID, element)) 
        { 
            case (0, WeaponElement.DEFAULT): return 0;
            case (1, WeaponElement.FIRE): return 1;
            case (2, WeaponElement.ELECTRICITY): return 2;
            case (3, WeaponElement.ICE): return 3;
            case (0, WeaponElement.FIRE): return 4;
            default: return -1;
        }
    }

    void ClearSprites()
    {
        foreach(GameObject sprite in frontWeaponSprites)
        {
            sprite.SetActive(false);
        }
        foreach (GameObject sprite in backWeaponSprites)
        {
            sprite.SetActive(false);
        }
        foreach(GameObject sprite in knifeOffhand)
        {
            sprite.SetActive(false);
        }
        foreach(GameObject sprite in clawsOffhand)
        {
            sprite.SetActive(false);
        }
    }

    public void InspectorChanged()
    {
        Debug.Log("Inspector Changed");
        if (previousWeapon == editorWeapon) return;
        SetWeapon(Utils.GetWeaponIndex(editorWeapon));
        previousWeapon = editorWeapon;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onChangeWeapon += Instance_onChangeWeapon;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onChangeWeapon -= Instance_onChangeWeapon;
    }

    private void Instance_onChangeWeapon(object sender, int index)
    {
        SetWeapon(playerData.currentWeapon);
    }
}
