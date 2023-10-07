using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class FileTicket : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData phoneData;
    [SerializeField] GameObject message;
    [SerializeField] TextMeshProUGUI screenText;
    string screenText1 = "File Support Ticket Here";
    string screenText2 = "Support Ticket Filed";
    Transform player;
    InputManager im;
    [SerializeField] EventReference sfx;
    float playerDistance;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => FileSupportTicket();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (mapData.ticketFiled)
        {
            screenText.text = screenText2;
        }
        else
        {
            screenText.text = screenText1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance && !mapData.ticketFiled)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void FileSupportTicket()
    {
        if(playerDistance <= interactDistance && !mapData.ticketFiled)
        {
            RuntimeManager.PlayOneShot(sfx, 2);
            mapData.ticketFiled = true;
            screenText.text = screenText2;
        }
    }
}
