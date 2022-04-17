using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    [SerializeField] int chargingStationID;
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] ParticleSystem particles;
    bool hasBeenUsed = false;
    Transform player;
    PlayerScript playerScript;
    InputManager im;
    TutorialManager tutorialManager;
    float playerDistance;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Charge();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerScript = player.GetComponent<PlayerScript>();
        if (mapData.usedChargingStations.Contains(chargingStationID))
        {
            hasBeenUsed = true;
            particles.Stop();
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance && !hasBeenUsed)
        {
            message.SetActive(true);
            if (playerData.tutorials.Contains("Altar"))
            {
                tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
                tutorialManager.AltarTutorial();
            }
        }
        else
        {
            message.SetActive(false);
        }
    }

    void Charge()
    {
        if(playerData.healCharges == playerData.maxHealCharges && playerData.health == playerData.MaxHealth())
        {
            return;
        }

        if(playerDistance <= interactDistance && !hasBeenUsed)
        {
            playerData.healCharges = playerData.maxHealCharges;
            playerData.duckCD = 0;
            playerScript.MaxHeal();
            hasBeenUsed = true;
            mapData.usedChargingStations.Add(chargingStationID);
            particles.Stop();
        }
    }
}
