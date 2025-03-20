using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [System.NonSerialized] public bool active = true;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;
    public event EventHandler onInteracted;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Interact();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= interactDistance && active)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void Interact()
    {
        if (!active) return;
        onInteracted?.Invoke(this, EventArgs.Empty);
    }
}
