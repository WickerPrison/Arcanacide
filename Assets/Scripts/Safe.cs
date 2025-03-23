using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FMODUnity;

public class Safe : MonoBehaviour
{
    [SerializeField] GameObject openMessage;
    [SerializeField] GameObject newEmblemMessage;
    [SerializeField] TextMeshProUGUI emblemMessageText;
    [SerializeField] PlayerData playerData;
    [SerializeField] string emblemName;
    [SerializeField] Patches patch;
    [SerializeField] Image newEmblemColorImage;
    [SerializeField] MapData mapData;
    [SerializeField] EventReference safeSFX;
    Transform player;
    TutorialManager tutorialManager;
    InputManager im;
    SoundManager sm;
    float playerDistance = 100;
    float interactDistance = 2;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        sm = im.gameObject.GetComponent<SoundManager>();
        im.controls.Gameplay.Interact.performed += ctx => Interaction();
        im.controls.Dialogue.Next.performed += ctx => CloseNewEmblemMessage();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        newEmblemMessage.SetActive(false);
        newEmblemColorImage.color = mapData.floorColor;
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance && !playerData.patches.Contains(patch))
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
        if(playerDistance <= interactDistance && !playerData.patches.Contains(patch))
        {
            RuntimeManager.PlayOneShot(safeSFX);
            playerData.patches.Add(patch);
            newEmblemMessage.SetActive(true);
            emblemMessageText.text = emblemName;
            im.Dialogue();
            Time.timeScale = 0;
        }
    }

    void CloseNewEmblemMessage()
    {
        sm.ButtonSound();
        if(playerDistance <= interactDistance)
        {
            newEmblemMessage.SetActive(false);
            im.Gameplay();
            Time.timeScale = 1;
            TriggerTutorial();
        }
    }

    void TriggerTutorial()
    {
        if (playerData.tutorials.Contains("Emblem"))
        {
            tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
            im.controls.Tutorial.Disable();
            tutorialManager.Tutorial("Emblem");
        }
    }
}
