using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remnant : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] GameObject message;
    [SerializeField] EmblemLibrary emblemLibrary;
    Transform player;
    TutorialManager tutorialManager;
    InputManager im;
    PlayerControls controls;
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
        
        controls = new PlayerControls();
        controls.Gameplay.Interact.performed += ctx => PickUpRemnant();

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

    void PickUpRemnant()
    {
        if(playerDistance <= interactDistance && im.controls.Gameplay.enabled)
        {
            playerData.money += playerData.lostMoney;
            playerData.lostMoney = 0;
            mapData.deathRoom = "none";
            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains))
            {
                weaponManager.RemoveWeaponMagicSource();
                patchEffects.arcaneRemainsActive = false;
                PlayerHealth playerHealth = playerScript.GetComponent<PlayerHealth>();
                playerHealth.MaxHeal();
            }
            if (playerData.equippedEmblems.Contains(emblemLibrary.death_aura))
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
            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains))
            {
                weaponManager.AddWeaponMagicSource();
                patchEffects.arcaneRemainsActive = true;
            }

            if (playerData.equippedEmblems.Contains(emblemLibrary.death_aura))
            {
                playerAnimation.StartBodyMagic();
                patchEffects.deathAuraActive = true;
            }
        }

        if (playerData.tutorials.Contains("Remnant") && other.CompareTag("Player"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            tutorialManager.Tutorial("Remnant", "null");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains))
            {
                weaponManager.RemoveWeaponMagicSource();
                patchEffects.arcaneRemainsActive = false;
            }

            if (playerData.equippedEmblems.Contains(emblemLibrary.death_aura))
            {
                playerAnimation.EndBodyMagic();
                patchEffects.deathAuraActive = false;
            }
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
