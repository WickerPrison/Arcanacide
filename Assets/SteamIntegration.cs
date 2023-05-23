using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamIntegration : MonoBehaviour
{
    [SerializeField] GameObject steamManagerPrefab;
    SteamManager steamManager;
    PlayerController playerController;
    bool pausedBySteam = false;

    // Start is called before the first frame update
    void Start()
    {
        steamManager = GameObject.FindGameObjectWithTag("SteamManager").GetComponent<SteamManager>();
        if(steamManager == null)
        {
            steamManager = Instantiate(steamManagerPrefab).GetComponent<SteamManager>();
        }

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController == null) return;

        if(SteamUtils.IsOverlayEnabled() && !pausedBySteam && !playerController.pauseMenu)
        {
            playerController.PauseMenu();
        }
        else if(!SteamUtils.IsOverlayEnabled() && pausedBySteam && playerController.pauseMenu)
        {
            playerController.PauseMenu();
        }
    }
}
