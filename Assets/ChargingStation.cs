using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    bool hasBeenUsed = false;
    Transform player;
    PlayerScript playerScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerScript = player.GetComponent<PlayerScript>();
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
            }
        }
        else
        {
            message.SetActive(false);
        }
    }
}