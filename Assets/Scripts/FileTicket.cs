using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;
using System;

public class FileTicket : MonoBehaviour, IBlockDoors
{
    [SerializeField] MapData mapData;
    [SerializeField] BuildMode buildMode;
    [SerializeField] DialogueData phoneData;
    [SerializeField] GameObject message;
    [SerializeField] TextMeshProUGUI screenText;
    [SerializeField] ParticleSystem wayFaerie;
    string screenText1 = "File Support Ticket Here";
    string screenText2 = "Support Ticket Filed";
    Transform player;
    InputManager im;
    [SerializeField] EventReference sfx;
    float playerDistance;
    float interactDistance = 2;
    public event EventHandler<bool> onBlockDoor;

    enum TicketState
    {
        FILED, FILABLE, DISABLED
    }

    TicketState _ticketState;
    TicketState ticketState
    {
        get { return _ticketState; }
        set
        {
            switch (value)
            {
                case TicketState.FILED:
                    screenText.text = screenText2;
                    wayFaerie.Stop();
                    wayFaerie.Clear();
                    onBlockDoor?.Invoke(this, false);
                    break;
                case TicketState.FILABLE:
                    screenText.text = screenText1;
                    wayFaerie.Play();
                    break;
                case TicketState.DISABLED:
                    screenText.text = screenText1;
                    wayFaerie.Stop();
                    wayFaerie.Clear();
                    break;
            }
            _ticketState = value;
        }
    }

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => FileSupportTicket();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        onBlockDoor?.Invoke(this, true);

        if (mapData.ticketFiled)
        {
            ticketState = TicketState.FILED;
        }
        else if(!mapData.miniboss1Killed && buildMode.buildMode != BuildModes.DEMO)
        {
            ticketState = TicketState.DISABLED;
        }
        else
        {
            ticketState = TicketState.FILABLE;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance && ticketState == TicketState.FILABLE)
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
        if(playerDistance <= interactDistance && ticketState == TicketState.FILABLE)
        {
            RuntimeManager.PlayOneShot(sfx, 2);
            ticketState = TicketState.FILED;
            mapData.ticketFiled = true;
            screenText.text = screenText2;
            wayFaerie.Stop();
        }
    }
    private void onMinibossKilled(object sender, System.EventArgs e)
    {
        if(!mapData.ticketFiled)
        {
            ticketState = TicketState.FILABLE;
        }
    }


    private void OnEnable()
    {
        GlobalEvents.instance.onMinibossKilled += onMinibossKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onMinibossKilled -= onMinibossKilled;
    }
}
