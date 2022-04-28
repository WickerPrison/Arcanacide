using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileTicket : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] GameObject message;
    Transform player;
    InputManager im;
    AudioSource sfx;
    float playerDistance;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => FileSupportTicket();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sfx = GetComponent<AudioSource>();
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
            sfx.Play();
            mapData.ticketFiled = true;
        }
    }
}
