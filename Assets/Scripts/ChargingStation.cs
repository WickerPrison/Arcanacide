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

    void Start()
    {
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
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2 && !hasBeenUsed && playerData.hasHealed)
        {
            message.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerData.hasHealed = false;
                playerData.duckCD = 0;
                playerScript.MaxHeal();
                hasBeenUsed = true;
                mapData.usedChargingStations.Add(chargingStationID);
                particles.Stop();
            }
        }
        else
        {
            message.SetActive(false);
        }
    }
}
