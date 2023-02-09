using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [SerializeField] int weaponID;
    Transform player;
    InputManager im;
    TutorialManager tutorialManager;
    float playerDistance = 100;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => PickUpWeapon();
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

    void PickUpWeapon()
    {
        if (playerDistance <= interactDistance)
        {
            playerData.unlockedWeapons.Add(weaponID);
            TriggerTutorial();
            Destroy(gameObject);
        }
    }

    void TriggerTutorial()
    {
        if (playerData.tutorials.Contains("NewWeapon"))
        {
            tutorialManager = im.gameObject.GetComponent<TutorialManager>();
            im.controls.Tutorial.Disable();
            tutorialManager.NewWeaponTutorial();
        }
    }
}
