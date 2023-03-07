using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHealingGem : MonoBehaviour
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

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => PickUpHealthGem();
        tutorialManager = im.GetComponent<TutorialManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (mapData.unlockedDoors.Contains(3))
        {
            Destroy(gameObject);
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
            tutorialManager.HealTutorial();   
            mapData.unlockedDoors.Add(3);
            playerData.hasHealthGem = true;
            healthGemIcon.SetActive(true);
            Destroy(gameObject);
        }
    }
}
