using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamIntegration : MonoBehaviour
{
    [SerializeField] GameObject steamManagerPrefab;
    SteamManager steamManager;
    GameObject playerControllerObject;
    PlayerController playerController;
    bool pausedBySteam = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject steamManagerObject = GameObject.FindGameObjectWithTag("SteamManager");
        if(steamManagerObject == null)
        {
            steamManager = Instantiate(steamManagerPrefab).GetComponent<SteamManager>();
        }
        else
        {
            steamManager = steamManagerObject.GetComponent<SteamManager>();
        }

        playerControllerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }
    }

    /*
    private void Update()
    {
        if (playerControllerObject == null) return;

        if(SteamUtils.IsOverlayEnabled() && !pausedBySteam && !playerController.pauseMenu)
        {
            playerController.PauseMenu();
        }
        else if(!SteamUtils.IsOverlayEnabled() && pausedBySteam && playerController.pauseMenu)
        {
            playerController.PauseMenu();
        }
    }
    */
}
