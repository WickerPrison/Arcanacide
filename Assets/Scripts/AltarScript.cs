using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarScript : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject restMenuPrfab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int altarNumber;
    Transform player;
    PlayerController playerController;
    GameObject restMenu;
    RestMenuButtons restMenuButtons;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2)
        {
            message.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && Time.timeScale == 1)
            {
                OpenRestMenu();
            }
        }
        else
        {
            message.SetActive(false);
        }
    }

    void OpenRestMenu()
    {
        playerController.anyMenuOpen = true;
        playerController.preventInput = true;
        restMenu = Instantiate(restMenuPrfab);
        restMenuButtons = restMenu.GetComponent<RestMenuButtons>();
        restMenuButtons.altarNumber = altarNumber;
        restMenuButtons.spawnPoint = spawnPoint;
    }

}
