using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Safe : MonoBehaviour
{
    [SerializeField] GameObject openMessage;
    [SerializeField] GameObject newEmblemMessage;
    [SerializeField] TextMeshProUGUI emblemMessageText;
    [SerializeField] PlayerData playerData;
    [SerializeField] string emblemName;
    Transform player;
    TutorialManager tutorialManager;
    bool newEmblemMessageOpen = false;
    InputManager im;
    float playerDistance;
    float interactDistance = 2;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Interaction();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        newEmblemMessage.SetActive(false);
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance && !playerData.emblems.Contains(emblemName))
        {
            openMessage.SetActive(true);
        }
        else
        {
            openMessage.SetActive(false);
        }
    }

    void Interaction()
    {
        if (newEmblemMessageOpen)
        { 
            newEmblemMessageOpen = false;
            newEmblemMessage.SetActive(false);
            TriggerTutorial();
        }
        else if(playerDistance <= interactDistance && !playerData.emblems.Contains(emblemName))
        {
            playerData.emblems.Add(emblemName);
            newEmblemMessage.SetActive(true);
            newEmblemMessageOpen = true;
            emblemMessageText.text = "New Emblem: " + emblemName;
        }
    }

    void TriggerTutorial()
    {
        if (playerData.tutorials.Contains("Emblem"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            im.controls.Tutorial.Disable();
            tutorialManager.EmblemTutorial();
        }
    }
}
