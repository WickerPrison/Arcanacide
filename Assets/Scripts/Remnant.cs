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
    PlayerController playerController;
    WeaponManager weaponManager;
    float playerDistance = 100;
    float interactDistance = 2;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.GetComponent<PlayerController>();
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
                playerController.arcaneRemainsActive = true;
                playerScript.MaxHeal();
            }
            if (playerData.equippedEmblems.Contains(emblemLibrary.death_aura))
            {
                playerData.mana = playerData.maxMana;
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
                playerController.arcaneRemainsActive = true;
            }

            if (playerData.equippedEmblems.Contains(emblemLibrary.death_aura))
            {
                playerScript.deathAuraActive = true;
            }
        }

        if (playerData.tutorials.Contains("Remnant") && other.CompareTag("Player"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            tutorialManager.RemnantTutorial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains))
            {
                weaponManager.RemoveWeaponMagicSource();
                playerController.arcaneRemainsActive = false;
            }

            playerScript.deathAuraActive = false;
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
