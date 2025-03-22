using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [SerializeField] bool canInteractWhileCombat = false;
    [System.NonSerialized] public bool active = true;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;
    public event EventHandler onInteracted;
    GameManager gm;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Interact();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (!canInteractWhileCombat)
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (CanInteract())
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    bool CanInteract()
    {
        if (!active) return false;
        if (playerDistance > interactDistance) return false;
        if (!canInteractWhileCombat && gm.awareEnemies > 0) return false;
        return true;
    }

    void Interact()
    {
        if (!CanInteract()) return;
        onInteracted?.Invoke(this, EventArgs.Empty);
    }
}
