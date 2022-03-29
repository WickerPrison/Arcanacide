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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        newEmblemMessage.SetActive(false);
    }

    void Update()
    {
        if (newEmblemMessageOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                newEmblemMessageOpen = false;
                newEmblemMessage.SetActive(false);
                TriggerTutorial();
            }
        }

        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2)
        {
            if (!playerData.emblems.Contains(emblemName))
            {
                openMessage.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerData.emblems.Add(emblemName);
                    newEmblemMessage.SetActive(true);
                    newEmblemMessageOpen = true;
                    emblemMessageText.text = "New Emblem: " + emblemName;
                }
            }
            else
            {
                openMessage.SetActive(false);
            }
        }
        else
        {
            openMessage.SetActive(false);
            if (newEmblemMessageOpen)
            {
                newEmblemMessageOpen = false;
                newEmblemMessage.SetActive(false);
                TriggerTutorial();
            }
        }
    }

    void TriggerTutorial()
    {
        if (playerData.tutorials.Contains("Emblem"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            tutorialManager.EmblemTutorial();
        }
    }
}
