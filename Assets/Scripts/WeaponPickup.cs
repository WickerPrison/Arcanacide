using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerWeapon weapon;
    [SerializeField] WeaponElement weaponElement;
    [SerializeField] float interactDistance = 2;
    int weaponID;
    Transform player;
    InputManager im;
    TutorialManager tutorialManager;
    float playerDistance = 100;

    private void Awake()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
    }

    void Start()
    {
        weaponID = Utils.GetWeaponIndex(weapon);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (playerData.unlockedWeapons.Contains(weaponID) && playerData.GetWeaponUnlockList(weaponID).Contains(weaponElement))
        {
            Destroy(gameObject);
        }
    }


    private void Update()
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

    void PickUpWeapon(InputAction.CallbackContext context)
    {
        PerformPickup();
    }

    public void PerformPickup()
    {
        if (playerDistance <= interactDistance)
        {
            if (!playerData.unlockedWeapons.Contains(weaponID))
            {
                playerData.unlockedWeapons.Add(weaponID);
                playerData.equippedElements[weaponID] = weaponElement;
            }
            playerData.GetElementList(weaponID).Add(weaponElement);
            playerData.newWeapon = true;
            TriggerTutorial();
            Destroy(gameObject);
        }
    }

    void TriggerTutorial()
    {
        tutorialManager = im.gameObject.GetComponent<TutorialManager>();
        im.controls.Tutorial.Disable();
        tutorialManager.Tutorial("New Weapon");
    }

    private void OnEnable()
    {
        im.controls.Gameplay.Interact.performed += PickUpWeapon;
    }

    private void OnDisable()
    {
        im.controls.Gameplay.Interact.performed -= PickUpWeapon;
    }
}
