using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSiteScript : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject restMenuPrfab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int swordSiteNumber;
    [SerializeField] Vector3 mapPlayerFacePosition;
    Transform player;
    PlayerController playerController;
    GameObject restMenu;
    RestMenuButtons restMenuButtons;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;
    float swordSiteTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => OpenRestMenu();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(swordSiteTimer >= 0)
        {
            swordSiteTimer -= Time.deltaTime;
        }

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

    void OpenRestMenu()
    {
        if(swordSiteTimer > 0)
        {
            return;
        }

        if(playerDistance <= interactDistance)
        {
            restMenu = Instantiate(restMenuPrfab);
            restMenuButtons = restMenu.GetComponent<RestMenuButtons>();
            restMenuButtons.altarNumber = swordSiteNumber;
            restMenuButtons.spawnPoint = spawnPoint;
            restMenuButtons.mapPlayerFacePosition = mapPlayerFacePosition;
            im.Menu();
        }
    }
}
