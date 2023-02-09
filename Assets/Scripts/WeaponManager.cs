using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] List<AnimatorController> frontAnimatorControllers;
    [SerializeField] List<AnimatorController> backAnimatorControllers;
    [SerializeField] List<GameObject> frontWeaponSprites;
    [SerializeField] List<GameObject> backWeaponSprites;
    [SerializeField] PlayerData playerData;
    PlayerController playerController;
    InputManager im;
    int weaponMagicSources;
    int[] specificWeaponMagicSources = new int[4];
    public event EventHandler OnStartWeaponMagic;
    public event EventHandler OnStopWeaponMagic;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();

        im.controls.Gameplay.Sword.performed += ctx => SwitchWeapon(0);
        im.controls.Gameplay.Axe.performed += ctx => SwitchWeapon(1);

        playerController = GetComponent<PlayerController>();

        SwitchWeapon(playerData.currentWeapon);
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
        if (!playerController.CanInput() || !playerData.unlockedWeapons.Contains(nextWeapon))
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

    public void ActivateWeaponSprite(int weaponID)
    {
        frontWeaponSprites[weaponID].SetActive(true);
        backWeaponSprites[weaponID].SetActive(true);
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
    }
}
