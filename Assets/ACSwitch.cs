using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACSwitch : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] MapData mapData;
    [SerializeField] Animator animator;
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
        if (!mapData.ACOn)
        {
            hasBeenUsed = true;
            animator.Play("SwitchDown");
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
            animator.Play("Handle|Pull_Down");
        }
    }
}
