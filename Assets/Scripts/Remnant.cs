using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remnant : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] GameObject message;
    Transform player;
    TutorialManager tutorialManager;
    PlayerControls controls;
    float playerDistance;
    float interactDistance = 2;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        controls = new PlayerControls();
        controls.Gameplay.Interact.performed += ctx => PickUpRemnant();
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
        if(playerDistance <= interactDistance)
        {
            playerData.money += playerData.lostMoney;
            playerData.lostMoney = 0;
            mapData.deathRoom = "none";
            if (playerData.path == "Dying")
            {
                PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
                playerController.EndPathOfTheDying();
                PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
                playerScript.DuckHeal();

            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerData.path == "Dying")
        {
            PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
            playerController.PathOfTheDying();
        }

        if (playerData.tutorials.Contains("Remnant") && other.CompareTag("Player"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            tutorialManager.RemnantTutorial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerData.path == "Dying")
        {
            PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
            playerController.EndPathOfTheDying();
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
