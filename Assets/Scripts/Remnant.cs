using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Remnant : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] GameObject message;
    [SerializeField] EmblemLibrary emblemLibrary;
    Transform player;
    TutorialManager tutorialManager;
    InputManager im;
    PlayerScript playerScript;
    PatchEffects patchEffects;
    PlayerAnimation playerAnimation;
    WeaponManager weaponManager;
    float playerDistance = 100;
    float interactDistance = 2;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        patchEffects = player.GetComponent<PatchEffects>();
        playerAnimation = player.GetComponent<PlayerAnimation>();
        playerScript = player.GetComponent<PlayerScript>();
        weaponManager = player.gameObject.GetComponent<WeaponManager>();
        
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void PickUpRemnant(InputAction.CallbackContext obj)
    {
        if(playerDistance <= interactDistance && im.controls.Gameplay.enabled)
        {
            GlobalEvents.instance.MoneyChange(playerData.money, playerData.lostMoney);
            playerData.lostMoney = 0;
            mapData.deathRoom = "none";
            if (playerData.equippedPatches.Contains(Patches.ARCANE_REMAINS))
            {
                weaponManager.RemoveWeaponMagicSource();
                patchEffects.arcaneRemainsActive = false;
                PlayerHealth playerHealth = playerScript.GetComponent<PlayerHealth>();
                playerHealth.MaxHeal();
            }
            if (playerData.equippedPatches.Contains(Patches.DEATH_AURA))
            {
                playerData.mana = playerData.maxMana;
                playerAnimation.EndBodyMagic();
                patchEffects.deathAuraActive = false;
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerData.equippedPatches.Contains(Patches.ARCANE_REMAINS))
            {
                weaponManager.AddWeaponMagicSource();
                patchEffects.arcaneRemainsActive = true;
            }

            if (playerData.equippedPatches.Contains(Patches.DEATH_AURA))
            {
                playerAnimation.StartBodyMagic();
                patchEffects.deathAuraActive = true;
            }
        }

        if (playerData.tutorials.Contains("Remnant") && other.CompareTag("Player"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            tutorialManager.Tutorial("Remnant");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerData.equippedPatches.Contains(Patches.ARCANE_REMAINS))
            {
                weaponManager.RemoveWeaponMagicSource();
                patchEffects.arcaneRemainsActive = false;
            }

            if (playerData.equippedPatches.Contains(Patches.DEATH_AURA))
            {
                playerAnimation.EndBodyMagic();
                patchEffects.deathAuraActive = false;
            }
        }
    }

    private void OnEnable()
    {
        im.controls.Gameplay.Interact.performed += PickUpRemnant;
    }

    private void OnDisable()
    {
        im.controls.Gameplay.Interact.performed -= PickUpRemnant;
    }
}
