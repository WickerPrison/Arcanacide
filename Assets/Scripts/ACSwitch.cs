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
    [SerializeField] ParticleSystem wayFaerie;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => FlipSwitch();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (mapData.ACUsed)
        {
            wayFaerie.Stop();
            wayFaerie.Clear();
        }

        if (!mapData.ACOn)
        {
            readout.text = "Off";
        }
        else
        {
            readout.text = "MAX";
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= interactDistance)
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
        if (playerDistance <= interactDistance)
        {
            GlobalEvents.instance.ACWallSwitch();
            RuntimeManager.PlayOneShot(beep);
            mapData.ACUsed = true;
            wayFaerie.Stop();
        }
    }

    void SwitchAC(bool acOn)
    {
        mapData.ACOn = acOn;
        if (acOn)
        {
            readout.text = "MAX";
        }
        else
        {
            readout.text = "Off";
        }
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onSwitchAC += Global_onSwitchAC;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onSwitchAC -= Global_onSwitchAC;

    }

    private void Global_onSwitchAC(object sender, bool acOn)
    {
        SwitchAC(acOn);
    }
}
