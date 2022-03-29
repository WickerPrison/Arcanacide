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

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2)
        {
            message.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerData.money += playerData.lostMoney;
                playerData.lostMoney = 0;
                mapData.deathRoom = "none";
                if(playerData.path == "Dying")
                {
                    PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
                    playerController.EndPathOfTheDying();
                    PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
                    playerScript.DuckHeal();

                }
                Destroy(gameObject);
            }
        }
        else
        {
            message.SetActive(false);
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
}
