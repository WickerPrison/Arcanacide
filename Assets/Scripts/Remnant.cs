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
    float playerDistance = 100;
    float interactDistance = 2;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
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
                WeaponManager weaponManager = player.gameObject.GetComponent<WeaponManager>();
                weaponManager.RemoveWeaponMagicSource();
                PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
                playerController.arcaneRemainsActive = true;
                PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
                playerScript.MaxHeal();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains))
        {
            WeaponManager weaponManager = player.gameObject.GetComponent<WeaponManager>();
            weaponManager.AddWeaponMagicSource();
            PlayerController playerController = weaponManager.gameObject.GetComponent<PlayerController>();
            playerController.arcaneRemainsActive = true;
        }

        if (playerData.tutorials.Contains("Remnant") && other.CompareTag("Player"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            tutorialManager.RemnantTutorial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains))
        {
            WeaponManager weaponManager = player.gameObject.GetComponent<WeaponManager>();
            weaponManager.RemoveWeaponMagicSource();
            PlayerController playerController = weaponManager.gameObject.GetComponent<PlayerController>();
            playerController.arcaneRemainsActive = false;
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
