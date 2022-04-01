using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject shopWindowPrefab;
    GameObject shopWindow;
    Transform player;
    PlayerController playerController;
    InputManager im;
    float playerDistance;
    float interactDistance = 2;

    // Start is called before the first frame update
    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => OpenShop();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.GetComponent<PlayerController>();
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

    void OpenShop()
    {
        if(playerDistance <= interactDistance)
        {
            im.Menu();
            playerController.preventInput = true;
            shopWindow = Instantiate(shopWindowPrefab);
        }
    }

    public void CloseShop()
    {
        playerController.preventInput = false;
        im.Gameplay();
        Destroy(shopWindow);
    }
}
