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
    InputManager im;
    float playerDistance;
    float interactDistance = 2;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Interaction();
        im.controls.Dialogue.Next.performed += ctx => CloseNewEmblemMessage();
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
        if(playerDistance <= interactDistance && !playerData.emblems.Contains(emblemName))
        {
            playerData.emblems.Add(emblemName);
            newEmblemMessage.SetActive(true);
            emblemMessageText.text = "New Emblem: " + emblemName;
            im.Dialogue();
        }
    }

    void CloseNewEmblemMessage()
    {
        if(playerDistance <= interactDistance)
        {
            newEmblemMessage.SetActive(false);
            im.Gameplay();
            TriggerTutorial();
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
