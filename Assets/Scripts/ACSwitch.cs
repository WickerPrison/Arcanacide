using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class ACSwitch : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] MapData mapData;
    [SerializeField] TextMeshProUGUI readout;
    [SerializeField] EventReference beep;
    bool hasBeenUsed = false;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => FlipSwitch();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        readout.text = "Max";
        if (!mapData.ACOn)
        {
            hasBeenUsed = true;
            readout.text = "Off";
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= interactDistance && !hasBeenUsed)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void FlipSwitch()
    {
        if (playerDistance <= interactDistance && !hasBeenUsed)
        {
            hasBeenUsed = true;
            mapData.ACOn = false;
            readout.text = "Off";
            RuntimeManager.PlayOneShot(beep);
        }
    }
}
