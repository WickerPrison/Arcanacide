using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [SerializeField] int weaponID;
    [SerializeField] float interactDistance = 2;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (playerData.unlockedWeapons.Contains(weaponID))
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
            playerData.unlockedWeapons.Add(weaponID);
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
        Debug.Log(im.controls);
        im.controls.Gameplay.Interact.performed += PickUpWeapon;
    }

    private void OnDisable()
    {
        im.controls.Gameplay.Interact.performed -= PickUpWeapon;
    }
}
