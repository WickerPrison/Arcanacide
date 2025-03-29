using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GetHealingGem : MonoBehaviour, IBlockDoors
{
    [SerializeField] GameObject message;
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject healthGemIcon;
    TutorialManager tutorialManager;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;
    public event EventHandler<bool> onBlockDoor;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => PickUpHealthGem();
        tutorialManager = im.GetComponent<TutorialManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (playerData.hasHealthGem)
        {
            Destroy(gameObject);
        }
        else
        {
            onBlockDoor?.Invoke(this, true);
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= interactDistance && !playerData.hasHealthGem)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void PickUpHealthGem()
    {
        if (playerDistance <= interactDistance && !playerData.hasHealthGem)
        {
            tutorialManager.Tutorial("Heal");
            onBlockDoor?.Invoke(this, false);
            playerData.hasHealthGem = true;
            healthGemIcon.SetActive(true);
            Destroy(gameObject);
        }
    }
}
