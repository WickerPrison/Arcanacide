using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] MapData mapData;
    [SerializeField] int switchNumber;
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
        if (mapData.usedAltars.Contains(switchNumber))
        {
            hasBeenUsed = true;
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
            mapData.powerSwitchesFlipped.Add(switchNumber);
        }
    }
}